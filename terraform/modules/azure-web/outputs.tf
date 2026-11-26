output "app_service_plan_id" {
  description = "ID of the App Service Plan for the Web Application"
  value       = azurerm_service_plan.asp.id
}

output "app_service_webapp_id" {
  description = "ID of the Web Application"
  value       = azurerm_linux_web_app.webapp.id
}

output "webapp_principal_id" {
  value = azurerm_linux_web_app.webapp.identity[0].principal_id
}