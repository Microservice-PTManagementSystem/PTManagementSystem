FROM python:3.9

WORKDIR /app

COPY ./appointment/requirements.txt .
RUN pip install -r requirements.txt

COPY ./appointment/app /app

CMD ["python3", "main.py"]
