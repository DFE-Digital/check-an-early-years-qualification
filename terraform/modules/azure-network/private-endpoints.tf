resource "azurerm_private_endpoint" "kv-private-endpoint" {
  name                = "${var.resource_name_prefix}-kv-pe"
  resource_group_name = var.resource_group
  location            = var.location
  subnet_id           = azurerm_subnet.private-endpoint-subnet.id

  private_service_connection {
    name                           = "${var.resource_name_prefix}-kv-psc"
    is_manual_connection           = false
    private_connection_resource_id = azurerm_key_vault.rbackv.id
    subresource_names              = ["vault"]
  }
  private_dns_zone_group {
    name                 = "${var.resource_name_prefix}-rbackv-dns-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.rbackv-dns-zone.id]
  }

  lifecycle {
    ignore_changes = [
      tags
    ]
  }
}

resource "azurerm_private_dns_zone" "rbackv-dns-zone" {
  name = "privatelink.vaultcore.azure.net"
  #resource_group_name = azurerm_resource_group.web-rg.name
  resource_group_name = var.resource_group

  lifecycle {
    ignore_changes = [
      tags
    ]
  }

  depends_on = [
    azurerm_key_vault.rbackv,
    azurerm_subnet.webapp_snet,
    azurerm_subnet.private-endpoint-subnet
  ]
}

#azurerm_subnet
resource "azurerm_private_dns_zone_virtual_network_link" "rbackv-dns-link" {
  name                  = "${var.resource_group}-rbackv-dns-link"
  resource_group_name   = var.resource_group
  private_dns_zone_name = azurerm_private_dns_zone.rbackv-dns-zone.name
  virtual_network_id    = azurerm_virtual_network.vnet.id

  lifecycle {
    ignore_changes = [
      tags
    ]
  }

  depends_on = [
    azurerm_subnet.webapp_snet,
    azurerm_subnet.private-endpoint-subnet
  ]
}