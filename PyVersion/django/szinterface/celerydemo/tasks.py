from celery import Celery

app = Celery('tasks', broker='redis://10.0.11.102:6379//')

@app.task
def add(x, y):
    return x + y