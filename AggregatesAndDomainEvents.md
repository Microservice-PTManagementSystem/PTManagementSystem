# PT Management System

## _Aggregates_

#### 1. Appointment Aggregate

Root Aggregate: Appointment
Value Objects:

-  SlotInfo (DateTime start, Duration duration, Location location)
-  AppointmentStatus (SCHEDULED, CONFIRMED, COMPLETED, CANCELLED, RESCHEDULED)
-  Price
-  AppointmentType (SINGLE, RECURRING)
   Entities:
-  AppointmentNotes
   References:
-  UserId (client)
-  TrainerId
-  PaymentId

#### 2. User Aggregate

Root Aggregate: User
Value Objects:

-  UserType (CLIENT, TRAINER)
-  UserProfile
   -  PersonalInfo (name, dob, gender)
   -  ContactInfo (email, phone)
   -  Address
-  PaymentInfo
-  TrainerProfile (only when UserType is TRAINER)
   -  Specializations
   -  Certifications
   -  AvailableSlots
   -  Experience
   -  Rate

#### 3. Payment Aggregate

Root Aggregate: Payment
Value Objects:

-  PaymentMethod
-  Status (PENDING, COMPLETED, FAILED, REFUNDED)
-  Amount
-  BillingDetails
-  TransactionId
   References:
-  UserId
-  AppointmentId

#### 4. Notification Aggregate

Root Aggregate: Notification
Value Objects:

-  DeliveryChannel (EMAIL, SMS, PUSH)
-  SendStatus (PENDING, SENT, FAILED)
-  NotificationContent
-  NotificationType (APPOINTMENT_REMINDER, PAYMENT_CONFIRMATION, etc.)
   References:
-  UserId
-  RelatedEntityId (could be AppointmentId or PaymentId)

---

## _Domain Events_

#### User Microservice:

1. _UserRegisteredEvent_: It is published when a new user registers on the system. Notification microservice listens and sends a welcome notification.
2. _TrainerDeletedEvent_: It is published when 'trainer user' is deleted. It is used to remove the appointments created by the trainer.
3. _PaymentInfoUpdatedEvent_: It is published when the user changes their payment information. It allows updating the payment information used in the payment microservice.

#### Appointment Microservice:

1. _SlotReservedEvent_: It is published when a user wants to make an appointment. It ensures that other users cannot make the same appointment until the payment related event is published from the payment microservice.
2. _SlotConfirmedEvent_: It is published when a user wants to make an appointment. It is listened to by the payment microservice and used to initiate the payment process.
3. _SlotReleasedEvent_: It is published if the appointment is not finalized or if the payment fails. It allows other users to be allowed to book this appointment.
4. _AppointmentCompletedEvent_: It is published when the appointment is completed.
5. _AppointmentRescheduledEvent_: It is published when the appointment is rescheduled. It is used to send notification of this change to the user and trainer
6. _AppointmentCanceledEvent_: It is published when the appointment is canceled. It is used to inform the user and trainer.

#### Payment Microservice:

1. _PaymentInitiatedEvent_: It is published when the payment process is initiated. It is used to integrate with the credit card system.
2. *PaymentSucceededEven*t: It is published when the payment is successful. It is used to create an invoice and send a notification to the user and trainer.
3. _PaymentFailedEvent_: It is published when the payment fails. It is used to send a warning message to the user and to retry.
4. _RefundIssuedEvent_: It is published when the user is reimbursed.

#### Notification Microservice:

1. _NotificationSentEvent_: It is published when the notification is sent. It can be used creating a submission report and for error tracking.
2. _NotificationFailedEvent_: It is published when the notification cannot be sent. It can be used to retry to send or change the sending method (email, message, etc.).
