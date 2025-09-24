resource "null_resource" "minikube_start" {
  provisioner "local-exec" {
    command = "minikube start --driver=docker && minikube addons enable registry"
  }
  triggers = {
    always_run = "${timestamp()}"
  }
}
