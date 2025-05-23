
# PTManagementSystem

A microservices-based PT management platform that streamlines appointment scheduling, user management, payment handling, and real-time notifications. Built with polyglot microservices using 2 DB technologies, event-driven architecture, and a modern, inspired by the aesthetics of gyms, user friendly functional frontend.

---

## Table of Contents

- [Architecture](#architecture)
- [Services](#services)
- [Environment Setup](#environment-setup)
- [Running Locally](#running-locally)
- [API Documentation](#api-documentation)
- [Authentication & Authorization](#authentication--authorization)
- [Event-Driven Messaging](#event-driven-messaging)
- [Contributing](#contributing)
- [License](#license)

---


## About the Project

**PT Management System** is a comprehensive web-based platform designed to manage physical therapy appointments efficiently for both clients and personal trainers (PTs). The system supports a full booking and payment workflow in a modular, scalable architecture powered by microservices.

### Core Features & User Flow

1. **User Authentication**
   - The system supports login, registration, and social login (Google).
   - Users can sign in either as **clients** (those seeking exercise sessions) or **trainers** (PTs offering sessions).
   - Authentication and role-based access control are managed via **Keycloak**, which stores identity, user roles, and permissions.

2. **Trainer Selection & Appointment Booking**
   - After logging in, a client can browse and select a trainer.
   - The client selects an available time slot and confirms the appointment.
   - Appointment data (including trainer ID, user ID, and timeslot) is stored in **MongoDB** by the **Appointment Service**.

3. **Payment Processing**
   - Once an appointment is created, the client is redirected to the **Payment Service**, which is built in Java using Spring Boot.
   - The system collects card information and securely processes payments.
   - Payment status (success/failure) is recorded in **AWS-hosted MySQL**.

4. **Real-Time Notifications**
   - When an appointment is successfully booked or a payment is processed, an event is published to **RabbitMQ**.
   - The **Notification Service** (built in Python using Flask) listens to these events and sends notifications (e.g., email confirmations or alerts) to users accordingly.

5. **Data Storage**
   - **MongoDB** is used for storing dynamic or flexible data like appointments and notifications.
   - **AWS MySQL** is used for structured relational data like user profiles and payment records.
   - Each service manages its own database schema to maintain loose coupling and autonomy.
6. **Frontend**
   - The entire workflow is integrated into a visually cohesive web interface built with Next.js and React.
   - The platform includes a dynamic header with button-based navigation, allowing users to easily access login, registration, appointment booking, and notification pages.
   - Clients can view available trainers and select appointment times through an intuitive calendar interface then select a slot.
   - Once a slot is selected, users are redirected to a secure payment page for payment processes and when it's done they receive real-time notification via Notifications page.


---

## How It's Implemented: Microservices in Action

The architecture follows the **microservice paradigm**, where each domain concern is isolated into its own service:

| Service         | Responsibility                                                                 |
|------------------|---------------------------------------------------------------------------------|
| **User Service** | Authentication, registration, identity via Keycloak and .NET                    |
| **Appointment Service** | Manages appointment creation and scheduling via Python + MongoDB         |
| **Payment Service** | Handles card input, transaction logic, and stores results via Java + MySQL  |
| **Notification Service** | Sends real-time updates via email or other channels using Python        |
| **Frontend (Next.js)** | React-based UI for booking, login, registration, and appointment management |

---

## Event-Driven Workflow (RabbitMQ)

To ensure **loose coupling and asynchronous communication**, the system uses **RabbitMQ** for event-based messaging:

1. When a client books an appointment:
   - `SlotReservedEvent` event is published.
   - **Notification Service** listens to the event and sends a booking confirmation.

2. When a payment is processed:
   - A `PaymentSucceededEvent` or `PaymentFailedEvent` event is published.
   - The appointment and user records are updated accordingly.
   - A notification is triggered based on the outcome.

---

## Scalability & Maintainability

This architecture provides:

- **Scalability**: Each service can scale independently based on load.
- **Maintainability**: Isolated codebases per domain make development and deployment modular.
- **Security**: Keycloak provides robust identity management, and sensitive data is isolated per service.

---

## Architecture

The system is composed of multiple loosely-coupled services communicating via **RabbitMQ**. Authentication is handled by **Keycloak**. Below is the tech stack and communication overview:


---

## Services

| Service        | Language     | DB        | Role                                 |
|----------------|--------------|-----------|--------------------------------------|
| **User**       | .NET 6       | MySQL     | User registration, auth, JWT issuing |
| **Payment**    | Java (Spring)| -     | Handles payment processing            |
| **Appointment**| Python (FastAPI) | MongoDB | Appointment CRUD                      |
| **Notification**| Python (Flask)  | MySQL | Sends emails / notifications          |
| **Frontend**   | Next.js      | -         | Web interface                         |
| **Message Bus**| RabbitMQ     | -         | Async inter-service events            |

---

## Environment Setup

Requirements:

- Docker & Docker Compose
- .NET 6 SDK
- Java 17 JDK + Maven
- Python 3.10+
- Node.js 18+
- Keycloak (configured with `ptmanagement-realm.json`)

---

##  Running Locally

### Using Docker

```bash
docker-compose up --build
```

> Includes MongoDB, RabbitMQ, Keycloak, all services, and the frontend.

---

## API Documentation

Each service exposes its own REST APIs:


 User            `http://localhost:5000/swagger` 
 Payment         `http://localhost:8081/swagger-ui.html` 
 Appointment     `http://localhost:8001/docs` 
 Notification    `http://localhost:8002/docs` 

---

## Authentication & Authorization

- Keycloak is used for identity and access management.
- Access tokens are passed via `Authorization: Bearer <token>`.
- Use `ptmanagement-realm.json` to configure Keycloak.

---

## Event-Driven Messaging

All async communication is done through RabbitMQ. Each service publishes and subscribes to topics as needed.

```
Event Flow:
→ Appointment Created
   → Notification Triggered
   → Payment Request Issued
```

- **Queue Names**: `reservationQueue`, `appointmentFailed`, `PaymentSucceededEvent` etc.
- Event handler jar: `event-listener-http-jar-with-dependencies.jar`

---

## Testing

Each service includes unit and integration tests. Example:

```bash
# .NET User


# Java Payment
.

# Python services

```

---

## Contributing

We welcome contributions! Please:

- Fork this repo
- Create a feature branch
- Follow existing code patterns and naming
- Submit a PR and describe your change

---

## License

MIT License © PT Team

---

## Resources

- [Keycloak Docs](https://www.keycloak.org/documentation.html)
- [RabbitMQ Tutorials](https://www.rabbitmq.com/getstarted.html)
- [FastAPI](https://fastapi.tiangolo.com)
- [Spring Boot](https://spring.io/projects/spring-boot)
- [.NET Docs](https://docs.microsoft.com/dotnet)
