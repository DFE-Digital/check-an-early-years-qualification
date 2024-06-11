provider "azurerm" {
  skip_provider_registration = "true"

  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }

    key_vault {
      purge_soft_delete_on_destroy    = true
      recover_soft_deleted_key_vaults = true
    }
  }
}

# Create Resource Group
resource "azurerm_resource_group" "rg" {
  name     = "${var.resource_name_prefix}-rg"
  location = var.azure_region

  tags = local.common_tags

  lifecycle {
    ignore_changes = [tags]
  }
}

# Create network resources
module "network" {
  source = "./modules/azure-network"

  environment                               = var.environment
  location                                  = var.azure_region
  resource_group                            = azurerm_resource_group.rg.name
  resource_name_prefix                      = var.resource_name_prefix
  kv_certificate_authority_label            = "GlobalSignCA"
  kv_certificate_authority_name             = "GlobalSign"
  kv_certificate_authority_username         = var.kv_certificate_authority_username
  kv_certificate_authority_password         = var.kv_certificate_authority_password
  kv_certificate_authority_admin_email      = var.admin_email_address
  kv_certificate_authority_admin_first_name = var.kv_certificate_authority_admin_first_name
  kv_certificate_authority_admin_last_name  = var.kv_certificate_authority_admin_last_name
  kv_certificate_authority_admin_phone_no   = var.kv_certificate_authority_admin_phone_no
  kv_certificate_label                      = var.kv_certificate_label
  kv_certificate_subject                    = var.kv_certificate_subject
  contentful_delivery_api_key               = var.contentful_delivery_api_key
  contentful_preview_api_key                = var.contentful_preview_api_key
  contentful_space_id                       = var.contentful_space_id
}

# Create storage account for web app
module "storage" {
  source = "./modules/azure-storage"

  location       = var.azure_region
  resource_group = azurerm_resource_group.rg.name
  tags           = local.common_tags
}

# Create web application resources
module "webapp" {
  source = "./modules/azure-web"

  environment                              = var.environment
  location                                 = var.azure_region
  resource_group                           = azurerm_resource_group.rg.name
  resource_name_prefix                     = var.resource_name_prefix
  as_service_principal_object_id           = var.as_service_principal_object_id
  asp_sku                                  = var.asp_sku
  webapp_admin_email_address               = var.admin_email_address
  webapp_worker_count                      = var.webapp_worker_count
  webapp_subnet_id                         = module.network.webapp_subnet_id
  webapp_name                              = var.webapp_name
  webapp_app_settings                      = local.webapp_app_settings
  webapp_slot_app_settings                 = local.webapp_slot_app_settings
  webapp_docker_image                      = var.webapp_docker_image
  webapp_docker_image_tag                  = var.webapp_docker_image_tag
  webapp_docker_registry_url               = var.webapp_docker_registry_url
  webapp_session_cookie_name               = "_early_years_qualification_session"
  webapp_cookie_preference_name            = "cookies_preferences_set"
  webapp_custom_domain_name                = var.custom_domain_name
  webapp_custom_domain_cert_secret_label   = var.kv_certificate_label
  webapp_health_check_path                 = "/health"
  webapp_health_check_eviction_time_in_min = 10
  agw_subnet_id                            = module.network.agw_subnet_id
  agw_pip_id                               = module.network.agw_pip_id
  kv_id                                    = module.network.kv_id
  kv_cert_secret_id                        = module.network.kv_cert_secret_id
  kv_mi_id                                 = module.network.kv_mi_id
  tags                                     = local.common_tags
  depends_on                               = [module.network]
}