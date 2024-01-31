locals {
  # Common tags to be assigned to all resources
  common_tags = {
    "Environment"      = var.default_environment
    "Parent Business"  = "Children’s Care"
    "Portfolio"        = ""
    "Product"          = "EY Qualification"
    "Service"          = ""
    "Service Line"     = ""
    "Service Offering" = "EY Qualification"
  }
}