locals {
  # Common tags to be assigned to all resources
  common_tags = {
    "Environment"      = var.default_environment
    "Parent Business"  = "Children's Care"
    "Product"          = "Early Years Qualifications"
    "Service Offering" = "Early Years Qualifications"
  }
}