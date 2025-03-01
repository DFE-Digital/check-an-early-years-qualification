# Azure Storage Module

This module provisions a new Azure blob storage to be used by the Web Application.

<!-- BEGIN_TF_DOCS -->
## Requirements

No requirements.

## Providers

| Name | Version |
|------|---------|
| <a name="provider_azurerm"></a> [azurerm](#provider\_azurerm) | n/a |

## Modules

No modules.

## Resources

| Name | Type |
|------|------|
| [azurerm_key_vault_secret.storage_connection_string](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_storage_account.sa](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/storage_account) | resource |
| [azurerm_storage_account_network_rules.sa_network_rules](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/storage_account_network_rules) | resource |
| [azurerm_storage_container.data_protection](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/storage_container) | resource |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_kv_id"></a> [kv\_id](#input\_kv\_id) | The ID of the Key Vault | `string` | n/a | yes |
| <a name="input_location"></a> [location](#input\_location) | Name of the Azure region to deploy resources | `string` | n/a | yes |
| <a name="input_resource_group"></a> [resource\_group](#input\_resource\_group) | Name of the Azure Resource Group to deploy resources | `string` | n/a | yes |
| <a name="input_tags"></a> [tags](#input\_tags) | Resource tags | `map(string)` | n/a | yes |
| <a name="input_webapp_storage_account_name"></a> [webapp\_storage\_account\_name](#input\_webapp\_storage\_account\_name) | Storage Account name | `string` | n/a | yes |
| <a name="input_webapp_subnet_id"></a> [webapp\_subnet\_id](#input\_webapp\_subnet\_id) | The ID of the WebApp Subnet | `string` | n/a | yes |

## Outputs

No outputs.
<!-- END_TF_DOCS -->