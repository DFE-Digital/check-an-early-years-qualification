output "instrumentation_key" {
  description = "The instrumentation key for App Insights"
  value       = azurerm_application_insights.main.instrumentation_key
}

output "connection_string" {
  description = "The connection string for App Insights"
  value       = azurerm_application_insights.main.connection_string
}