FROM jenkins/jenkins:lts
USER root

RUN apt-get update && apt-get install -y gnupg software-properties-common wget lsb-release apt-transport-https ca-certificates

# Terraform keys
RUN wget -O- https://apt.releases.hashicorp.com/gpg | gpg --dearmor > /usr/share/keyrings/hashicorp-archive-keyring.gpg
RUN echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/hashicorp-archive-keyring.gpg] https://apt.releases.hashicorp.com $(lsb_release -cs) main" > /etc/apt/sources.list.d/hashicorp.list

# Docker keys
RUN install -m 0755 -d /etc/apt/keyrings
RUN curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc
RUN chmod a+r /etc/apt/keyrings/docker.asc

RUN echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/debian \
  $(. /etc/os-release && echo "$VERSION_CODENAME") stable" | \
  tee /etc/apt/sources.list.d/docker.list > /dev/null

# Test
RUN whoami
RUN awk -F: '{ print $1 }' /etc/passwd

RUN groupadd -g 1001 docker || true
RUN usermod -aG docker jenkins

# Download minikube
RUN curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64

# Install packages
RUN install minikube-linux-amd64 /usr/local/bin/minikube
RUN apt-get update && apt-get install -y terraform docker-ce

USER jenkins