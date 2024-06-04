resource "random_string" "resource_code" {
  length  = 5
  special = false
  upper   = false
}

resource "azurerm_storage_account" "sa" {
  name                            = "eyqualwebapp${random_string.resource_code.result}sa"
  resource_group_name             = var.resource_group
  location                        = var.location
  account_tier                    = "Standard"
  min_tls_version                 = "TLS1_2"
  account_replication_type        = "LRS"
  allow_nested_items_to_be_public = false

  queue_properties {
    logging {
      delete                = true
      read                  = true
      write                 = true
      version               = "1.0"
      retention_policy_days = 10
    }
    hour_metrics {
      enabled               = true
      include_apis          = true
      version               = "1.0"
      retention_policy_days = 10
    }
    minute_metrics {
      enabled               = true
      include_apis          = true
      version               = "1.0"
      retention_policy_days = 10
    }
  }

  tags = var.tags

  #checkov:skip=CKV_AZURE_206:GRS not required
  #checkov:skip=CKV_AZURE_59:Argument has been deprecated
  #checkov:skip=CKV2_AZURE_18:Microsoft Managed keys are sufficient
  #checkov:skip=CKV2_AZURE_1:Microsoft Managed keys are sufficient
  #checkov:skip=CKV2_AZURE_38:Soft-delete not required
  #checkov:skip=CKV2_AZURE_33:VNet not configured
}

resource "azurerm_storage_container" "data_protection" {
  name                  = "data-protection"
  storage_account_name  = azurerm_storage_account.sa.name
  container_access_type = "private"

  #checkov:skip=CKV2_AZURE_21:Logging not required
}