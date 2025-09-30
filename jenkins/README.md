# Jenkins

## Overview
This directory contains the configuration for running Jenkins locally in a Docker container. Jenkins is used for the CI/CD pipeline to automate builds, tests, and deployments.

## Features
- **Plugins**: Pre-configured plugins for Terraform, Docker, and Kubernetes.
- **Pipeline**: Automated CI/CD pipeline for infrastructure and application deployment.

## Setup
1. Build the Jenkins Docker image:
   ```bash
   docker build -t local-jenkins:latest .
   ```
2. Run the Jenkins container:
   ```bash
   docker run -d -p 8050:8080 -p 50000:50000 \
     -v jenkins_home:/var/jenkins_home \
     -v /var/run/docker.sock:/var/run/docker.sock \
     --name jenkins local-jenkins:latest
   ```
3. Access Jenkins at `http://localhost:8050`.
4. Retrieve the administrator password:
   ```bash
   docker logs <container-id>
   ```
5. Complete the Jenkins setup by installing suggested plugins and creating an admin user.


## Add Job

Click on 'Create a Job'
Enter a name and select 'Pipeline'
Click on GitHub project and enter the repo URL.
In Pipeline, copy and past the Jenkinsfile information or select 'Pipeline script from SCM' and fill the information.