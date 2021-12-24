from celery import Celery

app = Celery('tasks', backend='redis://10.0.11.102:5002', broker='redis://10.0.11.102:5002')

@app.task
def add(x, y):
    return x + y