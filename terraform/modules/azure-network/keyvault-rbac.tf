# ============================================================
# Key Vault (RBAC, private endpoint only)
# ============================================================
resource "azurerm_key_vault" "rbackv" {
  location            = var.location
  name                = "${var.resource_name_prefix}-kvrbac"
  resource_group_name = var.resource_group
  sku_name            = "standard"
  tenant_id           = data.azurerm_client_config.az_config.tenant_id

  purge_protection_enabled   = true
  rbac_authorization_enabled = true

  public_network_access_enabled   = true
  enabled_for_template_deployment = true

  network_acls {
    default_action = "Deny"
    bypass         = "AzureServices"


    #   lifecycle {
    #   ignore_changes = [network_acls]
    # }

    ip_rules = [
      "${var.github_runner_ip}/32"
    ]
  }


  tags = {
    "Environment"      = var.environment
    "Parent Business"  = "Children's Care"
    "Product"          = "Early Years Qualifications"
    "Service Offering" = "Early Years Qualifications"
  }
}

# ============================================================
# Diagnostic logging
# ============================================================
resource "azurerm_monitor_diagnostic_setting" "rbackv_logs" {
  name                       = "rbackv-to-log-analytics-workspace"
  target_resource_id         = azurerm_key_vault.rbackv.id
  log_analytics_workspace_id = var.log_analytics_workspace_id

  enabled_log {
    category = "AuditEvent"
  }

  enabled_metric {
    category = "AllMetrics"
  }
}

# ============================================================
# Managed identities (declared before role assignments)
# ============================================================
resource "azurerm_user_assigned_identity" "cl-identity-reader" {
  name                = "${var.resource_name_prefix}mid-uks-cl-r"
  location            = var.location
  resource_group_name = var.resource_group
}

resource "azurerm_user_assigned_identity" "cl-identity-administrator" {
  name                = "${var.resource_name_prefix}mid-uks-cl-a"
  location            = var.location
  resource_group_name = var.resource_group
}

resource "azurerm_user_assigned_identity" "kv_mi_rbac" {
  name                = "${var.resource_name_prefix}-kv-mi-rbac"
  location            = var.location
  resource_group_name = var.resource_group
}

resource "azurerm_user_assigned_identity" "agw_mi_rbac" {
  count               = var.environment != "development" ? 1 : 0
  name                = "${var.resource_name_prefix}-agw-mi-rbac"
  location            = var.location
  resource_group_name = var.resource_group
}

# ============================================================
# Role assignments (RBAC on the new vault)
# ============================================================

# Terraform pipeline SP — writes secrets during apply
# resource "azurerm_role_assignment" "sp_kv_secrets_officer" {
#   scope                = azurerm_key_vault.rbackv.id
#   role_definition_name = "Key Vault Secrets Officer"
#   principal_id         = data.azurerm_client_config.az_config.object_id
#   principal_type       = "ServicePrincipal"
# }

# Terraform pipeline SP — writes keys and certificates during apply
resource "azurerm_role_assignment" "kv_admin_sp" {
  scope                = azurerm_key_vault.rbackv.id
  role_definition_name = "Key Vault Administrator"
  principal_id         = data.azurerm_client_config.az_config.object_id
  principal_type       = "ServicePrincipal"
}

# Web app identity — reads secrets
resource "azurerm_role_assignment" "kv_mi_secrets_user" {
  scope                = azurerm_key_vault.rbackv.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_user_assigned_identity.kv_mi_rbac.principal_id
  principal_type       = "ServicePrincipal"
}

# App Gateway identity (non-dev only) — reads secrets for TLS
resource "azurerm_role_assignment" "agw_mi_secrets_user" {
  count                = var.environment != "development" ? 1 : 0
  scope                = azurerm_key_vault.rbackv.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_user_assigned_identity.agw_mi_rbac[0].principal_id
  principal_type       = "ServicePrincipal"
}

# Reader identity — reads secrets
resource "azurerm_role_assignment" "kv_user" {
  scope                = azurerm_key_vault.rbackv.id
  role_definition_name = "Key Vault Secrets User"
  principal_id         = azurerm_user_assigned_identity.cl-identity-reader.principal_id
  principal_type       = "ServicePrincipal"
}

# Administrator identity — reads and writes secrets
resource "azurerm_role_assignment" "kv_officer" {
  scope                = azurerm_key_vault.rbackv.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = azurerm_user_assigned_identity.cl-identity-administrator.principal_id
  principal_type       = "ServicePrincipal"
}

# Administrator identity — full data-plane access
resource "azurerm_role_assignment" "kv_administrator" {
  scope                = azurerm_key_vault.rbackv.id
  role_definition_name = "Key Vault Administrator"
  principal_id         = azurerm_user_assigned_identity.cl-identity-administrator.principal_id
  principal_type       = "ServicePrincipal"
}

