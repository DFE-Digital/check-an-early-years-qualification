variable "environment" {
  description = "Environment to deploy resources"
  type        = string
}

variable "location" {
  description = "Name of the Azure region to deploy resources"
  type        = string
}

variable "resource_group" {
  description = "Name of the Azure Resource Group to deploy resources"
  type        = string
}

variable "resource_name_prefix" {
  description = "Prefix for resource names"
  type        = string
}

variable "vnet_id" {
  description = "ID of the virtual network"
  type        = string
}

variable "vnet_name" {
  description = "Name of the virtual network"
  type        = string
}

variable "cache_subnet_id" {
  description = "ID of the delegated Subnet for the Redis cache"
  type        = string
}

variable "logs_id" {
  description = "The ID of the Log Analytics workspace"
  type        = string
}

variable "tags" {
  description = "Resource tags"
  type        = map(string)
}
