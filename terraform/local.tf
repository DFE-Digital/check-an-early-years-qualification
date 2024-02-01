locals {
  # Common tags to be assigned to resources
  common_tags = {
    "Environment"      = var.environment
    "Parent Business"  = "Children’s Care"
    "Product"          = "Early Years Qualification"
    "Service Offering" = "Early Years Qualification"
  }

  # Web Application Configuration
  webapp_app_settings = {
    "ENVIRONMENT"                         = var.environment
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "BOT_TOKEN"                           = var.webapp_config_bot_token
    "DOMAIN"                              = var.webapp_config_domain
    "FEEDBACK_URL"                        = var.webapp_config_feedback_url
    "TRACKING_ID"                         = var.tracking_id
    "WEBSITES_CONTAINER_START_TIME_LIMIT" = 720
  }

  webapp_slot_app_settings = {
    "ENVIRONMENT"                         = var.environment
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "BOT_TOKEN"                           = var.webapp_config_bot_token
    "DOMAIN"                              = var.webapp_config_domain
    "FEEDBACK_URL"                        = var.webapp_config_feedback_url
    "TRACKING_ID"                         = var.tracking_id
    "WEBSITES_CONTAINER_START_TIME_LIMIT" = 720
  }
}