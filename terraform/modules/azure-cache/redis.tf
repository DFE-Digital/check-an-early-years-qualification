# Create Azure Cache for Redis
resource "azurerm_resource_provider_registration" "cache_provider" {
  count = var.environment != "development" ? 1 : 0
  name  = "Microsoft.Cache"
}

resource "azurerm_redis_cache" "redis" {
  name                               = "${var.resource_name_prefix}-redis-cache"
  location                           = var.location
  resource_group_name                = var.resource_group
  capacity                           = 0
  family                             = "C"
  sku_name                           = var.environment == "development" ? "Basic" : "Standard"
  access_keys_authentication_enabled = false
  non_ssl_port_enabled               = false
  minimum_tls_version                = "1.2"
  public_network_access_enabled      = false

  redis_configuration {
    authentication_enabled                  = true
    active_directory_authentication_enabled = true
  }

  tags = var.tags

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

# Create diagnostic settings for Redis
resource "azurerm_monitor_diagnostic_setting" "redis_monitor" {
  name                       = "${var.resource_name_prefix}-cache-mon"
  target_resource_id         = azurerm_redis_cache.redis.id
  log_analytics_workspace_id = var.logs_id

  enabled_log {
    category = "ConnectedClientList"
  }

  metric {
    category = "AllMetrics"
  }
}
