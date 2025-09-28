variable "docker_registry" {
  description = "Docker registry to push images to"
  type        = string
  default     = "localhost:5000"
}

variable "api_image" {
  description = "The docker image name for the backend API"
  type        = string
  default     = "store-api"
}

variable "api_image_tag" {
  description = "The docker image tag for the backend API"
  type        = string
  default     = "latest"
}
