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
    - Label it with `deprecation` label
    - Plan it for the next major release
2. Document deprecated feature on [`changelog.promitor.io`](http://changelog.promitor.io/)
3. Announce deprecation in [`📢 Promitor Updates`](https://github.com/tomkerkhove/promitor/issues/668)

## Deprecation Notice

```markdown
```

## Deprecation Template

```yaml
---
title:
subtitle: created on {{ now.Format "2006-01-02" }}
date: 2018-09-02T20:46:47+02:00
removal_date: 2019-07-07
weight:
version:
---

#### Swagger 2.0

Lorem markdownum fulmen repetita atro praecipitem tela accepto quantumque
funeribus spes; casus memorabile. [Concidit culmine](http://ora-tyria.net/),
unda ad **adhuc** liquidi cognata, sua cetera; ceu iam facientia quem. Regina
referre tibi places cum et meritorum, **in atque**, capillos et deos. Di sua
cepit excidit pectore probarunt fatale muta vento in Tamasenum.

Templa hunc, exosa felix os temerasse Boreas facies nam ferre regimen! Fidemque
signans stant Volturnus dicta vides, utque caelo pallidiora.

Tandem stat surgis. Rerum nati arbitrio, nactusque dilectos a!
```

## Deprecation Announcement

```markdown
**<feature name> is being deprecated** as of <deprecation-version> and removed in <removal-version>.

For more information, join the conversation in our [deprecation issue](<url-to-deprecation>).
```
