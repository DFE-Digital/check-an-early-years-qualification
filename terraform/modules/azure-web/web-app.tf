# Create Log Analytics
resource "azurerm_log_analytics_workspace" "webapp_logs" {
  name                = "${var.resource_name_prefix}-log"
  location            = var.location
  resource_group_name = var.resource_group
  sku                 = "PerGB2018"
  retention_in_days   = 30
  daily_quota_gb      = 1

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

resource "azurerm_application_insights" "web" {
  name                = "${var.resource_name_prefix}-appinsights"
  resource_group_name = var.resource_group
  location            = var.location
  application_type    = "web"
  workspace_id        = azurerm_log_analytics_workspace.webapp_logs.id
  tags                = var.tags

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Create App Service Plan
resource "azurerm_service_plan" "asp" {
  name                = "${var.resource_name_prefix}-asp"
  location            = var.location
  resource_group_name = var.resource_group
  os_type             = "Linux"
  sku_name            = var.asp_sku
  worker_count        = var.webapp_worker_count

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }

  #checkov:skip=CKV_AZURE_212:Argument not available
  #checkov:skip=CKV_AZURE_225:Ensure the App Service Plan is zone redundant
}

# Create Web Application
resource "azurerm_linux_web_app" "webapp" {
  name                      = var.webapp_name
  location                  = var.location
  resource_group_name       = var.resource_group
  service_plan_id           = azurerm_service_plan.asp.id
  https_only                = true
  virtual_network_subnet_id = var.webapp_subnet_id
  app_settings = merge({
    "APPINSIGHTS_INSTRUMENTATIONKEY"             = azurerm_application_insights.web.instrumentation_key
    "APPLICATIONINSIGHTS_CONNECTION_STRING"      = azurerm_application_insights.web.connection_string
    "ApplicationInsightsAgent_EXTENSION_VERSION" = "~3"
  }, var.webapp_app_settings)

  identity {
    type = "SystemAssigned"
  }

  site_config {
    app_command_line                  = var.webapp_startup_command
    health_check_path                 = var.webapp_health_check_path
    health_check_eviction_time_in_min = var.webapp_health_check_eviction_time_in_min
    http2_enabled                     = true
    vnet_route_all_enabled            = true

    application_stack {
      docker_image_name   = "${var.webapp_docker_image}:${var.webapp_docker_image_tag}"
      docker_registry_url = var.webapp_docker_registry_url
    }

    dynamic "ip_restriction" {
      # Deploy App Gateway rules only to the Test and Production subscription
      for_each = var.environment != "development" ? [1] : []
      content {
        name                      = "Allow app gateway"
        action                    = "Allow"
        priority                  = 300
        virtual_network_subnet_id = var.agw_subnet_id
      }
    }

    dynamic "ip_restriction" {
      # Deploy App Gateway rules only to the Test and Production subscription
      for_each = var.environment != "development" ? [1] : []
      content {
        name       = "Deny public"
        action     = "Deny"
        priority   = 500
        ip_address = "0.0.0.0/0"
      }
    }
  }

  sticky_settings {
    app_setting_names = keys(var.webapp_app_settings)
  }

  logs {
    detailed_error_messages = true
    failed_request_tracing  = true

    application_logs {
      file_system_level = "Warning"
    }

    http_logs {
      file_system {
        retention_in_days = 1
        retention_in_mb   = 25
      }
    }
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"],
      site_config.0.application_stack
    ]
  }

  tags = var.tags

  #checkov:skip=CKV_AZURE_13:App uses built-in authentication
  #checkov:skip=CKV_AZURE_88:Using Docker
  #checkov:skip=CKV_AZURE_17:Argument not available
  #checkov:skip=CKV_AZURE_78:Disabled by default in Terraform version used
  #checkov:skip=CKV_AZURE_16:Using VNET Integration
  #checkov:skip=CKV_AZURE_71:Using VNET Integration
  #checkov:skip=CKV_AZURE_222:Network access rules configured
}

