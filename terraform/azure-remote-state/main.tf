# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "= 4.57.0"
    }
  }

  required_version = ">= 1.10.5"
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

  tags = merge(local.common_tags, {
    "Region" = var.default_azure_region
  })
}

resource "azurerm_storage_container" "tfstate" {
  name                  = "${var.resource_name_prefix}-tfstate-stc"
  storage_account_name  = azurerm_storage_account.tfstate.name
  container_access_type = "private"
}
