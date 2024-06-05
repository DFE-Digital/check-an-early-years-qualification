# 0014 - C# Coding Standards

* **Status**: accepted

## Context and Problem Statement

Several different standards have been adopted in proof-of-concept C# code. The team
wants to adopt a single standard that is easy to implement.

## Decision Drivers

- Industry best practice
- Easy to implement

## Considered Options

- Formatting standards: tabs or spaces; alignment
- Naming: capitalisation/case
- Code quality standards: security; structured logging; SOLID principles

## Decision Outcome

Standards formally documented internally on Confluence: [C# coding standards](https://dfedigital.atlassian.net/wiki/spaces/EYQB/pages/4364075045/C+coding+standards).

Examples of the standards we agreed follow. The standards are a living document, it is expected they will be
extended during the project.

### Formatting:
- four spaces for indentation
- consistent alignment
- Allman style (code block on new line)
- braces align
- one statement per line
- blank lines to aid readability

### Naming
- meaningful, descriptive names: clarity of intention
- `PascalCase` for classes, methods, public methods, constants, namespaces
- `camelCase` for method parameters and local variables
- avoid acronyms or abbreviations except very common ones
- correct spellings, English usage

### Quality
- no secrets in repo
- don't commit commented-out code
- use [SOLID principles](https://en.wikipedia.org/wiki/SOLID)
- automated unit tests