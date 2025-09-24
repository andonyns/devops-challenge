provider "docker" {}

provider "kubernetes" {
  config_path    = pathexpand("~/.kube/config")
  config_context = "minikube"
}

module "minikube" {
  source = "./modules/minikube"
}

module "backend_image" {
  source = "./modules/backend_image"
  minikube_depends_on = [module.minikube]
}

module "helm_backend" {
  source = "./modules/helm_backend"
  backend_image_depends_on = [module.backend_image]
  local_registry = var.local_registry
}