# Deprecating A Feature

This guide walks through the process for deprecating a feature.

## Why? When? What now?!

Deprecations are never fun so we should elaborate on why it's being deprecated
and what the alternative is.

Every deprecation should explicitly mention in what version it is getting
deprecated and where it's being removed. Once removed, what is the impact?

## Procedure

Every feature that is being deprecated should follow this procedure:

1. Open issue as deprecation notice
    - Label it with `deprecation-notice` label
    - Plan it for the next major release
2. Document deprecated feature on [`changelog.promitor.io`](http://changelog.promitor.io/)
3. Announce deprecation in [`📢 Promitor Updates`](https://github.com/tomkerkhove/promitor/issues/668)

<details>
<summary><b>Deprecation Template</b></summary>

```yaml
---
title:
subtitle: created on {{ now.Format "2006-01-02" }}
date: 2018-09-02T20:46:47+02:00
removal_date: 2019-07-07
weight:
version:
---

#### <title>
###### Deprecated as of v<deprecation-version> and will be removed in v<removal-version>

OpenAPI v3.0 has been released in 2017 which is the new industry standard.
With Promitor, we want to support that standard and have decided to go forward
with 3.0 instead of 2.0.

We've added support for OpenAPI 3.0 in Promitor v1.1 next to Swagger 2.0 along with
OpenAPI UI 3.0.

**Impact:** <impact>

**Alternative:** <alternative>.

**Discussion:** [GitHub Issue #<issue-number>](https://github.com/tomkerkhove/promitor/issues/<issue-number>)
```

</details>

<details>
<summary><b>Deprecation Announcement</b></summary>

```markdown
**<feature name> is being deprecated** as of <deprecation-version> and removed in <removal-version>.

For more information, join the conversation in our [deprecation issue](<url-to-deprecation>).
```

</details>

<details>
<summary><b>Deprecation Notice</b></summary>

```markdown
OpenAPI v3.0 has been released in 2017 which is the new industry standard.
With Promitor, we want to support that standard and have decided to go forward
with 3.0 instead of 2.0.

We've added support for OpenAPI 3.0 in Promitor v1.1 next to Swagger 2.0 along with
OpenAPI UI 3.0.
Deprecated as of v<deprecation-version> and will be removed in v<removal-version>

**Deprecated as of:**
v<deprecation-version>

**Will be removed in:**
v<removal-version>

**Impact:**
<impact>

**Alternative:**
<alternative>.
```

</details>
