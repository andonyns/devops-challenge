resource "null_resource" "logging" {
  depends_on = [var.dependency]

  provisioner "local-exec" {
    command = "helm upgrade --install logging-stack ${path.module}/../../../kubernetes/logging-helm-chart --create-namespace --namespace logging --kube-context minikube --wait --timeout=10m"
  }

  provisioner "local-exec" {
    when    = destroy
    command = "helm uninstall logging-stack --namespace logging --kube-context minikube || true"
  }
}