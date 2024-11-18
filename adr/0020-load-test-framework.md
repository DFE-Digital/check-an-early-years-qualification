# 0020 - Load Testing Framework

* **Status**: accepted

## Context and Problem Statement

To ensure confidence in the service's performance under user load.

## Decision Drivers

* Ease of maintenance
* Established practice among other DfE service development teams
* Ability to use existing Cypress (or Playwright) tests

## Considered Options

* Azure Load Test
* JMeter
* Grafana k6

Azure Load Test can be fed `*.jmx` files from JMeter. However, JMeter itself seems a little
old fashioned nowadays, and was not designed with ease of development & maintenance, nor
for cloud native infrastructure. Also, Azure Load Test incurs a cost to run.

Grafana k6 is in use by other DfE teams, and investigation showed it is flexible
and easy to use.

## Decision Outcome

Grafana k6.
