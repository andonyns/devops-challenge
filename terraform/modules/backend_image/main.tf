resource "null_resource" "build_and_push_backend" {
  depends_on = [var.minikube_depends_on]

  provisioner "local-exec" {
    command = <<EOT
      eval $(minikube -p minikube docker-env)
      docker build -t localhost:5000/store-api:latest ${path.module}/../../../application/backend
      docker push localhost:5000/store-api:latest
    EOT
    interpreter = ["bash", "-c"]
  }
}

variable "minikube_depends_on" {
  description = "Dependency for minikube startup"
}
