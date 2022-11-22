from django.urls import path

from . import views

urlpatterns = [
    # 预警服务周/月度统计
    path('index', views.index, name='index'),
    # 预警服务订阅情况
    path('substatics', views.sub_statics, name='substatics'),
]