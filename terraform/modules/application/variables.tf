variable "depends_on_minikube" {
  description = "Dependency for minikube startup"
}

variable "docker_registry" {
  description = "Docker registry to push images to"
  type        = string
}

variable "api_image" {
  description = "API docker image name"
  type        = string
}

variable "api_image_tag" {
	description = "The tag for the backend API"
	type        = string
}
