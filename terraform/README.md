# Terraform Deployment

Current deployment is divided into modules:

main
└── modules/
    └── backend     # upgrade the backend
    └── cluster     # start the cluster
    └── publish     # build and publish image

To run, you need to specify a `.tfvars` file to handle the variables,
the recommended setup is to have a file `dev.tfvars`, `staging.tfvars` and `prod.tfvars`.
These variables is what determine which environment to deploy to.

> [!INFO]
> For this project, the only difference between enviroments is the handling of the variables,
> in more complex cases, a different approach with something like *Terraform wokspaces* is recommended.


To execute, run:

```bash
    terraform init
    terraform apply -var-file=dev.tfvars
``

Approve the changes and the deployment will start.

## License

See the Terraform [LICENSE](https://github.com/hashicorp/terraform/blob/main/LICENSE) for reference.