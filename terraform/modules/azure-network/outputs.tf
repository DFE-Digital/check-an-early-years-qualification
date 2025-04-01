output "vnet_id" {
  description = "ID of the Virtual Network"
  value       = azurerm_virtual_network.vnet.id
}

output "vnet_name" {
  description = "Name of the Virtual Network"
  value       = azurerm_virtual_network.vnet.name
}

output "webapp_subnet_id" {
  description = "ID of the delegated Subnet for the Web Application"
  value       = azurerm_subnet.webapp_snet.id
}

output "agw_subnet_id" {
  description = "ID of the Subnet for the App Gateway"
  value       = var.environment != "development" ? azurerm_subnet.agw_snet[0].id : null
}

output "cache_subnet_id" {
  description = "ID of the Subnet for the Redis cache"
  value       = azurerm_subnet.cache_snet.id
}

output "agw_pip_id" {
  description = "ID of the Public IP address for the App Gateway"
  value       = var.environment != "development" ? azurerm_public_ip.agw_pip[0].id : null
}

output "kv_id" {
  description = "ID of the Key Vault"
  value       = azurerm_key_vault.kv.id
}

output "kv_cert_secret_id" {
  description = "education.gov.uk SSL certificate Secret ID"
  value       = var.environment != "development" ? azurerm_key_vault_certificate.kv_cert[0].secret_id : null
}

output "kv_service_gov_uk_cert_secret_id" {
  description = "service.gov.uk SSL certificate Secret ID"
  value       = var.environment != "development" ? azurerm_key_vault_certificate.kv_service_gov_uk_cert[0].secret_id : null
}

output "kv_mi_id" {
  description = "ID of the Managed Identity for the Key Vault"
  value       = var.environment != "development" ? azurerm_user_assigned_identity.kv_mi[0].id : null
}
