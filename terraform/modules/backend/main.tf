
resource "null_resource" "backend" {
  depends_on = [var.dependency]

  provisioner "local-exec" {
    command = "helm upgrade --install backend ${path.module}/../../../kubernetes/application-helm-chart --set image.repository=${var.docker_registry}/${var.api_image} --set image.tag=${var.api_image_tag} --kube-context minikube"
  }
}


