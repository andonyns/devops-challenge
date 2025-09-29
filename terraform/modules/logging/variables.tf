variable "dependency" {
  description = "Dependencies that need to be resolved before this module"
  type        = list(any)
  default     = []
}