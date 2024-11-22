# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.11.0"
    }
  }

  required_version = "~> 1.9.0"

  backend "azurerm" {
    use_oidc = true
  }
}
