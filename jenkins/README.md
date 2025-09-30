# Jenkins

CI/CD to run local with Jenkins.

## Setup

Build a local version of Jenkins that integrate the plugins included in *plugins.txt*

```bash
docker build -t local-jenkins:latest .
```

To run, execute:

```bash
docker run -d -p 8050:8080 -p 50000:50000 \
  -v jenkins_home:/var/jenkins_home \
  -v /var/run/docker.sock:/var/run/docker.sock \
  --name jenkins local-jenkins:latest
```

Browse http://localhost:8050 to configure Jenkins.
When requested for and Administrator password, run:

```bash
docker ps #copy the id for jenkins
docker logs <id>
```

Copy the generated password and paste it in Jenkins.

<img src="docs/images/Jenkins-initial.png" alt="Jenkins Initial Setup" />

Click on Install suggested plugins and wait for the installation to finish.

Enter the Admin User information and click 'Save and Continue'.
Leave the Jenkins URL as is and click 'Save'.
Start using Jenkins.


## Add Job

Click on 'Create a Job'
Enter a name and select 'Pipeline'
Click on GitHub project and enter the repo URL.
In Pipeline, copy and past the Jenkinsfile information or select 'Pipeline script from SCM' and fill the information.