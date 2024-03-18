resource "azurerm_application_insights" "main" {
  name                = "${var.resource_name_prefix}-appinsights"
  resource_group_name = var.resource_group
  location            = var.location
  application_type    = "web"
}