FROM openjdk:21

WORKDIR /app

ARG JAR_FILE=target/paymentDemo-0.0.1-SNAPSHOT.jar

COPY ${JAR_FILE} payment-service.jar

ENTRYPOINT ["java", "-jar", "payment-service.jar"]