# Create Web Application Deployment Slot
resource "azurerm_linux_web_app_slot" "webapp_slot" {

  count = var.environment != "development" ? 1 : 0

  name                      = var.webapp_slot_name
  app_service_id            = azurerm_linux_web_app.webapp.id
  https_only                = true
  virtual_network_subnet_id = var.webapp_subnet_id
  app_settings = merge({
    "APPINSIGHTS_INSTRUMENTATIONKEY"             = azurerm_application_insights.web.instrumentation_key
    "APPLICATIONINSIGHTS_CONNECTION_STRING"      = azurerm_application_insights.web.connection_string
    "ApplicationInsightsAgent_EXTENSION_VERSION" = "~3"
  }, var.webapp_slot_app_settings)

  site_config {
    app_command_line                  = var.webapp_startup_command
    health_check_path                 = var.webapp_health_check_path
    health_check_eviction_time_in_min = var.webapp_health_check_eviction_time_in_min
    http2_enabled                     = true
    vnet_route_all_enabled            = true

    application_stack {
      docker_image_name   = "${var.webapp_docker_image}:${var.webapp_docker_image_tag}"
      docker_registry_url = var.webapp_docker_registry_url
    }
  }

  logs {
    detailed_error_messages = true
    failed_request_tracing  = true

    application_logs {
      file_system_level = "Warning"
    }

    http_logs {
      file_system {
        retention_in_days = 1
        retention_in_mb   = 25
      }
    }
  }

  identity {
    type = "SystemAssigned"
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"],
      site_config.0.application_stack
    ]
  }
}

resource "azurerm_monitor_diagnostic_setting" "webapp_logs_monitor" {

  name                       = "${var.resource_name_prefix}-webapp-mon"
  target_resource_id         = azurerm_linux_web_app.webapp.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.webapp_logs.id

  enabled_log {
    category = "AppServiceConsoleLogs"
  }

  enabled_log {
    category = "AppServicePlatformLogs"
  }

  timeouts {
    read = "30m"
  }

  lifecycle {
    ignore_changes = [metric]
  }
}

resource "azurerm_monitor_diagnostic_setting" "webapp_slot_logs_monitor" {

  count = var.environment != "development" ? 1 : 0

  name                       = "${var.resource_name_prefix}-webapp-${var.webapp_slot_name}-mon"
  target_resource_id         = azurerm_linux_web_app_slot.webapp_slot.0.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.webapp_logs.id

  enabled_log {
    category = "AppServiceConsoleLogs"
  }

  enabled_log {
    category = "AppServicePlatformLogs"
  }

  timeouts {
    read = "30m"
  }

  lifecycle {
    ignore_changes = [metric]
  }
}

