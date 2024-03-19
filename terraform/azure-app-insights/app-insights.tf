resource "azurerm_log_analytics_workspace" "main" {
  name                = "${var.resource_name_prefix}-workspace"
  resource_group_name = var.resource_group
  location            = var.location
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_application_insights" "main" {
  name                = "${var.resource_name_prefix}-appinsights"
  resource_group_name = var.resource_group
  location            = var.location
  application_type    = "web"
  workspace_id        = azurerm_log_analytics_workspace.main.id
}