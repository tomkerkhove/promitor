# Contributing to Promitor

First off, thank you for taking the time to contribute!

This guide provides guidelines on how to contribute, get involved or report feature requests & bugs. Following these guidelines helps maintainers and the community understand what your concern is and how we can approach it.

In this guide you can find some guidance on where you can help:

- [Where Can I Help?](#where-can-i-help)
- [Where Can I Help?](#where-can-i-help)
- [Pull Requests](#pull-requests)
- [Requesting A Feature](#requesting-a-feature)
   - [How Do I Create A (Good) Feature Request?](#how-do-i-create-a-good-feature-request)
- [Reporting Bugs](#requesting-features--reporting-bugs)
   - [How Do I Submit A (Good) Bug Report?](#how-do-i-submit-a-good-bug-report)
   
> **Note:** If you find a **Closed** issue that seems like it is the same thing that you're experiencing, open a new issue and include a link to the original issue in the body of your new one.

## Where Can I Help?

Promitor uses a [Contribution License Agreement (CLA)](https://gist.github.com/tomkerkhove/49506ad58e30076518609511619dc026) which needs to be signed for all contributions and is handled by [CLA Assistant](https://cla-assistant.io).

The Contribution License Agreement (CLA) is required for all parties to align on the expectations of the contributions in order to avoid disputes in the future.

The CLA Assistant will walk you through the process during your first pull request.

## Where Can I Help?

Unsure where to begin contributing to Promitor? You can start by looking through these `beginner` and `help-wanted` issues:

- [Beginner issues](https://github.com/tomkerkhove/promitor/issues?q=is%3Aopen+is%3Aissue+label%3Abeginner) - issues are very small and good to learn how Promitor works.
- [Help wanted issues](https://github.com/tomkerkhove/promitor/issues?q=is%3Aopen+is%3Aissue+label%3Ahelp-wanted) - issues which should be a bit more involved than `beginner` issues.

## Pull Requests

- Always **create an issue and discuss changes before sending PRs**
  - Allow us to assign the issue to you and avoid multiple people working on the same thing.
  - This is to avoid wasting your time because it's not part of the roadmap or not as how we would approach it.
- Fill in [the required template](PULL_REQUEST_TEMPLATE.md)
- Do not include issue numbers in the PR title
- Include thoughtfully-worded, well-structured OpenAPI specs when appropriate.

## Requesting A Feature

This section guides you through submitting a feature request - We are open to everything.

Every feature request will be considered and determined if it's in scope of the project, what the customer demand is and what priority it has.

Before creating a new feature request, please check the issue tracker as you might find out that you don't need to create one. In that case, just join the discussion and explain your scenario.

When you are creating a **feature request**, please use the [Feature Request template](./ISSUE_TEMPLATE/Feature_request.md) and describe what you would like to have and how this would help you.

### How Do I Create A (Good) Feature Request?

Explain the problem and include additional details to help maintainers reproduce the problem:

- **Use a clear and descriptive title** for the feature request to set the scene.
- **Explain what you would like to see, how it could help and if it's blocking you.**
- **Provide specific examples to demonstrate how you would expect it to work**. This would help us get an understanding of what it should look like and how it should work.
- **We are open for suggestions** on how to implement it in terms of runtime, configuration, deployment, etc 
- **We are open for pull requests, but wait for confirmation first**, see ["Pull Requests"](#pull-requests).

## Reporting Bugs

This section guides you through submitting a bug report for Promitor - Before creating bug reports, please check issue tracker as you might find out that you don't need to create one.

- When you are creating a **bug report**, please use the [Bug Report template](./ISSUE_TEMPLATE/Bug_report.md) and [include as many details as possible](#how-do-i-submit-a-good-bug-report). The information it asks for helps us resolve issues faster.
- When you are creating a **feature request**, follow the guidance in ["Requesting A Feature"](#requesting-a-feature).

### How Do I Submit A (Good) Bug Report?

Explain the problem and include additional details to help maintainers reproduce the problem:

- **Use a clear and descriptive title** for the issue to identify the problem.
- **Describe the exact steps which reproduce the problem** in as many details as possible. For example, start by explaining how you started Promitor, e.g. which command exactly you used to start the Docker image. When listing steps, **don't just say what you did, but explain what information the logs provided**.
- **Provide specific examples to demonstrate the steps**. Include links to files or GitHub projects, or copy/pasteable snippets, which you use in those examples. If you're providing snippets in the issue, use [Markdown code blocks](https://help.github.com/articles/markdown-basics/#multiple-lines).
- **Describe the behavior you observed after following the steps** and point out what exactly is the problem with that behavior.
- **Explain which behavior you expected to see instead and why.**
- **If you're reporting that Promitor crashed**, include a crash report with a stack trace from logs. Include the crash report in the issue in a [code block](https://help.github.com/articles/markdown-basics/#multiple-lines) or put it in a [gist](https://gist.github.com/) and provide link to that gist.

Include details about your configuration and environment:

- **Which version of Promitor are you using?**
- **What's the name and version of the container host that you're using**? Is it Docker, Kubernetes,...?
- **What configuration is it using and how does your scraping declaration look like?**
