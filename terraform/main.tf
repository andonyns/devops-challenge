provider "docker" {}

provider "kubernetes" {
  config_path    = pathexpand("~/.kube/config")
  config_context = "minikube"
}

module "minikube" {
  source = "./modules/cluster"
}

module "build_application" {
  source = "./modules/publish"
  dependency = [module.minikube]
  docker_registry = var.docker_registry
  api_image = var.api_image
  api_image_tag = var.api_image_tag
}

module "backend" {
  source = "./modules/backend"
  dependency = [module.build_application]
  docker_registry = var.docker_registry
  api_image = var.api_image
  api_image_tag = var.api_image_tag
}