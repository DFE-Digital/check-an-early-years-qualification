locals {
  # Common tags to be assigned to resources
  common_tags = {
    "Environment"      = var.environment
    "Parent Business"  = "Children’s Care"
    "Portfolio"        = ""
    "Product"          = "EY Qualification"
    "Service"          = ""
    "Service Line"     = ""
    "Service Offering" = "EY Qualification"
  }

  # Web Application Configuration
  webapp_app_settings = {
    "ENVIRONMENT"                         = var.environment
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "BOT_TOKEN"                           = var.webapp_config_bot_token
    "USER_PASSWORD"                       = var.webapp_config_user_password
    "CONTENTFUL_ENVIRONMENT"              = var.webapp_config_contentful_environment
    "CONTENTFUL_PREVIEW"                  = var.webapp_config_contentful_preview
    "DOMAIN"                              = var.webapp_config_domain
    "EDITOR"                              = var.webapp_config_editor
    "FEEDBACK_URL"                        = var.webapp_config_feedback_url
    "GROVER_NO_SANDBOX"                   = var.webapp_config_grover_no_sandbox
    "HOTJAR_SITE_ID"                      = var.hotjar_site_id
    "NODE_ENV"                            = var.webapp_config_node_env
    "TRACKING_ID"                         = var.tracking_id
    "WEB_CONCURRENCY"                     = var.webapp_config_web_concurrency
    "WEBSITES_CONTAINER_START_TIME_LIMIT" = 720
  }

  webapp_slot_app_settings = {
    "ENVIRONMENT"                         = var.environment
    "WEBSITES_ENABLE_APP_SERVICE_STORAGE" = "false"
    "BOT_TOKEN"                           = var.webapp_config_bot_token
    "USER_PASSWORD"                       = var.webapp_config_user_password
    "CONTENTFUL_ENVIRONMENT"              = var.webapp_config_contentful_environment
    "CONTENTFUL_PREVIEW"                  = var.webapp_config_contentful_preview
    "DOMAIN"                              = var.webapp_config_domain
    "EDITOR"                              = var.webapp_config_editor
    "FEEDBACK_URL"                        = var.webapp_config_feedback_url
    "GROVER_NO_SANDBOX"                   = var.webapp_config_grover_no_sandbox
    "HOTJAR_SITE_ID"                      = var.hotjar_site_id
    "NODE_ENV"                            = var.webapp_config_node_env
    "TRACKING_ID"                         = var.tracking_id
    "WEB_CONCURRENCY"                     = var.webapp_config_web_concurrency
    "WEBSITES_CONTAINER_START_TIME_LIMIT" = 720
  }
}