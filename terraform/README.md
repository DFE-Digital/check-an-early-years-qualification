# Azure Root Module

This module provisions a new Azure Resource Group that assembles together the infrastructure for hosting the web application.

## Prerequisites
* The [Remote State module](./azure-remote-state/README.md) must first be applied **independently** before this configuration can be used.

<!-- BEGIN_TF_DOCS -->
## Requirements

| Name | Version |
|------|---------|
| <a name="requirement_terraform"></a> [terraform](#requirement\_terraform) | >= 1.5.0 |
| <a name="requirement_azurerm"></a> [azurerm](#requirement\_azurerm) | = 3.71.0 |

## Providers

| Name | Version |
|------|---------|
| <a name="provider_azurerm"></a> [azurerm](#provider\_azurerm) | = 3.71.0 |

## Modules

| Name | Source | Version |
|------|--------|---------|
| <a name="module_network"></a> [network](#module\_network) | ./modules/azure-network | n/a |
| <a name="module_storage"></a> [storage](#module\_storage) | ./modules/azure-storage | n/a |
| <a name="module_webapp"></a> [webapp](#module\_webapp) | ./modules/azure-web | n/a |

## Resources

| Name | Type |
|------|------|
| [azurerm_resource_group.rg](https://registry.terraform.io/providers/hashicorp/azurerm/3.71.0/docs/resources/resource_group) | resource |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_admin_email_address"></a> [admin\_email\_address](#input\_admin\_email\_address) | Email Address of the Admin | `string` | n/a | yes |
| <a name="input_as_service_principal_object_id"></a> [as\_service\_principal\_object\_id](#input\_as\_service\_principal\_object\_id) | Object ID of the service principal for App Service | `string` | n/a | yes |
| <a name="input_asp_sku"></a> [asp\_sku](#input\_asp\_sku) | SKU name for the App Service Plan | `string` | `"S1"` | no |
| <a name="input_azure_region"></a> [azure\_region](#input\_azure\_region) | Name of the Azure region to deploy resources | `string` | `"westeurope"` | no |
| <a name="input_custom_domain_name"></a> [custom\_domain\_name](#input\_custom\_domain\_name) | Custom domain hostname | `string` | n/a | yes |
| <a name="input_environment"></a> [environment](#input\_environment) | Environment to deploy resources | `string` | `"development"` | no |
| <a name="input_kv_certificate_authority_admin_first_name"></a> [kv\_certificate\_authority\_admin\_first\_name](#input\_kv\_certificate\_authority\_admin\_first\_name) | First Name of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_last_name"></a> [kv\_certificate\_authority\_admin\_last\_name](#input\_kv\_certificate\_authority\_admin\_last\_name) | Last Name of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_phone_no"></a> [kv\_certificate\_authority\_admin\_phone\_no](#input\_kv\_certificate\_authority\_admin\_phone\_no) | Phone No. of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_password"></a> [kv\_certificate\_authority\_password](#input\_kv\_certificate\_authority\_password) | Password the Certificate provider | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_username"></a> [kv\_certificate\_authority\_username](#input\_kv\_certificate\_authority\_username) | Username for the Certificate provider | `string` | n/a | yes |
| <a name="input_kv_certificate_label"></a> [kv\_certificate\_label](#input\_kv\_certificate\_label) | Label for the Certificate | `string` | n/a | yes |
| <a name="input_kv_certificate_subject"></a> [kv\_certificate\_subject](#input\_kv\_certificate\_subject) | Subject of the Certificate | `string` | n/a | yes |
| <a name="input_resource_name_prefix"></a> [resource\_name\_prefix](#input\_resource\_name\_prefix) | Prefix for resource names | `string` | n/a | yes |
| <a name="input_tracking_id"></a> [tracking\_id](#input\_tracking\_id) | Google Tag Manager tracking ID | `string` | n/a | yes |
| <a name="input_webapp_docker_image"></a> [webapp\_docker\_image](#input\_webapp\_docker\_image) | Docker Image to deploy | `string` | n/a | yes |
| <a name="input_webapp_docker_image_tag"></a> [webapp\_docker\_image\_tag](#input\_webapp\_docker\_image\_tag) | Tag for the Docker Image | `string` | `"latest"` | no |
| <a name="input_webapp_docker_registry_url"></a> [webapp\_docker\_registry\_url](#input\_webapp\_docker\_registry\_url) | URL to the Docker Registry | `string` | n/a | yes |
| <a name="input_webapp_name"></a> [webapp\_name](#input\_webapp\_name) | Name for the Web Application | `string` | n/a | yes |
| <a name="input_webapp_worker_count"></a> [webapp\_worker\_count](#input\_webapp\_worker\_count) | Number of Workers for the App Service Plan | `string` | `1` | no |

## Outputs

No outputs.
<!-- END_TF_DOCS -->