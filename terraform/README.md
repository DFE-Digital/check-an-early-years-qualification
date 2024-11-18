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
| <a name="module_alerts"></a> [alerts](#module\_alerts) | ./modules/azure-alerts | n/a |
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
| <a name="input_asp_sku"></a> [asp\_sku](#input\_asp\_sku) | SKU name for the App Service Plan | `string` | n/a | yes |
| <a name="input_azure_region"></a> [azure\_region](#input\_azure\_region) | Name of the Azure region to deploy resources | `string` | `"westeurope"` | no |
| <a name="input_clarity_tag"></a> [clarity\_tag](#input\_clarity\_tag) | The Microsoft Clarity tag | `string` | `""` | no |
| <a name="input_contentful_delivery_api_key"></a> [contentful\_delivery\_api\_key](#input\_contentful\_delivery\_api\_key) | Contentful delivery API key | `string` | n/a | yes |
| <a name="input_contentful_preview_api_key"></a> [contentful\_preview\_api\_key](#input\_contentful\_preview\_api\_key) | Contentful preview API key | `string` | n/a | yes |
| <a name="input_contentful_space_id"></a> [contentful\_space\_id](#input\_contentful\_space\_id) | Contentful space ID | `string` | n/a | yes |
| <a name="input_contentful_use_preview_api"></a> [contentful\_use\_preview\_api](#input\_contentful\_use\_preview\_api) | Boolean used to set whether content is preview or published | `bool` | n/a | yes |
| <a name="input_custom_domain_name"></a> [custom\_domain\_name](#input\_custom\_domain\_name) | Custom domain hostname | `string` | n/a | yes |
| <a name="input_environment"></a> [environment](#input\_environment) | Environment to deploy resources | `string` | `"development"` | no |
| <a name="input_gtm_tag"></a> [gtm\_tag](#input\_gtm\_tag) | The Google Analytics tag | `string` | `""` | no |
| <a name="input_kv_certificate_authority_admin_first_name"></a> [kv\_certificate\_authority\_admin\_first\_name](#input\_kv\_certificate\_authority\_admin\_first\_name) | First Name of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_last_name"></a> [kv\_certificate\_authority\_admin\_last\_name](#input\_kv\_certificate\_authority\_admin\_last\_name) | Last Name of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_phone_no"></a> [kv\_certificate\_authority\_admin\_phone\_no](#input\_kv\_certificate\_authority\_admin\_phone\_no) | Phone No. of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_password"></a> [kv\_certificate\_authority\_password](#input\_kv\_certificate\_authority\_password) | Password the Certificate provider | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_username"></a> [kv\_certificate\_authority\_username](#input\_kv\_certificate\_authority\_username) | Username for the Certificate provider | `string` | n/a | yes |
| <a name="input_kv_certificate_label"></a> [kv\_certificate\_label](#input\_kv\_certificate\_label) | Label for the Certificate | `string` | n/a | yes |
| <a name="input_kv_certificate_subject"></a> [kv\_certificate\_subject](#input\_kv\_certificate\_subject) | Subject of the Certificate | `string` | n/a | yes |
| <a name="input_resource_name_prefix"></a> [resource\_name\_prefix](#input\_resource\_name\_prefix) | Prefix for resource names | `string` | n/a | yes |
| <a name="input_webapp_access_is_public"></a> [webapp\_access\_is\_public](#input\_webapp\_access\_is\_public) | Web app service is public, and access is unchallenged | `bool` | `false` | no |
| <a name="input_webapp_access_key_1"></a> [webapp\_access\_key\_1](#input\_webapp\_access\_key\_1) | Web app access key for invited access 1 | `string` | n/a | yes |
| <a name="input_webapp_access_key_2"></a> [webapp\_access\_key\_2](#input\_webapp\_access\_key\_2) | Web app access key for invited access 2 | `string` | n/a | yes |
| <a name="input_webapp_docker_image"></a> [webapp\_docker\_image](#input\_webapp\_docker\_image) | Docker Image to deploy | `string` | n/a | yes |
| <a name="input_webapp_docker_image_tag"></a> [webapp\_docker\_image\_tag](#input\_webapp\_docker\_image\_tag) | Tag for the Docker Image | `string` | `"latest"` | no |
| <a name="input_webapp_docker_registry_url"></a> [webapp\_docker\_registry\_url](#input\_webapp\_docker\_registry\_url) | URL to the Docker Registry | `string` | n/a | yes |
| <a name="input_webapp_e2e_access_key"></a> [webapp\_e2e\_access\_key](#input\_webapp\_e2e\_access\_key) | Web app access key for automated end-to-end tests | `string` | n/a | yes |
| <a name="input_webapp_name"></a> [webapp\_name](#input\_webapp\_name) | Name for the Web Application | `string` | n/a | yes |
| <a name="input_webapp_slot_name"></a> [webapp\_slot\_name](#input\_webapp\_slot\_name) | Name for the slot for the Web Application | `string` | `"green"` | no |
| <a name="input_webapp_storage_account_name"></a> [webapp\_storage\_account\_name](#input\_webapp\_storage\_account\_name) | Storage Account name | `string` | n/a | yes |
| <a name="input_webapp_team_access_key"></a> [webapp\_team\_access\_key](#input\_webapp\_team\_access\_key) | Web app access key for the service team | `string` | n/a | yes |
| <a name="input_webapp_worker_count"></a> [webapp\_worker\_count](#input\_webapp\_worker\_count) | Number of Workers for the App Service Plan | `string` | `1` | no |

## Outputs

No outputs.
<!-- END_TF_DOCS -->