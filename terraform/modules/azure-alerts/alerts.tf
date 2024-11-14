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
      email_receiver,
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Alert for CPU >= 90%
resource "azurerm_monitor_metric_alert" "cpu_alert" {
  name                = "cpu-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_plan_id]
  description         = "Action will be triggered when CPU Percentage is greater than 90%"
  tags                = var.tags
  severity            = 2 # warning

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "CpuPercentage"
    aggregation      = "Average"
    operator         = "GreaterThanOrEqual"
    threshold        = 90
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Alert for Memory >= 90%
resource "azurerm_monitor_metric_alert" "memory_alert" {
  name                = "memory-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_plan_id]
  description         = "Action will be triggered when Memory Percentage is greater than 90%"
  tags                = var.tags
  severity            = 2 # warning

  criteria {
    metric_namespace = "Microsoft.Web/serverfarms"
    metric_name      = "MemoryPercentage"
    aggregation      = "Average"
    operator         = "GreaterThanOrEqual"
    threshold        = 90
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Alert for Http4xx errors Avg >= 10
resource "azurerm_monitor_metric_alert" "http4xx_errors" {
  name                = "http4xx-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_webapp_id]
  description         = "Action will be triggered when Http4xx errors occur"
  tags                = var.tags
  severity            = 2 # warning

  criteria {
    metric_namespace = "Microsoft.Web/sites"
    metric_name      = "Http4xx"
    aggregation      = "Average"
    operator         = "GreaterThanOrEqual"
    threshold        = 10
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Alert for Http5xx errors Avg >= 10
resource "azurerm_monitor_metric_alert" "http5xx_errors" {
  name                = "http5xx-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_webapp_id]
  description         = "Action will be triggered when Http5xx errors occur"
  tags                = var.tags
  severity            = 1 # error

  criteria {
    metric_namespace = "Microsoft.Web/sites"
    metric_name      = "Http5xx"
    aggregation      = "Average"
    operator         = "GreaterThanOrEqual"
    threshold        = 10
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Alert for InstanceCount increase > 2
resource "azurerm_monitor_metric_alert" "instance_count_increase" {
  name                = "instance-count-increase-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_plan_id]
  description         = "Action will be triggered when the instance count increases"
  tags                = var.tags
  severity            = 3 # informational

  criteria {
    metric_namespace = "microsoft.insights/autoscalesettings"
    metric_name      = "ScaleActionsInitiated"
    aggregation      = "Total"
    operator         = "GreaterThan"
    threshold        = 2
  }

  dimension {
    name     = "ScaleDirection"
    operator = "Equals"
    values   = ["Increase"]
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Alert for InstanceCount decrease <= 2
resource "azurerm_monitor_metric_alert" "instance_count_decrease" {
  name                = "instance-count-decrease-alert"
  resource_group_name = var.resource_group
  scopes              = [var.app_service_webapp_id]
  description         = "Action will be triggered when the instance count decreases"
  tags                = var.tags
  severity            = 3 # informational

  criteria {
    metric_namespace = "microsoft.insights/autoscalesettings"
    metric_name      = "ScaleActionsInitiated"
    aggregation      = "Total"
    operator         = "LessThanOrEqual"
    threshold        = 2
  }

  dimension {
    name     = "ScaleDirection"
    operator = "Equals"
    values   = ["Decrease"]
  }

  action {
    action_group_id = azurerm_monitor_action_group.dev_team.id
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}