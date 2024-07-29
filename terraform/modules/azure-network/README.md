# Azure Network Module

This module provisions the necessary network related resources such as a VNET, Subnets & KeyVault.

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
| [azurerm_key_vault.kv](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault) | resource |
| [azurerm_key_vault_access_policy.kv_gh_ap](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_access_policy) | resource |
| [azurerm_key_vault_access_policy.kv_mi_ap](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_access_policy) | resource |
| [azurerm_key_vault_certificate.kv_cert](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_certificate) | resource |
| [azurerm_key_vault_certificate_issuer.kv_ca](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_certificate_issuer) | resource |
| [azurerm_key_vault_key.data-protection](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_key) | resource |
| [azurerm_key_vault_secret.contentful_delivery_api_key](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.contentful_preview_api_key](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_key_vault_secret.contentful_space_id](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_secret) | resource |
| [azurerm_public_ip.agw_pip](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/public_ip) | resource |
| [azurerm_subnet.agw_snet](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/subnet) | resource |
| [azurerm_subnet.webapp_snet](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/subnet) | resource |
| [azurerm_user_assigned_identity.kv_mi](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/user_assigned_identity) | resource |
| [azurerm_virtual_network.vnet](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/virtual_network) | resource |
| [azurerm_client_config.az_config](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/data-sources/client_config) | data source |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_contentful_delivery_api_key"></a> [contentful\_delivery\_api\_key](#input\_contentful\_delivery\_api\_key) | Contentful delivery API key | `string` | n/a | yes |
| <a name="input_contentful_preview_api_key"></a> [contentful\_preview\_api\_key](#input\_contentful\_preview\_api\_key) | Contentful preview API key | `string` | n/a | yes |
| <a name="input_contentful_space_id"></a> [contentful\_space\_id](#input\_contentful\_space\_id) | Contentful space ID | `string` | n/a | yes |
| <a name="input_environment"></a> [environment](#input\_environment) | Environment to deploy resources | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_email"></a> [kv\_certificate\_authority\_admin\_email](#input\_kv\_certificate\_authority\_admin\_email) | Email Address of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_first_name"></a> [kv\_certificate\_authority\_admin\_first\_name](#input\_kv\_certificate\_authority\_admin\_first\_name) | First Name of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_last_name"></a> [kv\_certificate\_authority\_admin\_last\_name](#input\_kv\_certificate\_authority\_admin\_last\_name) | Last Name of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_admin_phone_no"></a> [kv\_certificate\_authority\_admin\_phone\_no](#input\_kv\_certificate\_authority\_admin\_phone\_no) | Phone No. of the Certificate Authority Admin | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_label"></a> [kv\_certificate\_authority\_label](#input\_kv\_certificate\_authority\_label) | Label for the Certificate Authority | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_name"></a> [kv\_certificate\_authority\_name](#input\_kv\_certificate\_authority\_name) | Name of the Certificate Authority | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_password"></a> [kv\_certificate\_authority\_password](#input\_kv\_certificate\_authority\_password) | Password the Certificate provider | `string` | n/a | yes |
| <a name="input_kv_certificate_authority_username"></a> [kv\_certificate\_authority\_username](#input\_kv\_certificate\_authority\_username) | Username for the Certificate provider | `string` | n/a | yes |
| <a name="input_kv_certificate_label"></a> [kv\_certificate\_label](#input\_kv\_certificate\_label) | Label for the Certificate | `string` | n/a | yes |
| <a name="input_kv_certificate_subject"></a> [kv\_certificate\_subject](#input\_kv\_certificate\_subject) | Subject of the Certificate | `string` | n/a | yes |
| <a name="input_location"></a> [location](#input\_location) | Name of the Azure region to deploy resources | `string` | n/a | yes |
| <a name="input_resource_group"></a> [resource\_group](#input\_resource\_group) | Name of the Azure Resource Group to deploy resources | `string` | n/a | yes |
| <a name="input_resource_name_prefix"></a> [resource\_name\_prefix](#input\_resource\_name\_prefix) | Prefix for resource names | `string` | n/a | yes |

## Outputs

| Name | Description |
|------|-------------|
| <a name="output_agw_pip_id"></a> [agw\_pip\_id](#output\_agw\_pip\_id) | ID of the Public IP address for the App Gateway |
| <a name="output_agw_subnet_id"></a> [agw\_subnet\_id](#output\_agw\_subnet\_id) | ID of the Subnet for the App Gateway |
| <a name="output_kv_cert_secret_id"></a> [kv\_cert\_secret\_id](#output\_kv\_cert\_secret\_id) | SSL certificate Secret ID |
| <a name="output_kv_id"></a> [kv\_id](#output\_kv\_id) | ID of the Key Vault |
| <a name="output_kv_mi_id"></a> [kv\_mi\_id](#output\_kv\_mi\_id) | ID of the Managed Identity for the Key Vault |
| <a name="output_vnet_id"></a> [vnet\_id](#output\_vnet\_id) | ID of the Virtual Network |
| <a name="output_vnet_name"></a> [vnet\_name](#output\_vnet\_name) | Name of the Virtual Network |
| <a name="output_webapp_subnet_id"></a> [webapp\_subnet\_id](#output\_webapp\_subnet\_id) | ID of the delegated Subnet for the Web Application |
<!-- END_TF_DOCS -->