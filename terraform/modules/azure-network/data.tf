data "azurerm_client_config" "current" {}

data "azurerm_key_vault_certificate" "service_gov_uk_cert" {
  name         = "service-gov-uk"
  key_vault_id = azurerm_key_vault.rbackv.id

}