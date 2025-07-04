provider "azurerm" {
  resource_provider_registrations = "none"

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

moved {
  from = module.webapp.azurerm_key_vault_access_policy.webapp_kv_app_service_slot
  to   = module.webapp.azurerm_key_vault_access_policy.webapp_kv_app_service_slot[0]
}

moved {
  from = module.webapp.azurerm_monitor_diagnostic_setting.webapp_slot_logs_monitor
  to   = module.webapp.azurerm_monitor_diagnostic_setting.webapp_slot_logs_monitor[0]
}

moved {
  from = module.webapp.azurerm_monitor_diagnostic_setting.webapp_slot_logs_monitor
  to   = module.webapp.azurerm_monitor_diagnostic_setting.webapp_slot_logs_monitor[0]
}

moved {
  from = module.webapp.azurerm_log_analytics_workspace.webapp_logs
  to   = module.monitor.azurerm_log_analytics_workspace.log_analytics
}

moved {
  from = module.webapp.azurerm_application_insights.web
  to   = module.monitor.azurerm_application_insights.app_insights
}

moved {
  from = module.cache.azurerm_resource_provider_registration.cache_provider
  to   = module.cache.azurerm_resource_provider_registration.cache_provider[0]
}

# Create Resource Group
resource "azurerm_resource_group" "rg" {
  name     = "${var.resource_name_prefix}-rg"
  location = var.azure_region

  tags = local.common_tags

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

module "monitor" {
  source = "./modules/azure-monitoring"

  environment          = var.environment
  location             = var.azure_region
  resource_group       = azurerm_resource_group.rg.name
  resource_name_prefix = var.resource_name_prefix
  tags                 = local.common_tags
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
  kv_service_gov_uk_certificate_label       = var.kv_service_gov_uk_certificate_label
  kv_service_gov_uk_certificate_subject     = var.kv_service_gov_uk_certificate_subject
  contentful_delivery_api_key               = var.contentful_delivery_api_key
  contentful_preview_api_key                = var.contentful_preview_api_key
  contentful_space_id                       = var.contentful_space_id
  govuk_notify_api_key                      = var.govuk_notify_api_key
}

# Create storage account for web app
module "storage" {
  source = "./modules/azure-storage"

  location                    = var.azure_region
  resource_group              = azurerm_resource_group.rg.name
  webapp_storage_account_name = var.webapp_storage_account_name
  kv_id                       = module.network.kv_id
  webapp_subnet_id            = module.network.webapp_subnet_id
  tags                        = local.common_tags
}

# Create Redis cache
module "cache" {
  source = "./modules/azure-cache"

  environment          = var.environment
  location             = var.azure_region
  resource_group       = azurerm_resource_group.rg.name
  resource_name_prefix = var.resource_name_prefix
  vnet_id              = module.network.vnet_id
  vnet_name            = module.network.vnet_name
  cache_subnet_id      = module.network.cache_subnet_id
  logs_id              = module.monitor.logs_id
  tags                 = local.common_tags
  dns_zone_link_tags   = local.dns_zone_link_tags
}

# Create web application resources
module "webapp" {
  source = "./modules/azure-web"

  environment                                           = var.environment
  location                                              = var.azure_region
  resource_group                                        = azurerm_resource_group.rg.name
  resource_name_prefix                                  = var.resource_name_prefix
  asp_sku                                               = var.asp_sku
  webapp_admin_email_address                            = var.admin_email_address
  webapp_worker_count                                   = var.webapp_worker_count
  webapp_subnet_id                                      = module.network.webapp_subnet_id
  webapp_name                                           = var.webapp_name
  webapp_slot_name                                      = var.webapp_slot_name
  webapp_app_settings                                   = local.webapp_app_settings
  webapp_slot_app_settings                              = local.webapp_slot_app_settings
  webapp_docker_image                                   = var.webapp_docker_image
  webapp_docker_image_tag                               = var.webapp_docker_image_tag
  webapp_docker_registry_url                            = var.webapp_docker_registry_url
  webapp_session_cookie_name                            = "_early_years_qualification_session"
  webapp_cookie_preference_name                         = "cookies_preferences_set"
  webapp_cookie_auth_secret_name                        = "auth-secret"
  webapp_cookie_user_journey_name                       = "user_journey"
  webapp_service_gov_uk_custom_domain_name              = var.service_gov_uk_custom_domain_name
  webapp_service_gov_uk_custom_domain_cert_secret_label = var.kv_service_gov_uk_certificate_label
  webapp_health_check_path                              = "/health"
  webapp_health_check_eviction_time_in_min              = 10
  logs_id                                               = module.monitor.logs_id
  instrumentation_key                                   = module.monitor.insights_instrumentation_key
  insights_connection_string                            = module.monitor.insights_connection_string
  agw_subnet_id                                         = module.network.agw_subnet_id
  agw_pip_id                                            = module.network.agw_pip_id
  kv_id                                                 = module.network.kv_id
  kv_service_gov_uk_cert_secret_id                      = module.network.kv_service_gov_uk_cert_secret_id
  kv_mi_id                                              = module.network.kv_mi_id
  redis_cache_id                                        = module.cache.redis_cache_id
  redis_cache_name                                      = module.cache.redis_cache_name
  cache_endpoint_secret                                 = var.cache_endpoint_secret
  tags                                                  = local.common_tags
  depends_on                                            = [module.network]
}

# Create service alerts
module "alerts" {
  source = "./modules/azure-alerts"

  resource_group        = azurerm_resource_group.rg.name
  app_service_plan_id   = module.webapp.app_service_plan_id
  app_service_webapp_id = module.webapp.app_service_webapp_id
  tags                  = local.common_tags
  depends_on            = [module.webapp]
}
