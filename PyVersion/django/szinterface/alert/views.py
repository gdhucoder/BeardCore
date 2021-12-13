from django.shortcuts import render
from django.http import HttpResponse
from django.db import connections


# Create your views here.

def dictfetchall(cursor):
    "Return all rows from a cursor as a dict"
    columns = [col[0] for col in cursor.description]
    return [
        dict(zip(columns, row))
        for row in cursor.fetchall()
    ]

def index(request):
    result = ''
    with connections['szcreditmysqldb'].cursor() as c:
        c.execute('''SELECT
            DATE_FORMAT( DATE_SUB( now( ), INTERVAL 6 DAY ), '%m.%d' ) AS 'begin',
            DATE_FORMAT( CURDATE( ), '%m.%d' ) AS 'end' 
        FROM
            DUAL''')
        inverval = dictfetchall(c)[0]
        first_line = '统计区间：{0}-{1}'.format(inverval['begin'], inverval['end'])+'<br>'
        c.execute(
            '''select count(1) as total_times from EnterpriseInfoAlterDB.ENTERPRISE_INFO_QRY_REQ WHERE CREATE_DATE > STR_TO_DATE('2021-01-01','%Y-%m-%d')''')
        summary = dictfetchall(c)
        print(summary)
        result += '，今年累计'+str(summary[0]['total_times'])+'次，其中为'
        c.execute('''SELECT
                    q.CLIENT_ID 'clientID',
                    u.CLIENT_NAME_CN 'clientNameCn',
                    count( 1 ) 'qryTime'
                FROM
                    EnterpriseInfoAlterDB.ENTERPRISE_INFO_QRY_REQ q
                    LEFT JOIN UserDB.USER_USER_ALERT_CLIENT u on u.CLIENT_ID = q.CLIENT_ID
                WHERE
                    q.CREATE_DATE > DATE_SUB( now( ), INTERVAL 7 DAY ) 
                    AND q.CLIENT_ID in ('893afe5a78be4fe28cc69d520e77f7d8', '43b54e65c06849afb33ee00fb7ba87ac')
                GROUP BY
                    q.CLIENT_ID
                ORDER BY count( 1 ) desc''')
        summary = dictfetchall(c)
        sum = 0
        for clt in summary:
            result += clt['clientNameCn']+str(clt['qryTime'])+"次，"
            sum += clt['qryTime']
        head = '上周新版数据接口产品为各合作机构服务{}次'.format(sum)
        result = result[:-1]
        result = first_line + head + result + '。'
    return HttpResponse(result)

def monthly_summary():
    template = '''新版数据接口月度统计（9月），统计区间9.1-28  <br>
                9月新版数据接口产品为各合作机构服务4306次，今年累计5485次，其中深圳农村商业银行服务612次，交通银行深圳分行3694次。'''
    month = ''
    month_first_day = ''
    month_cur_day = ''
    month_qry_times = ''
    total_times = ''