# Configure Web App Autoscaling
resource "azurerm_monitor_autoscale_setting" "asp_as" {
  # Autoscaling rules only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name                = "${var.resource_name_prefix}-asp-as"
  location            = var.location
  resource_group_name = var.resource_group
  target_resource_id  = azurerm_service_plan.asp.id

  profile {
    name = "Autoscaling conditions"

    capacity {
      default = 2
      minimum = 2
      maximum = 5
    }

    rule {
      metric_trigger {
        metric_name        = "CpuPercentage"
        metric_namespace   = "microsoft.web/serverfarms"
        metric_resource_id = azurerm_service_plan.asp.id
        statistic          = "Average"
        operator           = "GreaterThan"
        threshold          = 75
        time_aggregation   = "Average"
        time_grain         = "PT1M"
        time_window        = "PT10M"
      }

      scale_action {
        direction = "Increase"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT5M"
      }
    }

    rule {
      metric_trigger {
        metric_name        = "CpuPercentage"
        metric_namespace   = "microsoft.web/serverfarms"
        metric_resource_id = azurerm_service_plan.asp.id
        statistic          = "Average"
        operator           = "LessThan"
        threshold          = 20
        time_aggregation   = "Average"
        time_grain         = "PT1M"
        time_window        = "PT10M"
      }

      scale_action {
        direction = "Decrease"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT5M"
      }
    }

    rule {
      metric_trigger {
        metric_name        = "MemoryPercentage"
        metric_namespace   = "microsoft.web/serverfarms"
        metric_resource_id = azurerm_service_plan.asp.id
        statistic          = "Average"
        operator           = "GreaterThan"
        threshold          = 80
        time_aggregation   = "Average"
        time_grain         = "PT1M"
        time_window        = "PT10M"
      }

      scale_action {
        direction = "Increase"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT5M"
      }
    }

    rule {
      metric_trigger {
        metric_name        = "MemoryPercentage"
        metric_namespace   = "microsoft.web/serverfarms"
        metric_resource_id = azurerm_service_plan.asp.id
        statistic          = "Average"
        operator           = "LessThan"
        threshold          = 65
        time_aggregation   = "Average"
        time_grain         = "PT1M"
        time_window        = "PT10M"
      }

      scale_action {
        direction = "Decrease"
        type      = "ChangeCount"
        value     = "1"
        cooldown  = "PT5M"
      }
    }
  }

  notification {
    email {
      custom_emails = [var.webapp_admin_email_address]
    }
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Create Custom Domain Name
resource "azurerm_app_service_custom_hostname_binding" "webapp_custom_domain" {
  # Custom hostname only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  resource_group_name = var.resource_group
  hostname            = var.webapp_custom_domain_name
  app_service_name    = azurerm_linux_web_app.webapp.name

  lifecycle {
    ignore_changes = [ssl_state, thumbprint]
  }
}

resource "azurerm_app_service_custom_hostname_binding" "webapp_service_gov_uk_custom_domain" {
  # Custom hostname only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  resource_group_name = var.resource_group
  hostname            = var.webapp_service_gov_uk_custom_domain_name
  app_service_name    = azurerm_linux_web_app.webapp.name

  lifecycle {
    ignore_changes = [ssl_state, thumbprint]
  }
}

data "azurerm_client_config" "az_config" {}

# References the web app to be used in KV access policy as it already existed when changes needed to be made
data "azurerm_linux_web_app" "ref" {
  name                = azurerm_linux_web_app.webapp.name
  resource_group_name = azurerm_linux_web_app.webapp.resource_group_name
}

# Grants permissions to key vault for the managed identity of the App Service
resource "azurerm_key_vault_access_policy" "webapp_kv_app_service" {
  key_vault_id            = var.kv_id
  tenant_id               = data.azurerm_client_config.az_config.tenant_id
  object_id               = data.azurerm_linux_web_app.ref.identity.0.principal_id
  key_permissions         = ["Get", "UnwrapKey", "WrapKey"]
  secret_permissions      = ["Get", "List"]
  certificate_permissions = ["Get"]

  lifecycle {
    ignore_changes = [object_id, tenant_id]
  }
}

# Grants permissions to key vault for the managed identity of the App Service slot
resource "azurerm_key_vault_access_policy" "webapp_kv_app_service_slot" {

  count = var.environment != "development" ? 1 : 0

  key_vault_id            = var.kv_id
  tenant_id               = data.azurerm_client_config.az_config.tenant_id
  object_id               = azurerm_linux_web_app_slot.webapp_slot.0.identity.0.principal_id
  key_permissions         = ["Get", "UnwrapKey", "WrapKey"]
  secret_permissions      = ["Get", "List"]
  certificate_permissions = ["Get"]

  lifecycle {
    ignore_changes = [object_id, tenant_id]
  }
}

resource "azurerm_app_service_certificate" "webapp_custom_domain_cert" {
  # Custom hostname only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name                = var.webapp_custom_domain_cert_secret_label
  resource_group_name = var.resource_group
  location            = var.location
  key_vault_secret_id = var.kv_cert_secret_id
}

resource "azurerm_app_service_certificate_binding" "webapp_custom_domain_cert_bind" {
  # Custom hostname only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  hostname_binding_id = azurerm_app_service_custom_hostname_binding.webapp_custom_domain[0].id
  certificate_id      = azurerm_app_service_certificate.webapp_custom_domain_cert[0].id
  ssl_state           = "SniEnabled"
}

resource "azurerm_app_service_certificate" "webapp_service_gov_uk_custom_domain_cert" {
  # Custom hostname only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name                = var.webapp_service_gov_uk_custom_domain_cert_secret_label
  resource_group_name = var.resource_group
  location            = var.location
  key_vault_secret_id = var.kv_service_gov_uk_cert_secret_id
}

resource "azurerm_app_service_certificate_binding" "webapp_service_gov_uk_custom_domain_cert_bind" {
  # Custom hostname only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  hostname_binding_id = azurerm_app_service_custom_hostname_binding.webapp_service_gov_uk_custom_domain[0].id
  certificate_id      = azurerm_app_service_certificate.webapp_service_gov_uk_custom_domain_cert[0].id
  ssl_state           = "SniEnabled"
}


resource "azure_redis_cache_access_policy_assignment" "web_app_contrib" {
  name               = "web-app-redis-contributor"
  redis_cache_id     = var.azurerm_redis_cache_id
  access_policy_name = "Data Contributor"
  object_id          = azurerm_linux_web_app.webapp.identity.object_id
  object_id_alias    = "ServicePrincipal"
}
