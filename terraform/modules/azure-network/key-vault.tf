# Create Key Vault
data "azurerm_client_config" "az_config" {}

resource "azurerm_key_vault" "kv" {
  name                        = "${var.resource_name_prefix}-kv"
  resource_group_name         = var.resource_group
  location                    = var.location
  tenant_id                   = data.azurerm_client_config.az_config.tenant_id
  enabled_for_disk_encryption = true
  soft_delete_retention_days  = 7
  purge_protection_enabled    = true
  sku_name                    = "standard"

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }

  #checkov:skip=CKV_AZURE_109:Access Policies configured
  #checkov:skip=CKV_AZURE_189:Access Policies configured
  #checkov:skip=CKV2_AZURE_32:VNET configuration adequate
}

resource "azurerm_user_assigned_identity" "kv_mi" {
  # Identity for App Gateway, only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name                = "${var.resource_name_prefix}-agw-mi"
  location            = var.location
  resource_group_name = var.resource_group
}

# Access Policy for GitHub Actions
resource "azurerm_key_vault_access_policy" "kv_gh_ap" {
  key_vault_id = azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.az_config.tenant_id
  object_id    = data.azurerm_client_config.az_config.object_id

  key_permissions = [
    "Create",
    "Delete",
    "Get",
    "UnwrapKey",
    "WrapKey",
    "Recover",
    "Purge",
    "Update",
    "GetRotationPolicy",
    "SetRotationPolicy"
  ]

  secret_permissions = ["List", "Get", "Set", "Recover"]

  certificate_permissions = [
    "Create",
    "Get",
    "GetIssuers",
    "Import",
    "List",
    "ListIssuers",
    "ManageContacts",
    "ManageIssuers",
    "SetIssuers",
    "Update"
  ]

  lifecycle {
    ignore_changes = [object_id]
  }
}

resource "azurerm_key_vault_access_policy" "kv_mi_ap" {
  # Key Vault only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  key_vault_id = azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.az_config.tenant_id
  object_id    = azurerm_user_assigned_identity.kv_mi[0].principal_id

  secret_permissions = [
    "Get"
  ]

  certificate_permissions = [
    "Get"
  ]
}

resource "azurerm_key_vault_certificate_issuer" "kv_ca" {
  # Certificate issuer only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name          = var.kv_certificate_authority_label
  key_vault_id  = azurerm_key_vault.kv.id
  provider_name = var.kv_certificate_authority_name
  account_id    = var.kv_certificate_authority_username
  password      = var.kv_certificate_authority_password

  admin {
    email_address = var.kv_certificate_authority_admin_email
    first_name    = var.kv_certificate_authority_admin_first_name
    last_name     = var.kv_certificate_authority_admin_last_name
    phone         = var.kv_certificate_authority_admin_phone_no
  }
}

resource "azurerm_key_vault_certificate" "kv_cert" {
  # Certificate only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name         = var.kv_certificate_label
  key_vault_id = azurerm_key_vault.kv.id

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
      subject            = var.kv_certificate_subject
      validity_in_months = 12
    }
  }
}

resource "azurerm_key_vault_certificate" "kv_service_gov_uk_cert" {
  # Certificate only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name         = var.kv_service_gov_uk_certificate_label
  key_vault_id = azurerm_key_vault.kv.id

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
}

resource "azurerm_key_vault_key" "data-protection" {
  name         = "data-protection"
  key_vault_id = azurerm_key_vault.kv.id
  key_type     = "RSA"
  key_size     = 2048

  key_opts = [
    "decrypt",
    "encrypt",
    "sign",
    "unwrapKey",
    "verify",
    "wrapKey",
  ]
}

resource "azurerm_key_vault_secret" "contentful_delivery_api_key" {
  name         = "ContentfulOptions--DeliveryApiKey"
  value        = var.contentful_delivery_api_key
  key_vault_id = azurerm_key_vault.kv.id
}

resource "azurerm_key_vault_secret" "contentful_preview_api_key" {
  name         = "ContentfulOptions--PreviewApiKey"
  value        = var.contentful_preview_api_key
  key_vault_id = azurerm_key_vault.kv.id
}

resource "azurerm_key_vault_secret" "contentful_space_id" {
  name         = "ContentfulOptions--SpaceId"
  value        = var.contentful_space_id
  key_vault_id = azurerm_key_vault.kv.id
}

resource "azurerm_key_vault_secret" "govuk_notify_api_key" {
  name         = "Notifications--ApiKey"
  value        = var.govuk_notify_api_key
  key_vault_id = azurerm_key_vault.kv.id
}