resource "azurerm_redis_cache" "cache" {
  name                               = "${var.resource_name_prefix}-redis-cache"
  location                           = var.location
  resource_group_name                = var.resource_group
  capacity                           = var.environment != "development" ? 0 : 1
  family                             = "C"
  sku_name                           = var.environment != "development" ? "Standard" : "Basic"
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

resource "azurerm_private_endpoint" "cache_endpoint" {
  name                = "${var.resource_name_prefix}-redis-nic"
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.cache_subnet_id

  private_service_connection {
    name                           = "${var.resource_name_prefix}-redis-con"
    private_connection_resource_id = azurerm_redis_cache.cache.id
    is_manual_connection           = false
    subresource_names = [
      "redisCache"
    ]
  }

  private_dns_zone_group {
    name = "redis-dns"

    private_dns_zone_ids = [
      azurerm_private_dns_zone.dns_zone.id
    ]
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

resource "azurerm_private_dns_zone" "dns_zone" {
  name                = "${azurerm_redis_cache.cache.name}.redis.cache.windows.net"
  resource_group_name = var.resource_group
}



resource "azurerm_private_dns_zone_virtual_network_link" "redis_snet" {
  name                  = "${var.resource_name_prefix}-redis-vnet-link"
  resource_group_name   = var.resource_group
  private_dns_zone_name = azurerm_private_dns_zone.dns_zone.name
  virtual_network_id    = var.cache_subnet_id
}
