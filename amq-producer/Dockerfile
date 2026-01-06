# ===============================
# Stage 1 - Build
# ===============================
FROM maven:3.9.6-eclipse-temurin-17 AS build

WORKDIR /build

COPY pom.xml .
RUN mvn -B dependency:go-offline

COPY src src
RUN mvn -B package -DskipTests

# ===============================
# Stage 2 - Runtime
# ===============================
FROM registry.access.redhat.com/ubi9/openjdk-17-runtime

WORKDIR /work

# Copia já com o usuário correto
COPY --from=build --chown=1001:0 /build/target/quarkus-app /work/

USER 1001

EXPOSE 8080

CMD ["java", "-jar", "quarkus-run.jar"]