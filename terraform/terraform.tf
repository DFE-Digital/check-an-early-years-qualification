# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "= 4.57.0"
    }
    azapi = {
      source = "Azure/azapi"
    }
  }

  required_version = ">= 1.10.5"

  backend "azurerm" {
    use_oidc = true
  }
}
