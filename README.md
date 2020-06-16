# Kangaroo
[![Gitter](https://badges.gitter.im/KangarooApp/community.svg)](https://gitter.im/KangarooApp/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

Kangaroo is containerized microservices application based on .NET Core. The project demonstrates how to develop small microservices for larger applications using containers, orchestration, service discovery, gateway, and best practices. You are always welcome to improve code quality and contribute it, if you have any questions or issues don't hesitate to ask in our [gitter](https://gitter.im/KangarooApp/community) chat.

## Motivation

- Developing independently deployable and scalable micro-services
- Developing cross-platform beautiful mobile apps using React native
- Configuring CI/CD pipelines using Travis CI, Github Actions, automated deployments using Google Kubernetes Engine, Docker Containers
- Using modern technologies such as gRPC, RabbitMQ, Service meshes
- Writing clean, maintainable and fully testable code, Unit Testing, Integration Testing and Mocking practices
- Using SOLID Design Principles
- Using Design Patterns and Best practices such as DDD and CQRS

## Architecture overview

The architecture proposes a microservice oriented architecture implementation with multiple autonomous microservices (each one owning its own data/db) and implementing different approaches within each microservice (simple CRUD vs. DDD/CQRS patterns) using REST/HTTP as the communication protocol between the client apps, and supports asynchronous communication for data updates propagation across multiple services based on gRPC, Integration Events and an Event Bus(RabbitMQ)

<img src="art/KangarooArchitecture.png"/>

GitHub Actions:

![Kangaroo.sln build](https://github.com/Jamaxack/Kangaroo/workflows/Kangaroo.sln%20build/badge.svg?branch=master)

![Courier.API](https://github.com/Jamaxack/Kangaroo/workflows/courier-api/badge.svg?branch=master)

![Delivery.API](https://github.com/Jamaxack/Kangaroo/workflows/delivery-api/badge.svg?branch=master)

![Pricing.API](https://github.com/Jamaxack/Kangaroo/workflows/pricing-api/badge.svg?branch=master)

Travis CI: 

[![Build Status](https://travis-ci.org/Jamaxack/Kangaroo.svg?branch=master)](https://travis-ci.org/Jamaxack/Kangaroo)

## Contributing

1. Fork it
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Adds some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request

## License

Code released under the [MIT license](https://github.com/Jamaxack/Kangaroo/blob/master/LICENSE).
