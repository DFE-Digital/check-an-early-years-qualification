resource "azurerm_redis_cache" "cache" {
  name                               = "${var.resource_name_prefix}-redis-cache"
  location                           = var.location
  resource_group_name                = var.resource_group
  capacity                           = var.environment != "development" ? 0 : 1
  family                             = "C"
  sku_name                           = var.environment != "development" ? "Standard" : "Basic"
  non_ssl_port_enabled               = false
  minimum_tls_version                = "1.2"
  public_network_access_enabled      = false
  access_keys_authentication_enabled = false


  redis_configuration {
    active_directory_authentication_enabled = true
    maxmemory_reserved                      = 30
    maxfragmentationmemory_reserved         = 30
    maxmemory_delta                         = 30
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

resource "azurerm_private_endpoint" "cache_endpoint" {
  name                = "${var.resource_name_prefix}-redis-nic"
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.webapp_subnet_id

  private_service_connection {
    name                           = "${var.resource_name_prefix}-redis-con"
    private_connection_resource_id = azurerm_redis_cache.cache.id
    is_manual_connection           = false
  }
}

output "redis_cache_id" {
  value = azurerm_redis_cache.cache.id
}