# ============================================================
# Wait for RBAC propagation (600s = 10 min)
# ============================================================
resource "time_sleep" "wait_for_rbac" {
  depends_on = [
    #azurerm_role_assignment.sp_kv_secrets_officer,
    azurerm_role_assignment.kv_admin_sp,
    azurerm_role_assignment.kv_mi_secrets_user,
    azurerm_role_assignment.kv_user,
    azurerm_role_assignment.kv_officer,
    azurerm_role_assignment.kv_administrator,
    azurerm_role_assignment.agw_mi_secrets_user,
  ]
  create_duration = "600s"
}

# ============================================================
# Certificate issuer + certificate (non-dev only)
# ============================================================
resource "azurerm_key_vault_certificate_issuer" "rbackv_ca" {
  count = var.environment != "development" ? 1 : 0

  name          = var.kv_certificate_authority_label
  key_vault_id  = azurerm_key_vault.rbackv.id
  provider_name = var.kv_certificate_authority_name
  account_id    = var.kv_certificate_authority_username
  password      = var.kv_certificate_authority_password

  admin {
    email_address = var.kv_certificate_authority_admin_email
    first_name    = var.kv_certificate_authority_admin_first_name
    last_name     = var.kv_certificate_authority_admin_last_name
    phone         = var.kv_certificate_authority_admin_phone_no
  }

  depends_on = [time_sleep.wait_for_rbac]
}

resource "azurerm_key_vault_certificate" "rbackv_service_gov_uk_cert" {
  count = var.environment != "development" ? 1 : 0

  name         = var.kv_service_gov_uk_certificate_label
  key_vault_id = azurerm_key_vault.rbackv.id

  certificate_policy {
    issuer_parameters {
      name = var.kv_certificate_authority_label
    }
    key_properties {
      exportable = true
      key_size   = 2048
      key_type   = "RSA"
      reuse_key  = true
    }
    lifetime_action {
      action {
        action_type = "AutoRenew"
      }
      trigger {
        days_before_expiry = 30
      }
    }
    secret_properties {
      content_type = "application/x-pkcs12"
    }
    x509_certificate_properties {
      extended_key_usage = ["1.3.6.1.5.5.7.3.1", "1.3.6.1.5.5.7.3.2"]
      key_usage          = ["digitalSignature", "keyEncipherment"]
      subject            = var.kv_service_gov_uk_certificate_subject
      validity_in_months = 12
    }
  }

  depends_on = [
    time_sleep.wait_for_rbac,
    azurerm_key_vault_certificate_issuer.rbackv_ca,
  ]
}

# ============================================================
# Data protection key
# ============================================================
resource "azurerm_key_vault_key" "rbackv_data_protection" {
  name         = "data-protection"
  key_vault_id = azurerm_key_vault.rbackv.id
  key_type     = "RSA"
  key_size     = 2048
  key_opts     = ["decrypt", "encrypt", "sign", "unwrapKey", "verify", "wrapKey"]

  depends_on = [time_sleep.wait_for_rbac]
}

# ============================================================
# Secrets
# ============================================================
resource "azurerm_key_vault_secret" "contentful_delivery_api_key_rbac" {
  name         = "ContentfulOptions--DeliveryApiKey"
  value        = var.contentful_delivery_api_key
  key_vault_id = azurerm_key_vault.rbackv.id
  depends_on   = [time_sleep.wait_for_rbac]
}

resource "azurerm_key_vault_secret" "contentful_preview_api_key_rbac" {
  name         = "ContentfulOptions--PreviewApiKey"
  value        = var.contentful_preview_api_key
  key_vault_id = azurerm_key_vault.rbackv.id
  depends_on   = [time_sleep.wait_for_rbac]
}

resource "azurerm_key_vault_secret" "contentful_management_api_key_rbac" {
  name         = "ContentfulOptions--ManagementApiKey"
  value        = var.contentful_management_api_key
  key_vault_id = azurerm_key_vault.rbackv.id
  depends_on   = [time_sleep.wait_for_rbac]
}

resource "azurerm_key_vault_secret" "contentful_space_id_rbac" {
  name         = "ContentfulOptions--SpaceId"
  value        = var.contentful_space_id
  key_vault_id = azurerm_key_vault.rbackv.id
  depends_on   = [time_sleep.wait_for_rbac]
}

resource "azurerm_key_vault_secret" "govuk_notify_api_key_rbac" {
  name         = "Notifications--ApiKey"
  value        = var.govuk_notify_api_key
  key_vault_id = azurerm_key_vault.rbackv.id
  depends_on   = [time_sleep.wait_for_rbac]
}

resource "azurerm_key_vault_secret" "download_endpoint_secret_rbac" {
  name         = "Download--AuthSecret"
  value        = var.download_endpoint_secret
  key_vault_id = azurerm_key_vault.rbackv.id
  depends_on   = [time_sleep.wait_for_rbac]
}