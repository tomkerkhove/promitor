<img src="https://static.scarf.sh/a.png?x-pxid=a425a60c-d7db-4b52-a4a1-7346343f9f8e" />
<!-- Because this file contains custom formatting for the heading, we need to
    disable some of the markdownlint rules -->
<!-- markdownlint-disable -->
<p align=center><img src="./docs/media/logos/promitor.png" alt="Promitor Logo" height="150"></p>

<h1 align="center">Bringing Azure Monitor metrics where you need them.</h1>

<p align="center">
    <a href="./LICENSE" rel="nofollow"><img src="https://img.shields.io/github/license/mashape/apistatus.svg?style=flat-square" alt="License"></a>
<a href="https://hub.docker.com/r/tomkerkhove/promitor-agent-scraper/" rel="nofollow"><img src="https://img.shields.io/docker/pulls/tomkerkhove/promitor-agent-scraper.svg?style=flat-square" alt="Docker Pulls"></a>
<a href="https://dev.azure.com/tomkerkhove/Promitor/_build/latest?definitionId=50&branchName=master" rel="nofollow"><img src="https://img.shields.io/azure-devops/build/tomkerkhove/promitor/50/master.svg?label=Scraper%20Agent%20-%20CI&style=flat-square" alt="Build Status"></a>
<a href="https://hub.docker.com/r/tomkerkhove/promitor-agent-scraper/" rel="nofollow"><img src="https://img.shields.io/docker/stars/tomkerkhove/promitor-agent-scraper.svg?style=flat-square" alt="Docker Stars"></a>
<a href="https://artifacthub.io/packages/search?repo=promitor" rel="nofollow"><img src="https://img.shields.io/endpoint?url=https://artifacthub.io/badge/repository/promitor&style=flat-square" alt="Artifact Hub"></a>
<a href="https://github.com/users/tomkerkhove/sponsorship" rel="nofollow"><img src="https://img.shields.io/badge/Donate%20via-GitHub-blue.svg?style=flat-square" alt="Donate"></a> <a href="https://app.fossa.com/projects/git%2Bgithub.com%2Ftomkerkhove%2Fpromitor?ref=badge_shield" alt="FOSSA Status"><img src="https://app.fossa.com/api/projects/git%2Bgithub.com%2Ftomkerkhove%2Fpromitor.svg?type=shield"/></a>
</p>

**Promitor** is an Azure Monitor scraper which makes the metrics available through a scraping endpoint for Prometheus or push to a StatsD server.

## Documentation

All documentation is available on [promitor.io](https://docs.promitor.io)

## End-users

We are proud to have the following end-users(s) running Promitor in production:

![Adfinis](./docs/media/logos/end-users/adfinis.png)
![Adobe](./docs/media/logos/end-users/adobe.png)
![Albert Heijn](./docs/media/logos/end-users/albert-heijn.png)
![Axon](./docs/media/logos/end-users/axon.png)
![Bryte Blue](./docs/media/logos/end-users/bryte-blue.png)
![ResDiary](./docs/media/logos/end-users/resdiary.png)
![theTradeDesk](./docs/media/logos/end-users/the-trade-desk.png)
![Trynz](./docs/media/logos/end-users/trynz.png)
![Vsoft](./docs/media/logos/end-users/vsoft.png)
![Walmart Labs](./docs/media/logos/end-users/walmart-labs.jpg)

Are you a Promitor user? Let us know and [get listed](https://forms.gle/hjcpaaVFa1A1hZaK6)!

## Contribution

All contribution guidelines can be found [here](./.github/CONTRIBUTING.md). We
welcome bug reports, improvement suggestions and pull requests.

Want to see support for a scraper that is not [already supported](https://docs.promitor.io/configuration/v2.x/metrics/#supported-azure-services)?
You can contribute by [adding one yourself](adding-a-new-scraper.md)!

Information about making changes to Promitor can be found [here](contributing.md).

### Testing Infrastructure

Our testing infrastructure is managed through Bicep and is open to contributions on [promitor/testing-infrastructure](https://github.com/promitor/testing-infrastructure).

## Support

Learn more about our support options [here](https://github.com/tomkerkhove/promitor/blob/master/SUPPORT.md).

Thanks for those who are supporting us via [GitHub Sponsors](https://github.com/sponsors/tomkerkhove/).

[![Carlo Garcia-Mier](./media/supporters/CarloGarcia.jpg)](https://github.com/CarloGarcia)
[![Jorge Turrado Ferrero](./media/supporters/JorTurFer.jpg)](https://github.com/JorTurFer)
[![Karl Ots](./media/supporters/karlgots.jpg)](https://github.com/karlgots)
[![Loc Mai](./media/supporters/locmai.jpg)](https://github.com/locmai)
[![Lovelace Engineering](./media/supporters/LovelaceEngineering.png)](https://github.com/LovelaceEngineering)
[![Nills Franssens](./media/supporters/nillsf.jpg)](https://github.com/NillsF)
[![Richard Simpson](./media/supporters/RichiCoder1.jpg)](https://github.com/RichiCoder1)
[![Sam Vanhoutte](./media/supporters/samvanhoutte.png)](https://github.com/samvanhoutte)

## Security

Learn more about our security policy [here](https://github.com/tomkerkhove/promitor/security/policy).

## Performance

Learn more about our performance tests [here](tests/README.md).

## Donate

Promitor is fully OSS and built free-of-charge, however, if you appreciate my work
you can do a small donation.

[![Donate](https://img.shields.io/badge/Donate%20via-GitHub-blue.svg?style=flat-square)](https://github.com/sponsors/promitor)

## Get in touch

Do you have a security issue to report or just want to privately contact me? Feel
free to [write me an email](mailto:kerkhove.tom@gmail.com) or [get listed as a user](https://forms.gle/hjcpaaVFa1A1hZaK6).

## Thank you

We'd like to thank all the services, tooling & NuGet packages that support us -
 [Thank you](https://docs.promitor.io/thank-you)!

## License Information

This is licensed under The MIT License (MIT). Which means that you can use, copy,
modify, merge, publish, distribute, sublicense, and/or sell copies of the web
application. But you always need to state that Tom Kerkhove is the original author
of this web application.

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Ftomkerkhove%2Fpromitor.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Ftomkerkhove%2Fpromitor?ref=badge_large)
