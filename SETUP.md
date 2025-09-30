# Setup

There are two ways to configure the application, using Docker Compose or using Kubernetes with Minikube (recommended).

## Prerequisites

### General Requirements
1. [Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/)
2. [Helm](https://helm.sh/docs/intro/install/)
3. [Terraform](https://developer.hashicorp.com/terraform/tutorials/aws-get-started/install-cli)
4. [Minikube](https://minikube.sigs.k8s.io/docs/start/)

### Local Jenkins in Docker
1. Pull the Jenkins Docker image:
   ```bash
   docker pull jenkins/jenkins:lts
   ```
2. Run Jenkins in a Docker container:
   ```bash
   docker run -d -p 8080:8080 -p 50000:50000 \
     -v jenkins_home:/var/jenkins_home \
     --name jenkins \
     jenkins/jenkins:lts
   ```
3. Access Jenkins at `http://localhost:8080`.
4. Follow the on-screen instructions to complete the setup.

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

### How to

See the [application README](./application/README.md) for detailed information.

From the `/application/` folder, run:
```bash
    docker compose up -d
```

## Kubernetes with Minikube (Recommended)

### How to

1. Start Minikube:
   ```bash
   minikube start
   ```
2. Enable required addons:
   ```bash
   minikube addons enable ingress
   minikube addons enable registry
   ```
3. Deploy the application and monitoring stack using Helm:
   ```bash
   helm install logging-stack kubernetes/logging-helm-chart
   helm install application kubernetes/application-helm-chart
   ```
4. Access the application:
   - **Store API**: `http://store-api.local`
   - **Grafana**: `http://localhost:30300` (admin/admin)
   - **Minikube Dashboard**: `minikube dashboard`