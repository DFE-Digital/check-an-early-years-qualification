locals {
  # Common tags to be assigned to resources
  common_tags = {
    "Environment"      = var.environment
    "Parent Business"  = "Children's Care"
    "Product"          = "Early Years Qualifications"
    "Service Offering" = "Early Years Qualifications"
  }

  # Tags used for private DNS zone resources [to work around a bug in Azure]
  dns_zone_link_tags = {
    "Environment" = var.environment
    "Product"     = "Early Years Qualifications"
    # ...the bug is that Azure does not add tags with names containing a space to private DNS zones vnet link
  }

  # Web Application Configuration
  webapp_app_settings = {
    "ENVIRONMENT"                           = var.environment
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE"   = "false"
    "WEBSITES_CONTAINER_START_TIME_LIMIT"   = 720
    "KeyVault__Endpoint"                    = "https://${var.resource_name_prefix}-kv.vault.azure.net/"
    "ContentfulOptions__UsePreviewApi"      = var.contentful_use_preview_api
    "WEBSITES_PORT"                         = "8080"
    "ServiceAccess__IsPublic"               = var.webapp_access_is_public
    "ServiceAccess__Keys__0"                = var.webapp_e2e_access_key
    "ServiceAccess__Keys__1"                = var.webapp_team_access_key
    "ServiceAccess__Keys__2"                = var.webapp_access_key_1
    "ServiceAccess__Keys__3"                = var.webapp_access_key_2
    "GTM__Tag"                              = var.gtm_tag
    "Clarity__Tag"                          = var.clarity_tag
    "Notifications__Feedback__TemplateId"   = var.notifications_feedback_template_id
    "Notifications__Feedback__EmailAddress" = var.notifications_feedback_email_address
    "Notifications__IsTestEnvironment"      = var.notifications_is_test_environment
    "Cache__Type"                           = var.cache_type
  }

  webapp_slot_app_settings = {
    "ENVIRONMENT"                         = var.environment
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "WEBSITES_CONTAINER_START_TIME_LIMIT" = 720
    "KeyVault__Endpoint"                  = "https://${var.resource_name_prefix}-kv.vault.azure.net/"
    "ContentfulOptions__UsePreviewApi"    = var.contentful_use_preview_api
    "WEBSITES_PORT"                       = "8080"
    "ServiceAccess__IsPublic"             = var.webapp_access_is_public
    "ServiceAccess__Keys__0"              = var.webapp_e2e_access_key
    "ServiceAccess__Keys__1"              = var.webapp_team_access_key
    "ServiceAccess__Keys__2"              = var.webapp_access_key_1
    "ServiceAccess__Keys__3"              = var.webapp_access_key_2
    "Cache__Type"                         = var.cache_type
  }
}
