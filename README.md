# HotelUp - Kitchen service
![Application tests](https://github.com/Wiaz24/HotelUp.Kitchen/actions/workflows/tests.yml/badge.svg)
![Github issues](https://img.shields.io/github/issues/Wiaz24/HotelUp.Kitchen)
[![Docker Image Size](https://badgen.net/docker/size/wiaz/hotelup.kitchen?icon=docker&label=image%20size)](https://hub.docker.com/r/wiaz/hotelup.kitchen/)

This service should expose endpoints on port `5006` starting with:
```http
/api/kitchen/
```

## Health checks
Health status of the service should be available at:
```http
/api/kitchen/_health
```
and should return 200 OK if the service is running, otherwise 503 Service Unavailable.

## Message broker
This service uses `MassTransit` library to communicate with the message broker. For the purpose of integration with
another MassTransit service, all published events are declared in the `HotelUp.Kitchen.Services.Events` namespace.

### AMQP Queues
This service creates queues that consume messages from the following exchanges:
- `HotelUp.Employee:EmployeeCreatedEvent` from [HotelUp.Employee](https://github.com/Wiaz24/HotelUp.Employee)
