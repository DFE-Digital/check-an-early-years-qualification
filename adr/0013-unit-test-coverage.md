# 0013 - Unit Tests Code Coverage

* **Status**: accepted

## Context and Problem Statement

What tool should be adopted in Check and Early Years Qualification service pipelines
to ensure adequate unit-test coverage? 

## Decision Drivers

- Easy to implement in GitHub pipelines
- Can fail a build if minimum threshold not met

## Considered Options

- Coverlet
- SonarQube / SonarCloud

SonarQube / SonarCloud may be a better long term solution, since it will meet the requirement
for code coverage _and_ static analysis of code quality. Wider discussion across the DfE
Early Years Portfolio may be warranted first, as other teams have expressed an interest also,
and a licence may be required.

## Decision Outcome

Coverlet: can be implemented straight away; will produce code coverage reports as well
as providing a quality gate in the build pipelines.