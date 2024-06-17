# 0015 - Cookie Policy

* **Status**: accepted

## Context and Problem Statement

The service must allow the user to accept or decline optional cookies, in a pattern consistent
with other government online services. There are multiple possible implementations.

## Decision Drivers

- Consistency with other government services
- Not reliant on client-side JavaScript
- Straightforward to implement the service's optional cookies

## Considered Options

- New JavaScript implementation
- Approaches taken by other DfE/digital government project teams

This has been implemented on several government digital services.

Several other teams' implementations suggested possible approaches. One in particular
seemed a close match to the service's requirements.

## Decision Outcome

The approach worked out by the 
[Schools Technology Services (STS) team](https://github.com/DFE-Digital/sts-plan-technology-for-your-school/tree/main)
was chosen as the model for this service's handling of web Cookie policy.