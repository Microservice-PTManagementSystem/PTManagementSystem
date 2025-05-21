FROM python:3.9

WORKDIR /app

COPY ./notification/requirements.txt .
RUN pip install -r requirements.txt

COPY ./notification /app


CMD ["python3", "main.py"]
