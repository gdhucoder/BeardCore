from django.db import models
from django.contrib.auth.models import User
from datetime import datetime
# Create your models here.

JobTypes = [
    (0, "技术类"),
    (1, "产品类"),
    (2, "语言类"),
    (3, "设计类")
]

Cities = [
    (0, "北京"),
    (1, "上海"),
    (2, "深圳")
]

class Job(models.Model):
    job_type = models.SmallIntegerField(blank=False, choices=JobTypes, verbose_name='职位类别')
    job_name = models.CharField(max_length=250, blank=False, verbose_name="职位名称")
    job_city = models.SmallIntegerField(choices=Cities, blank=False, verbose_name="工作地点")
    job_responsibility = models.TextField(max_length=1024, verbose_name="工作职责")
    job_requirement = models.TextField(max_length=1024, verbose_name="工作要求")
    # auth.models 中的 user
    creator = models.ForeignKey(User, verbose_name="创建人", null=True, on_delete=models.SET_NULL)
    # 默认值（当前时间）
    created_date = models.DateTimeField(verbose_name="创建时间", default=datetime.now)
    modified_date = models.DateTimeField(verbose_name="修改时间", default=datetime.now)
