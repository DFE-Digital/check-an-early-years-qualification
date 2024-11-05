# Supported Metrics can be found here: https://learn.microsoft.com/en-us/azure/azure-monitor/reference/metrics-index

# Create the Dev Team action group - Manual step to add / update users in the group
resource "azurerm_monitor_action_group" "dev_team" {
  name                = "dev-team-action-group"
  resource_group_name = var.resource_group
  short_name          = "Dev Team"
  tags                = var.tags
  
  # Should ignore changes made to email receivers
  lifecycle {
      ignore_changes = [
          email_receiver
      ]
  }
}

# Alert for CPU > 90%
resource "azurerm_monitor_metric_alert" "cpu_alert" {
  name                = "cpu-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_plan_id]
  description         = "Action will be triggered when CPU Percentage is greater than 90%"
  tags                = var.tags

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "CpuPercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 90
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }
}

# Alert for Memory > 90%
resource "azurerm_monitor_metric_alert" "memory_alert" {
  name                = "memory-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_plan_id]
  description         = "Action will be triggered when Memory Percentage is greater than 90%"
  tags                = var.tags

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "MemoryPercentage"
    aggregation      = "Average"
    operator         = "GreaterThan"
    threshold        = 90
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }
}