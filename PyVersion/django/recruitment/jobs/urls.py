from django.urls import re_path
from jobs import views

urlpatterns = [
    # 新的api 使用re_path
    re_path(r'^joblist/', views.joblist, name='joblist'),
]

