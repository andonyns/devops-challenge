# Setup

There are two ways to configure the application, using Docker Compose or using Kubernetes with Minikube (recommended).

## Prerequisites

### General Requirements
1. [Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/)
2. [Helm](https://helm.sh/docs/intro/install/)
3. [Terraform](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli)
4. [Minikube](https://minikube.sigs.k8s.io/docs/start/)

### Local Jenkins in Docker
1. Navigate to the /jenkins folder
2. Pull the Jenkins Docker image:
   ```bash
   docker build -t local-jenkins:latest .
   ```
3. Run Jenkins in a Docker container:
   ```bash
   docker run -d -p 8800:8080 -p 50000:50000 \
     -v jenkins_home:/var/jenkins_home \
     --name jenkins \
     local-jenkins:latest
   ```
3. Access Jenkins at `http://localhost:8800`.
4. Follow the on-screen instructions to complete the setup.

See the Jenkins folder [README](./jenkins/README.md) for more information

### Local Docker Registry
1. Run a local Docker registry:
   ```bash
   docker run -d -p 5000:5000 --name registry registry:2
   ```
2. Push images to the local registry using the tag `localhost:5000/<image-name>`.

### Terraform Prerequisites
1. Install Terraform:
   - Follow the [official installation guide](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli).
2. Initialize Terraform in the project directory:
   ```bash
   terraform init
   ```
3. Verify the installation:
   ```bash
   terraform version
   ```

## Docker Compose

Ideal for local development and quick setup.

### How to

From the `/application/` folder, run:
```bash
    docker compose up -d
```

## Kubernetes with Minikube (Recommended)

The best way to use is through Terraform, which sets up:

1. The minikube provisioning.
2. Builds the application and pushes to the local registry.
3. Upgrades the Helm charts to deploy the cluster. 

### How to

From the terraform folder, Run `terraform apply` to setup the infrastructure.