
resource "null_resource" "helm_backend" {
  depends_on = [var.depends_on_application]

  provisioner "local-exec" {
    command = "helm upgrade --install backend ${path.module}/../../../kubernetes/helm-chart --set image.repository=${var.docker_registry}/${var.api_image} --set image.tag=${var.api_image_tag} --kube-context minikube"
  }
}


