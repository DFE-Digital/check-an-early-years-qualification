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
| [azurerm_application_insights.app_insights](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/application_insights) | resource |
| [azurerm_log_analytics_workspace.log_analytics](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/log_analytics_workspace) | resource |

## Inputs

| Name | Description | Type | Default | Required |
|------|-------------|------|---------|:--------:|
| <a name="input_environment"></a> [environment](#input\_environment) | Environment to deploy resources | `string` | n/a | yes |
| <a name="input_location"></a> [location](#input\_location) | Name of the Azure region to deploy resources | `string` | n/a | yes |
| <a name="input_resource_group"></a> [resource\_group](#input\_resource\_group) | Name of the Azure Resource Group to deploy resources | `string` | n/a | yes |
| <a name="input_resource_name_prefix"></a> [resource\_name\_prefix](#input\_resource\_name\_prefix) | Prefix for resource names | `string` | n/a | yes |
| <a name="input_tags"></a> [tags](#input\_tags) | Resource tags | `map(string)` | n/a | yes |

## Outputs

| Name | Description |
|------|-------------|
| <a name="output_insights_connection_string"></a> [insights\_connection\_string](#output\_insights\_connection\_string) | Application Insights connection string |
| <a name="output_insights_instrumentation_key"></a> [insights\_instrumentation\_key](#output\_insights\_instrumentation\_key) | Application Insights instrumentation key |
| <a name="output_logs_id"></a> [logs\_id](#output\_logs\_id) | ID of the Log Analytics workspace |
<!-- END_TF_DOCS -->