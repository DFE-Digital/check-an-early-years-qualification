variable "resource_group" {
  description = "Name of the Azure Resource Group to deploy resources"
  type        = string
}

variable "app_service_plan_id" {
  description = "Id of the App Service Plan"
  type        = string
}

variable "app_service_webapp_id" {
  description = "Id of the Web Application"
  type        = string
}

variable "log_analytics_workspace_id" {
  description = "The log analytics workspace ID"
  type        = string
}

variable "tags" {
  description = "Resource tags"
  type        = map(string)
}
