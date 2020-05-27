---
weight: 1
version:
---

- {{% tag fixed %}} Runtime config for metrics sinks is not working correctly ([#1052](https://github.com/tomkerkhove/promitor/issues/1052))
- {{% tag removed %}} Default Prometheus configuration since we have multiple metric sinks nowadays. We are removing
 this since you cannot have duplicate Prometheus scraping endpoints. If you omit legacy configuration it will use
  `/metrics` as a default value which forces you to use different endpoint in the Prometheus metric sink.
