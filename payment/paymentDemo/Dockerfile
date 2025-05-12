# === Build aşaması ===
FROM maven:3.9.5-eclipse-temurin-17 AS build
ARG BUILD_CONFIGURATION=release
WORKDIR /app

# pom.xml ve src klasörünü kopyala
COPY pom.xml .
COPY src ./src

# Bağımlılıkları indir
RUN mvn dependency:resolve

# Projeyi build et
RUN mvn clean package -DskipTests

# === Final aşama (sadece jar dosyasıyla hafif image) ===
FROM eclipse-temurin:17-jdk AS final
WORKDIR /app
COPY --from=build /app/target/*.jar app.jar
COPY ../application.yml ./application.yml
#bu copy de iyzicoyu dockera image olarak dahil etsin diye eklendi
ENTRYPOINT ["java", "-jar", "app.jar", "--spring.config.location=classpath:/,file:./application.yml"]
