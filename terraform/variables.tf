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

variable "kv_certificate_label" {
  description = "Label for the Certificate"
  type        = string
}

variable "kv_certificate_subject" {
  description = "Subject of the Certificate"
  type        = string
}

variable "as_service_principal_object_id" {
  description = "Object ID of the service principal for App Service"
  type        = string
  sensitive   = true
}

variable "asp_sku" {
  default     = "S1"
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

variable "custom_domain_name" {
  description = "Custom domain hostname"
  type        = string
}

variable "tracking_id" {
  description = "Google Tag Manager tracking ID"
  type        = string
}

variable "webapp_config_bot_token" {
  type      = string
  sensitive = true
}

variable "webapp_config_domain" {
  type = string
}

variable "webapp_config_feedback_url" {
  type = string
}

variable "webapp_config_web_concurrency" {
  type = string
}