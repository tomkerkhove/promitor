# (Automated) Tests

This folder provides all resources related to (automated) tests that are not part of our unit & integration test suite.

## Performance tests

You can find performance tests in the `performance` folder which are using [Artillery](https://www.artillery.io/).

Easily install Artillery on your machine with the following command:

```shell
npm install -g artillery@latest
```

Verify that the installation was successful:

```shell
$ artillery dino
 ------------
< Artillery! >
 ------------
          \
           \
               __
              / _)
     _/\/\/\_/ /
   _|         /
 _|  (  | (  |
/__.-'|_|--|_|
```

*[Docs](https://www.artillery.io/docs/guides/getting-started/installing-artillery#installing-artillery)*

### Scraper Agent

Automatically send a growing number of requests (5 to 25) to scrape metrics over 60 seconds:

```shell
artillery run .\performance\steady-load-scraper-scrape.yml
```

### Resource Discovery Agent

Automatically send a growing number of requests (5 to 25) to probe the health endpoint over 60 seconds:

```shell
artillery run .\performance\steady-load-resource-discovery-health.yml
```
