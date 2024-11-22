# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.65.0"
    }
  }

  required_version = ">= 1.5.0"
}

provider "azurerm" {
  resource_provider_registrations = "none"

  features {}
}

# Create Remote State Storage
resource "azurerm_resource_group" "tfstate" {
  name     = "${var.resource_name_prefix}-tfstate-rg"
  location = var.default_azure_region

  tags = merge(local.common_tags, {
    "Region" = var.default_azure_region
  })
}

resource "random_string" "resource_code" {
  length  = 5
  special = false
  upper   = false
}

resource "azurerm_storage_account" "tfstate" {
  name                            = "eyqualtfstate${random_string.resource_code.result}st"
  resource_group_name             = azurerm_resource_group.tfstate.name
  location                        = var.default_azure_region
  account_tier                    = "Standard"
  account_kind                    = "StorageV2"
  min_tls_version                 = "TLS1_2"
  account_replication_type        = "LRS"
  allow_nested_items_to_be_public = false

  tags = merge(local.common_tags, {
    "Region" = var.default_azure_region
  })

  #checkov:skip=CKV_AZURE_206:GRS not required
  #checkov:skip=CKV_AZURE_59:Argument has been deprecated
  #checkov:skip=CKV2_AZURE_18:Microsoft Managed keys are sufficient
  #checkov:skip=CKV2_AZURE_1:Microsoft Managed keys are sufficient
  #checkov:skip=CKV2_AZURE_38:Soft-delete not required
  #checkov:skip=CKV2_AZURE_33:VNet not configured
}

resource "azurerm_storage_account_queue_properties" "tfstateq" {
  storage_account_id = azurerm_storage_account.tfstate.id

  logging {
    version               = "1.0"
    delete                = true
    read                  = true
    write                 = true
    retention_policy_days = 10
  }

  hour_metrics {
    version               = "1.0"
    include_apis          = true
    retention_policy_days = 10
  }

  minute_metrics {
    version               = "1.0"
    include_apis          = true
    retention_policy_days = 10
  }
}

resource "azurerm_storage_container" "tfstate" {
  name                  = "${var.resource_name_prefix}-tfstate-stc"
  storage_account_id    = azurerm_storage_account.tfstate.id
  container_access_type = "private"

  #checkov:skip=CKV2_AZURE_21:Logging not required
}
