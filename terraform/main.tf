terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "~> 3.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.0"
    }
  }
}

provider "docker" {}

provider "kubernetes" {
  config_path    = pathexpand("~/.kube/config")
  config_context = "minikube"
}

# Start minikube with the local registry addon enabled
resource "null_resource" "minikube_start" {
  provisioner "local-exec" {
    command = "minikube start --driver=docker && minikube addons enable registry"
  }
  triggers = {
    always_run = "${timestamp()}"
  }
}

# Build and push the backend image to the local minikube registry
resource "null_resource" "build_and_push_backend" {
  depends_on = [null_resource.minikube_start]

  provisioner "local-exec" {
    command = "${path.module}/scripts/build_and_push.sh"
    interpreter = ["bash", "-c"]
  }
}

# Deploy the backend Helm chart, using the local registry image
resource "null_resource" "helm_backend" {
  depends_on = [null_resource.build_and_push_backend]

  provisioner "local-exec" {
    command = "helm upgrade --install backend ../kubernetes/backend --set image.repository=localhost:5000/store-api --set image.tag=latest --kube-context minikube"
  }
}