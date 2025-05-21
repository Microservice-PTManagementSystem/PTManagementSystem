FROM python:3.9

WORKDIR /app

# Update package lists and install dependencies
RUN apt-get update && \
    apt-get install -y curl gnupg apt-transport-https && \
    # Install Microsoft's public key and repository
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    # Remove conflicting packages to avoid overwrite issues
    apt-get remove -y unixodbc-dev unixodbc-common libodbc2 libodbcinst2 && \
    # Install msodbcsql18 and dependencies
    ACCEPT_EULA=Y apt-get install -y msodbcsql18 unixodbc unixodbc-dev && \
    # Clean up to reduce image size
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Install Python dependencies
COPY ./notification/requirements.txt .
RUN pip install -r requirements.txt

# Copy application files
COPY ./notification /app

CMD ["python3", "main.py"]