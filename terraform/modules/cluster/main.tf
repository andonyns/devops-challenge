resource "null_resource" "start_cluster" {
  provisioner "local-exec" {
    command = "minikube start --driver=docker && minikube addons enable registry"
  }
  triggers = {
    always_run = "${timestamp()}"
  }
}
