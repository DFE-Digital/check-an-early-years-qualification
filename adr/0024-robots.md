# 0024 - Robots.txt Policy

* **Status**: Accepted

## Context and Problem Statement

A robots.txt file is used to manage the crawling and indexing of the service by search engine.
Omitting a robots.txt file leads to 404 errors in the logs as crawlers attempt to access the robots.txt.
If a robots.txt is not found crawlers will still attempt to index the site.

## Decision Drivers

* Government digital service guiance
* Omitting the robots.txt file leads to 404 errors in the logs
* Straightforward to implement in the public www folder

## Considered Options

This follows the guidance in the service manual

## Decision Outcome

[GOV.UK service manual](https://www.gov.uk/service-manual/technology/get-a-domain-name)
Guidance from the service manual indicates that the robots.txt file should not prevent crawling of the service and instead use the noindex directive meta tags on individual pages to manage indexing.
This means that the robots.txt file should not disallow any user agents and paths.
The noindex directive has been added to the _layout and is applied to all pages by default.
A robots.txt file has been added to the wwwroot folder which has a wildcard from all user agents and allows all paths.