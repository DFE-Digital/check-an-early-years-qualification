# 0017 - Access Guard in Pilot

* **Status**: proposed

## Context and Problem Statement

The service should not be public until it has achieved authority to operate. This will not happen until
after the completion and acceptance of the private Beta phase, when the service is ready to move to
public Beta.

Because it is a public service, there is no authentication or user recognition functionality. Some
other means is therefore necessary to remove the service from public view, and prevent its indexing
by search engines.

Development and test deployments of the service will _always_ need to be hidden from public view. 

## Decision Drivers

- No requirement for the service to implement authentication solution
- Requirement for invited users to be authorised to access a non-public instance of the service
    - …for example early adopters of the service in private beta
- Authorisation secret(s) can be changed
- Authorisation can be easily switched off in production environments
    - …but left on in lower environments

## Considered Options

- Authentication solution
- IP allow list
- Application Gateway configuration
- Challenge page for secret to be shared with permitted users

Implementing an authentication solution is overkill for this requirement, would be
inconvenient for users, and would give an unrepresentative experience of the service.

IP allow lists, and Application Gateway configuration, would be onerous to maintain. (For instance,
before private beta it might be necessary to configure a whole tranche of individual
user-filtering criteria, which would have to be maintained, and then deleted on
entering public beta.)

A challenge page would be relatively easy to implement (action filter on all controllers),
configure (a single "secret value" configuration), change (edit the secret value config),
turn off (on/off configuration), and share with users (a single value for them to enter).

## Decision Outcome

Challenge page. All other pages to redirect to the challenge page until user has entered
configurable secret value. Once user has entered secret value, other areas of the service
are all accessible.