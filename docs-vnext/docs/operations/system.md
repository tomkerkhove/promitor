# System

![Resource Discovery Support Badge](https://img.shields.io/badge/Support%20for%20Resource%20Discovery-Yes-green.svg)
![Scraper Support Badge](https://img.shields.io/badge/Support%20for%20Scraper-Yes-green.svg)

Promitor provides a basic system endpoint that provides information about itself such as its version.

## Consuming the System endpoint

You can check the status with a simple `GET`:

```shell
❯ curl -i -X GET "http://<uri>/api/v1/system"
```

## Exploring our REST APIs

We provide API documentation to make it easier for you to consume our REST APIs them:

- **OpenAPI 3.0 format** ![Availability Badge](https://img.shields.io/badge/Available%20Starting-v1.1-green.svg)
  - You can explore it with OpenAPI UI on `/api/docs`
  - You can find the raw documentation on `/api/v1/docs.json`
- **Swagger 2.0 format** [![Deprecation Badge](https://img.shields.io/badge/Deprecated%20as%20of-v1.1-red)](http://changelog.promitor.io/)
  - You can explore it with Swagger UI on `/swagger`
  - You can find the raw documentation on `/swagger/v1/swagger.json`
