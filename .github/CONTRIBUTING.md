# Contributing to Promitor

:+1::tada: First off, thanks for taking the time to contribute! :tada::+1:

## Reporting Bugs

This section guides you through submitting a bug report for Promitor. Following these guidelines helps maintainers and the community understand your report :pencil:, reproduce the behavior :computer: :computer:, and find related reports :mag_right:.

Before creating bug reports, please check issue tracker as you might find out that you don't need to create one.

- When you are creating a **bug report**, please use the [Bug Report template](./ISSUE_TEMPLATE/Bug_report.md) and [include as many details as possible](#how-do-i-submit-a-good-bug-report). The information it asks for helps us resolve issues faster.
- When you are creating a **feature request**, please use the [Feature Request template](./ISSUE_TEMPLATE/Feature_request.md) and describe what you would like to have and how this would help you.

> **Note:** If you find a **Closed** issue that seems like it is the same thing that you're experiencing, open a new issue and include a link to the original issue in the body of your new one.

### How Do I Submit A (Good) Bug Report or Feature Request?

Explain the problem and include additional details to help maintainers reproduce the problem:

* **Use a clear and descriptive title** for the issue to identify the problem.
* **Describe the exact steps which reproduce the problem** in as many details as possible. For example, start by explaining how you started Promitor, e.g. which command exactly you used to start the Docker image. When listing steps, **don't just say what you did, but explain what information the logs provided**.
* **Provide specific examples to demonstrate the steps**. Include links to files or GitHub projects, or copy/pasteable snippets, which you use in those examples. If you're providing snippets in the issue, use [Markdown code blocks](https://help.github.com/articles/markdown-basics/#multiple-lines).
* **Describe the behavior you observed after following the steps** and point out what exactly is the problem with that behavior.
* **Explain which behavior you expected to see instead and why.**
* **If you're reporting that Promitor crashed**, include a crash report with a stack trace from logs. Include the crash report in the issue in a [code block](https://help.github.com/articles/markdown-basics/#multiple-lines) or put it in a [gist](https://gist.github.com/) and provide link to that gist.

Include details about your configuration and environment:

* **Which version of Promitor are you using?**
* **What's the name and version of the container host that you're using**? Is it Docker, Kubernetes,...?
* **What configuration is it using and how does your scraping declaration look like?**

## Your First Code Contribution

Unsure where to begin contributing to Atom? You can start by looking through these `beginner` and `help-wanted` issues:

* [Beginner issues](https://github.com/tomkerkhove/promitor/issues?q=is%3Aopen+is%3Aissue+label%3Abeginner) - issues which should only require a few lines of code, and a test or two.
* [Help wanted issues](https://github.com/tomkerkhove/promitor/issues?q=is%3Aopen+is%3Aissue+label%3Ahelp-wanted) - issues which should be a bit more involved than `beginner` issues.

## Pull Requests

* Fill in [the required template](PULL_REQUEST_TEMPLATE.md)
* Always create an issue and discuss changes before sending PRs. This is to avoid wasting your time because it's not part of the roadmap or not as how we would approach it.
* Do not include issue numbers in the PR title
* Include thoughtfully-worded, well-structured OpenAPI specs when appropriate.

## Styleguides

### Git Commit Messages

* Use the present tense ("Add feature" not "Added feature")
* Use the imperative mood ("Move cursor to..." not "Moves cursor to...")
* Reference issues and pull requests liberally after the first line
* When only changing documentation, include `[ci skip]` in the commit title
* Consider starting the commit message with an applicable emoji:
    * :art: `:art:` when improving the format/structure of the code
    * :racehorse: `:racehorse:` when improving performance
    * :memo: `:memo:` when writing docs
    * :whale: `:whale:` when fixing something Docker related
    * :cloud: `:cloud:` when fixing something Azure related
    * :fire: `:fire:` when fixing something Prometheus related
    * :bar_chart: `:bar_chart:` when fixing something metrics related
    * :mag_right: `:mag_right:` when fixing something operations related
    * :bug: `:bug:` when fixing a bug
    * :fire: `:fire:` when removing code or files
    * :green_heart: `:green_heart:` when fixing the CI build
    * :white_check_mark: `:white_check_mark:` when adding tests
    * :lock: `:lock:` when dealing with security
    * :arrow_up: `:arrow_up:` when upgrading dependencies
    * :arrow_down: `:arrow_down:` when downgrading dependencies

