resource "azurerm_storage_account" "sa" {
  name                             = var.webapp_storage_account_name
  resource_group_name              = var.resource_group
  location                         = var.location
  account_tier                     = "Standard"
  min_tls_version                  = "TLS1_2"
  account_replication_type         = "LRS"
  allow_nested_items_to_be_public  = false
  cross_tenant_replication_enabled = false
  shared_access_key_enabled        = true

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

  blob_properties {
    delete_retention_policy {
      days = 7
    }
    container_delete_retention_policy {
      days = 7
    }
  }

  tags = var.tags

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }

  #checkov:skip=CKV_AZURE_206:GRS not required
  #checkov:skip=CKV_AZURE_59:Argument has been deprecated
  #checkov:skip=CKV2_AZURE_18:Microsoft Managed keys are sufficient
  #checkov:skip=CKV2_AZURE_1:Microsoft Managed keys are sufficient
  #checkov:skip=CKV2_AZURE_38:Soft-delete not required
  #checkov:skip=CKV2_AZURE_33:VNet not configured
}

resource "azurerm_storage_account_network_rules" "sa_network_rules" {
  storage_account_id         = azurerm_storage_account.sa.id
  default_action             = "Deny"
  bypass                     = ["AzureServices"]
  ip_rules                   = []
  virtual_network_subnet_ids = [var.webapp_subnet_id]
}

resource "azurerm_storage_container" "data_protection" {
  name                  = "data-protection"
  storage_account_id    = azurerm_storage_account.sa.id
  container_access_type = "private"

  lifecycle {
    ignore_changes = [storage_account_name]
    # ...reports storage_account_name as deprecated, but this ignore_changes block is necessary here.
    # Without it, the switch from storage_account_name to storage_account_id causes destruction and recreation.
  }

  #checkov:skip=CKV2_AZURE_21:Logging not required
}

resource "azurerm_key_vault_secret" "storage_connection_string" {
  name         = "Storage--ConnectionString"
  value        = azurerm_storage_account.sa.primary_connection_string
  key_vault_id = var.kv_id
}
