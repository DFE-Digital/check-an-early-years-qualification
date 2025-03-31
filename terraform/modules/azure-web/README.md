# Azure Web Module

This module provisions a new Azure App Service & Application Gateway to host a Docker container for a Web Application.

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
| [azurerm_app_service_certificate.webapp_custom_domain_cert](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_service_certificate) | resource |
| [azurerm_app_service_certificate.webapp_service_gov_uk_custom_domain_cert](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_service_certificate) | resource |
| [azurerm_app_service_certificate_binding.webapp_custom_domain_cert_bind](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_service_certificate_binding) | resource |
| [azurerm_app_service_certificate_binding.webapp_service_gov_uk_custom_domain_cert_bind](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_service_certificate_binding) | resource |
| [azurerm_app_service_custom_hostname_binding.webapp_custom_domain](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_service_custom_hostname_binding) | resource |
| [azurerm_app_service_custom_hostname_binding.webapp_service_gov_uk_custom_domain](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_service_custom_hostname_binding) | resource |
| [azurerm_application_gateway.agw](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/application_gateway) | resource |
| [azurerm_key_vault_access_policy.webapp_kv_app_service](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_access_policy) | resource |
| [azurerm_key_vault_access_policy.webapp_kv_app_service_slot](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/key_vault_access_policy) | resource |
| [azurerm_linux_web_app.webapp](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app) | resource |
| [azurerm_linux_web_app_slot.webapp_slot](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/linux_web_app_slot) | resource |
| [azurerm_monitor_autoscale_setting.asp_as](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_autoscale_setting) | resource |
| [azurerm_monitor_diagnostic_setting.agw_logs_monitor](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_monitor_diagnostic_setting.webapp_logs_monitor](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_monitor_diagnostic_setting.webapp_slot_logs_monitor](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/monitor_diagnostic_setting) | resource |
| [azurerm_service_plan.asp](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/service_plan) | resource |
| [azurerm_web_application_firewall_policy.agw_wafp](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/web_application_firewall_policy) | resource |
| [azurerm_client_config.az_config](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/data-sources/client_config) | data source |
| [azurerm_linux_web_app.ref](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/data-sources/linux_web_app) | data source |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_agw_pip_id"></a> [agw\_pip\_id](#input\_agw\_pip\_id) | ID of the Public IP address for the App Gateway | `string` | n/a | yes |
| <a name="input_agw_subnet_id"></a> [agw\_subnet\_id](#input\_agw\_subnet\_id) | ID of the Subnet for the App Gateway | `string` | n/a | yes |
| <a name="input_asp_sku"></a> [asp\_sku](#input\_asp\_sku) | SKU name for the App Service Plan | `string` | n/a | yes |
| <a name="input_environment"></a> [environment](#input\_environment) | Environment to deploy resources | `string` | n/a | yes |
| <a name="input_insights_connection_string"></a> [insights\_connection\_string](#input\_insights\_connection\_string) | App Insights connection string | `string` | n/a | yes |
| <a name="input_instrumentation_key"></a> [instrumentation\_key](#input\_instrumentation\_key) | App Insights instrumentation key | `string` | n/a | yes |
| <a name="input_kv_cert_secret_id"></a> [kv\_cert\_secret\_id](#input\_kv\_cert\_secret\_id) | education.gov.uk SSL certificate Secret ID | `string` | n/a | yes |
| <a name="input_kv_id"></a> [kv\_id](#input\_kv\_id) | ID of the Key Vault | `string` | n/a | yes |
| <a name="input_kv_mi_id"></a> [kv\_mi\_id](#input\_kv\_mi\_id) | ID of the Managed Identity for the Key Vault | `string` | n/a | yes |
| <a name="input_kv_service_gov_uk_cert_secret_id"></a> [kv\_service\_gov\_uk\_cert\_secret\_id](#input\_kv\_service\_gov\_uk\_cert\_secret\_id) | service.gov.uk SSL certificate Secret ID | `string` | n/a | yes |
| <a name="input_location"></a> [location](#input\_location) | Name of the Azure region to deploy resources | `string` | n/a | yes |
| <a name="input_logs_id"></a> [logs\_id](#input\_logs\_id) | Log Analytics workspace ID | `string` | n/a | yes |
| <a name="input_resource_group"></a> [resource\_group](#input\_resource\_group) | Name of the Azure Resource Group to deploy resources | `string` | n/a | yes |
| <a name="input_resource_name_prefix"></a> [resource\_name\_prefix](#input\_resource\_name\_prefix) | Prefix for resource names | `string` | n/a | yes |
| <a name="input_tags"></a> [tags](#input\_tags) | Resource tags | `map(string)` | n/a | yes |
| <a name="input_webapp_admin_email_address"></a> [webapp\_admin\_email\_address](#input\_webapp\_admin\_email\_address) | Email Address of the Admin | `string` | n/a | yes |
| <a name="input_webapp_app_settings"></a> [webapp\_app\_settings](#input\_webapp\_app\_settings) | App Settings are exposed as environment variables | `map(string)` | n/a | yes |
| <a name="input_webapp_cookie_auth_secret_name"></a> [webapp\_cookie\_auth\_secret\_name](#input\_webapp\_cookie\_auth\_secret\_name) | Name of the cookie holding the auth secret | `string` | n/a | yes |
| <a name="input_webapp_cookie_preference_name"></a> [webapp\_cookie\_preference\_name](#input\_webapp\_cookie\_preference\_name) | Name of the user's cookie preference cookie | `string` | n/a | yes |
| <a name="input_webapp_cookie_user_journey_name"></a> [webapp\_cookie\_user\_journey\_name](#input\_webapp\_cookie\_user\_journey\_name) | Name of the cookie holding the user's filter selections | `string` | n/a | yes |
| <a name="input_webapp_custom_domain_cert_secret_label"></a> [webapp\_custom\_domain\_cert\_secret\_label](#input\_webapp\_custom\_domain\_cert\_secret\_label) | Label for the education.gov.uk certificate | `string` | n/a | yes |
| <a name="input_webapp_custom_domain_name"></a> [webapp\_custom\_domain\_name](#input\_webapp\_custom\_domain\_name) | education.gov.uk custom domain hostname | `string` | n/a | yes |
| <a name="input_webapp_docker_image"></a> [webapp\_docker\_image](#input\_webapp\_docker\_image) | Docker Image to deploy | `string` | n/a | yes |
| <a name="input_webapp_docker_image_tag"></a> [webapp\_docker\_image\_tag](#input\_webapp\_docker\_image\_tag) | Tag for the Docker Image | `string` | n/a | yes |
| <a name="input_webapp_docker_registry_url"></a> [webapp\_docker\_registry\_url](#input\_webapp\_docker\_registry\_url) | URL to the Docker Registry | `string` | n/a | yes |
| <a name="input_webapp_health_check_eviction_time_in_min"></a> [webapp\_health\_check\_eviction\_time\_in\_min](#input\_webapp\_health\_check\_eviction\_time\_in\_min) | Minutes before considering an instance unhealthy | `number` | `null` | no |
| <a name="input_webapp_health_check_path"></a> [webapp\_health\_check\_path](#input\_webapp\_health\_check\_path) | Path to health check endpoint | `string` | `null` | no |
| <a name="input_webapp_name"></a> [webapp\_name](#input\_webapp\_name) | Name for the Web Application | `string` | n/a | yes |
| <a name="input_webapp_service_gov_uk_custom_domain_cert_secret_label"></a> [webapp\_service\_gov\_uk\_custom\_domain\_cert\_secret\_label](#input\_webapp\_service\_gov\_uk\_custom\_domain\_cert\_secret\_label) | Label for the service.gov.uk certificate | `string` | n/a | yes |
| <a name="input_webapp_service_gov_uk_custom_domain_name"></a> [webapp\_service\_gov\_uk\_custom\_domain\_name](#input\_webapp\_service\_gov\_uk\_custom\_domain\_name) | service.gov.uk custom domain hostname | `string` | n/a | yes |
| <a name="input_webapp_session_cookie_name"></a> [webapp\_session\_cookie\_name](#input\_webapp\_session\_cookie\_name) | Name of the user session Cookie | `string` | n/a | yes |
| <a name="input_webapp_slot_app_settings"></a> [webapp\_slot\_app\_settings](#input\_webapp\_slot\_app\_settings) | App Settings are exposed as environment variables | `map(string)` | n/a | yes |
| <a name="input_webapp_slot_name"></a> [webapp\_slot\_name](#input\_webapp\_slot\_name) | Name for the slot for the Web Application | `string` | n/a | yes |
| <a name="input_webapp_startup_command"></a> [webapp\_startup\_command](#input\_webapp\_startup\_command) | Startup command to pass into the Web Application | `string` | `null` | no |
| <a name="input_webapp_subnet_id"></a> [webapp\_subnet\_id](#input\_webapp\_subnet\_id) | ID of the delegated Subnet for the Web Application | `string` | n/a | yes |
| <a name="input_webapp_worker_count"></a> [webapp\_worker\_count](#input\_webapp\_worker\_count) | Number of Workers for the App Service Plan | `string` | n/a | yes |

## Outputs

| Name | Description |
|------|-------------|
| <a name="output_app_service_plan_id"></a> [app\_service\_plan\_id](#output\_app\_service\_plan\_id) | ID of the App Service Plan for the Web Application |
| <a name="output_app_service_webapp_id"></a> [app\_service\_webapp\_id](#output\_app\_service\_webapp\_id) | ID of the Web Application |
<!-- END_TF_DOCS -->