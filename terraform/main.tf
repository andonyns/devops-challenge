provider "docker" {}

provider "kubernetes" {
  config_path    = pathexpand("~/.kube/config")
  config_context = "minikube"
}

module "minikube" {
  source = "./modules/minikube"
}

module "build_application" {
  source = "./modules/application"
  depends_on_minikube = [module.minikube]
  docker_registry = var.docker_registry
  api_image = var.api_image
  api_image_tag = var.api_image_tag
}

module "helm_backend" {
  source = "./modules/helm_backend"
  depends_on_application = [module.build_application]
  docker_registry = var.docker_registry
  api_image = var.api_image
  api_image_tag = var.api_image_tag
}