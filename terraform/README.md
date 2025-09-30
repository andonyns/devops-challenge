# Terraform Deployment

## Overview
This repository uses Terraform to provision and manage the infrastructure for the DevOps Challenge. The deployment is modularized for flexibility and scalability.

## Modules
- **Cluster Module**: Starts the Minikube cluster and enables required addons.
- **Backend Module**: Deploys the backend application using Helm charts.
- **Logging Module**: Deploys the logging stack (Promtail, Loki, Grafana) using Helm charts.
- **Publish Module**: Builds and pushes Docker images to the registry.

## Prerequisites
1. Install Terraform: [Terraform Installation Guide](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli)
2. Configure a `.tfvars` file for your environment (e.g., `dev.tfvars`, `staging.tfvars`, `prod.tfvars`).

## Usage
1. Initialize Terraform:
   ```bash
   terraform init
   ```
2. Plan and apply the infrastructure:
   ```bash
   terraform plan -var-file=dev.tfvars
   terraform apply -var-file=dev.tfvars
   ```
3. Approve the changes to start the deployment.

## License
See the Terraform [LICENSE](https://github.com/hashicorp/terraform/blob/main/LICENSE) for reference.