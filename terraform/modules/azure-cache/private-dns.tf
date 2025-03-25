resource "azurerm_private_dns_zone" "redis_pdz" {
  name                = "privatelink.redis.cache.windows.net"
  resource_group_name = var.resource_group

  tags = var.tags

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

resource "azurerm_private_endpoint" "redis_endpoint" {
  name                = "${var.resource_name_prefix}-redis-pe"
  location            = var.location
  resource_group_name = var.resource_group
  subnet_id           = var.cache_subnet_id

  private_service_connection {
    name                           = "pe-${azurerm_redis_cache.redis.name}"
    private_connection_resource_id = azurerm_redis_cache.redis.id
    is_manual_connection           = false
    subresource_names              = ["redisCache"]
  }

  private_dns_zone_group {
    name                 = "default"
    private_dns_zone_ids = [azurerm_private_dns_zone.redis_pdz.id]
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
