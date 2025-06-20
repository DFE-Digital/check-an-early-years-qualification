variable "azure_region" {
  default     = "westeurope"
  description = "Name of the Azure region to deploy resources"
  type        = string
}

variable "environment" {
  default     = "development"
  description = "Environment to deploy resources"
  type        = string
}

variable "resource_name_prefix" {
  description = "Prefix for resource names"
  type        = string
}

variable "admin_email_address" {
  description = "Email Address of the Admin"
  type        = string
  sensitive   = true
}

variable "kv_certificate_authority_username" {
  description = "Username for the Certificate provider"
  type        = string
  sensitive   = true
}

variable "kv_certificate_authority_password" {
  description = "Password the Certificate provider"
  type        = string
  sensitive   = true
}

variable "kv_certificate_authority_admin_first_name" {
  description = "First Name of the Certificate Authority Admin"
  type        = string
  sensitive   = true
}

variable "kv_certificate_authority_admin_last_name" {
  description = "Last Name of the Certificate Authority Admin"
  type        = string
  sensitive   = true
}

variable "kv_certificate_authority_admin_phone_no" {
  description = "Phone No. of the Certificate Authority Admin"
  type        = string
  sensitive   = true
}

variable "kv_service_gov_uk_certificate_label" {
  description = "Label for the service.gov.uk certificate"
  type        = string
}

variable "kv_service_gov_uk_certificate_subject" {
  description = "Subject of the service.gov.uk certificate"
  type        = string
}

variable "asp_sku" {
  description = "SKU name for the App Service Plan"
  type        = string
}

variable "webapp_worker_count" {
  default     = 1
  description = "Number of Workers for the App Service Plan"
  type        = string
}

variable "webapp_name" {
  description = "Name for the Web Application"
  type        = string
}

variable "webapp_slot_name" {
  default     = "green"
  description = "Name for the slot for the Web Application"
  type        = string
}

variable "webapp_storage_account_name" {
  description = "Storage Account name"
  type        = string
}

variable "webapp_access_is_public" {
  description = "Web app service is public, and access is unchallenged"
  default     = false
  type        = bool
}

variable "webapp_e2e_access_key" {
  description = "Web app access key for automated end-to-end tests"
  type        = string
}

variable "webapp_team_access_key" {
  description = "Web app access key for the service team"
  type        = string
}

variable "webapp_access_key_1" {
  description = "Web app access key for invited access 1"
  type        = string
}

variable "webapp_access_key_2" {
  description = "Web app access key for invited access 2"
  type        = string
}

variable "webapp_docker_registry_url" {
  description = "URL to the Docker Registry"
  type        = string
}

variable "webapp_docker_image" {
  description = "Docker Image to deploy"
  type        = string
}

variable "webapp_docker_image_tag" {
  default     = "latest"
  description = "Tag for the Docker Image"
  type        = string
}

variable "service_gov_uk_custom_domain_name" {
  description = "Custom domain hostname for the service.gov.uk domain"
  type        = string
}

variable "contentful_delivery_api_key" {
  description = "Contentful delivery API key"
  type        = string
}

variable "contentful_preview_api_key" {
  description = "Contentful preview API key"
  type        = string
}

variable "contentful_space_id" {
  description = "Contentful space ID"
  type        = string
}

variable "contentful_use_preview_api" {
  description = "Boolean used to set whether content is preview or published"
  type        = bool
}

variable "gtm_tag" {
  default     = ""
  description = "The Google Analytics tag"
  type        = string
}

variable "clarity_tag" {
  default     = ""
  description = "The Microsoft Clarity tag"
  type        = string
}

variable "govuk_notify_api_key" {
  description = "GovUK Notify API Key"
  type        = string
}

variable "cache_endpoint_secret" {
  description = "Secret value to be supplied when calling cache endpoint"
  type        = string
}

variable "notifications_feedback_template_id" {
  description = "GovUK Notify Feedback Email Template Id"
  type        = string
}

variable "notifications_feedback_email_address" {
  description = "GovUK Notify Feedback Email Address"
  type        = string
}

variable "notifications_is_test_environment" {
  description = "Flag to indicate if the notification comes from a test environment"
  type        = bool
}

variable "cache_type" {
  description = "Cache type (\"Redis\", \"Memory\", or \"None\")"
  type        = string
  nullable    = false
  default     = "None"
  validation {
    condition     = contains(["Redis", "Memory", "None"], var.cache_type)
    error_message = "Invalid cache type. Must be one of: Redis, Memory, None."
  }
}
