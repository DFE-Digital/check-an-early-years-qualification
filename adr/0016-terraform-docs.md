# 0016 - IaC Documentation

* **Status**: accepted

## Context and Problem Statement

It should be easy to tell what resources are included in the Infrastructure as Code (Terraform).  

## Decision Drivers

* Easy to implement
* Documentation in a central place, preferably _with_ the code

## Considered Options

* Manual maintenance
* GitHub `terraform-docs/gh-actions` action

GitHub action can be built into pipeline, to output Markdown README documentation, and automatically
commit it back to the repo. Initial results seemed comprehensive.

## Decision Outcome

GitHub `terraform-docs/gh-actions` action in pipeline.
