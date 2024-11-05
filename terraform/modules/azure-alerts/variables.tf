variable "resource_group" {
  description = "Name of the Azure Resource Group to deploy resources"
  type        = string
}

variable "app_service_plan_id" {
  description = "Id of the App Service Plan"
  type        = string
}

variable "tags" {
  description = "Resource tags"
  type        = map(string)
}