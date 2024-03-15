# 0008 - Container Registry

* **Status**: Accepted

## Context and Problem Statement

As the application will be containerised with Docker where is the best place solution to store the associated Docker images?

## Decision Drivers

* Within DfE’s Technical Guidance
* Compatible with selected hosting platform
* Simple with minimal configuration

## Considered Options

* Azure Container Registry
* GitHub Container Registry

## Decision Outcome

Chosen option: [GHCR](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry) as this is within source repository making configuration simpler.