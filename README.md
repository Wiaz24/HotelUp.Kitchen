# HotelUp - Kitchen service
![dockerhub_badge](https://github.com/Wiaz24/HotelUp.Kitchen/actions/workflows/dockerhub.yml/badge.svg)

This service should expose endpoints on port `5006` starting with:
```http
/api/kitchen/
```

## Healthchecks
Health status of the service should be available at:
```http
/api/kitchen/_health
```
and should return 200 OK if the service is running, otherwise 503 Service Unavailable.
