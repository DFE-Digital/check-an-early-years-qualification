# Create App Gateway
resource "azurerm_web_application_firewall_policy" "agw_wafp" {
  # App Gateway only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name                = "${var.resource_name_prefix}-agw-wafp"
  location            = var.location
  resource_group_name = var.resource_group

  managed_rules {
    managed_rule_set {
      type    = "OWASP"
      version = "3.2"

      rule_group_override {
        rule_group_name = "REQUEST-920-PROTOCOL-ENFORCEMENT"
        rule {
          id      = "920420"
          enabled = false
        }
      }
    }

    managed_rule_set {
      type    = "Microsoft_BotManagerRuleSet"
      version = "0.1"
    }

    exclusion {
      match_variable          = "RequestCookieNames"
      selector                = var.webapp_session_cookie_name
      selector_match_operator = "Equals"
    }

    exclusion {
      match_variable          = "RequestCookieNames"
      selector                = var.webapp_cookie_preference_name
      selector_match_operator = "Equals"
    }

    exclusion {
      match_variable          = "RequestCookieNames"
      selector                = var.webapp_cookie_auth_secret_name
      selector_match_operator = "Equals"
    }

    exclusion {
      match_variable          = "RequestCookieNames"
      selector                = var.webapp_cookie_user_journey_name
      selector_match_operator = "Equals"
    }

    /* The two exclusions below allow the anti-forgery to bypass the SQLi 942440 rule */
    exclusion {
      match_variable          = "RequestCookieNames"
      selector                = ".AspNetCore.Antiforgery"
      selector_match_operator = "Equals"
    }

    exclusion {
      match_variable          = "RequestArgNames"
      selector                = "__RequestVerificationToken"
      selector_match_operator = "StartsWith"
    }

    /* The exclusion below stops the cookie banner hide functionality from triggering the 949110 - REQUEST-949-BLOCKING-EVALUATION rule.
     As it contains the word select in the request arg, it thinks it's a SQLi attack. */
    exclusion {
      match_variable          = "RequestArgValues"
      selector                = "/select-a-qualification-to-check"
      selector_match_operator = "Contains"
    }
  }

  policy_settings {
    enabled                                   = true
    file_upload_limit_in_mb                   = 100
    max_request_body_size_in_kb               = 128
    mode                                      = "Prevention"
    request_body_check                        = true
    js_challenge_cookie_expiration_in_minutes = 30
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }
}

locals {
  backend_address_pool_name      = "${var.resource_name_prefix}-agw-beap"
  frontend_port_name             = "${var.resource_name_prefix}-agw-feport"
  frontend_ip_configuration_name = "${var.resource_name_prefix}-agw-feip"
  http_setting_name              = "${var.resource_name_prefix}-agw-best"
  health_probe_name              = "${var.resource_name_prefix}-agw-hp"
  listener_name                  = "${var.resource_name_prefix}-agw-lstn"
  ssl_certificate_name           = "${var.resource_name_prefix}-agw-cert"
}

resource "azurerm_application_gateway" "agw" {
  # App Gateway only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name                              = "${var.resource_name_prefix}-agw"
  location                          = var.location
  resource_group_name               = var.resource_group
  enable_http2                      = true
  firewall_policy_id                = azurerm_web_application_firewall_policy.agw_wafp[0].id
  force_firewall_policy_association = true

  sku {
    name = "WAF_v2"
    tier = "WAF_v2"
  }

  autoscale_configuration {
    min_capacity = 2
    max_capacity = 10
  }

  gateway_ip_configuration {
    name      = "${var.resource_name_prefix}-agw-ipc"
    subnet_id = var.agw_subnet_id
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [var.kv_mi_id]
  }

  ssl_certificate {
    name                = local.ssl_certificate_name
    key_vault_secret_id = var.kv_service_gov_uk_cert_secret_id
  }

  ssl_policy {
    policy_type = "Predefined"
    policy_name = "AppGwSslPolicy20220101"
  }

  probe {
    host                = var.webapp_service_gov_uk_custom_domain_name
    name                = local.health_probe_name
    interval            = 30
    path                = "/health"
    protocol            = "Https"
    timeout             = 30
    unhealthy_threshold = 3

    match {
      status_code = [200]
      body        = null
    }
  }

  frontend_port {
    name = local.frontend_port_name
    port = 443
  }

  frontend_ip_configuration {
    name                 = local.frontend_ip_configuration_name
    public_ip_address_id = var.agw_pip_id
  }

  backend_address_pool {
    name  = local.backend_address_pool_name
    fqdns = [azurerm_linux_web_app.webapp.default_hostname]
  }

  backend_http_settings {
    name                  = local.http_setting_name
    cookie_based_affinity = "Disabled"
    port                  = 443
    protocol              = "Https"
    probe_name            = local.health_probe_name
    request_timeout       = 300

    connection_draining {
      enabled           = true
      drain_timeout_sec = 60
    }
  }

  http_listener {
    name                           = local.listener_name
    frontend_ip_configuration_name = local.frontend_ip_configuration_name
    frontend_port_name             = local.frontend_port_name
    firewall_policy_id             = azurerm_web_application_firewall_policy.agw_wafp[0].id
    protocol                       = "Https"
    ssl_certificate_name           = local.ssl_certificate_name
  }

  request_routing_rule {
    name                       = "${var.resource_name_prefix}-agw-rr"
    rule_type                  = "Basic"
    priority                   = 100
    http_listener_name         = local.listener_name
    backend_address_pool_name  = local.backend_address_pool_name
    backend_http_settings_name = local.http_setting_name
  }

  lifecycle {
    ignore_changes = [
      tags["Environment"],
      tags["Product"],
      tags["Service Offering"]
    ]
  }

  #checkov:skip=CKV_AZURE_218:Secure transit protocols used
  #checkov:skip=CKV_AZURE_120:WAF is enabled
}

resource "azurerm_monitor_diagnostic_setting" "agw_logs_monitor" {
  # App Gateway only deployed to the Test and Production subscription
  count = var.environment != "development" ? 1 : 0

  name                       = "${var.resource_name_prefix}-agw-mon"
  target_resource_id         = azurerm_application_gateway.agw[0].id
  log_analytics_workspace_id = var.logs_id

  enabled_log {
    category = "ApplicationGatewayFirewallLog"
  }

  timeouts {
    read = "30m"
  }

  lifecycle {
    ignore_changes = [metric]
  }
}
