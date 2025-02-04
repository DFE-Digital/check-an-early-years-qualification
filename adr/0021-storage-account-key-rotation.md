# 0021 - Storage account key rotation

* **Status**: Accepted

## Context and Problem Statement

As part of the IT Health Check (ITHC), it was recommended that the storage account keys are periodically rotated.
## Decision Drivers

* Ideally an automated process
* Little or no downtime to the service

## Considered Options

* Terraform
* Manual through the Azure Portal
* AZ Cli
* GitHub Workflow

## Decision Outcome

Chosen option: GitHub workflow. This can be run on a schedule and triggered automatically meaning no manual intervention required.