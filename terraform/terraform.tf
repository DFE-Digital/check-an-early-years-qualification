# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "= 4.23.0"
    }
  }

  required_version = ">= 1.10.5"

  backend "azurerm" {
    use_oidc = true
  }
}
