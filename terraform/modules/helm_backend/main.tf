
resource "null_resource" "helm_backend" {
  depends_on = [var.backend_image_depends_on]

  provisioner "local-exec" {
    command = "helm upgrade --install backend ../../kubernetes/helm-chart --set image.repository=${var.local_registry} --set image.tag=latest --kube-context minikube"
  }
}

variable "backend_image_depends_on" {
  description = "Dependency for backend image build"
}


