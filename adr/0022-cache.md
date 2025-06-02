# 0021 - Caching Contentful responses

* **Status**: Accepted

## Context and Problem Statement

The main motivation was to mitigate the dependency on Contentful. If Contentful were to become
unavailable it would be advantageous if our service could continue to operate for a while
without users noticing. DfE's Contentful agreement has a stated SLA of 99.8% uptime, which
suggests clients should allow for it being unavailable for 80 minutes in any 4-week period.

A secondary consideration is that since content changes are infrequent, it would make sense
for the service to cache Contentful API responses rather than accessing the Contentful API
across the public Internet every time a user accesses a page.

## Decision Drivers

* Caching should be transparent to the service application logic
* Cache should be distributed (one cache instance servicing all application instances)
* Reasonable cost

## Considered Options

* Azure Cache for Redis
* Azure Managed Redis
* In Memory cache
* No caching
* Caching raw Contentful API response bodies
* Caching in-memory objects

The team agreed that caching would bring immediate benefits, and caching features could be
gradually phased in until the point was reached that implemented the original motivation to
allow the service to operate even if Contentful was unavailable.

Caching Contentful API responses can be implemented at the `HttpClient` level, and thus be
transparent to the application logic, and very simple to unit test.
Caching in-memory objects on the other hand would involve accessing the cache
as part of application logic, and thus be more onerous to write automated tests
for, and therefore more expensive to maintain in the future.

## Decision Outcome

Chosen options: Azure Cache for Redis, Standard tier; cache raw Contentful API response bodies.
