resource "null_resource" "build_and_push_backend" {
  depends_on = [var.dependency]

  provisioner "local-exec" {
    command = <<EOT
      eval $(minikube -p minikube docker-env)
      image_path=${var.docker_registry}/${var.api_image}:${var.api_image_tag}
      docker build -t $image_path ${path.module}/../../../application/backend
      docker push $image_path
    EOT
    interpreter = ["bash", "-c"]
  }
}
