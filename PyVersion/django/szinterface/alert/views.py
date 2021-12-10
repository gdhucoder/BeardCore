from django.shortcuts import render
from django.http import  HttpResponse
from django.db import connections
# Create your views here.

def index(request):
    with connections['szcreditmysqldb'].cursor() as c:
        c.execute("SELECT ID FROM `ENTERPRISE_INFO_QRY_REQ` limit 1")
        print(c.fetchall())
    return HttpResponse("你好！")