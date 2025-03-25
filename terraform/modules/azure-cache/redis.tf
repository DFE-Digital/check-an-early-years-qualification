resource "azurerm_private_dns_zone" "redis_dns" {
  name                = "redis.cache.windows.net"
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
