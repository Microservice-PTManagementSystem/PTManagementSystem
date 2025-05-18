FROM python:3.9

WORKDIR /app


RUN apt-get update && \
    apt-get install -y curl gnupg apt-transport-https unixodbc-dev && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql18

# Python bağımlılıklarını yükle
COPY ./notification/requirements.txt .
RUN pip install -r requirements.txt

# Uygulama dosyalarını kopyala
COPY ./notification /app

CMD ["python3", "main.py"]
