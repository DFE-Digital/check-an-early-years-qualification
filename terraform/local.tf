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
}