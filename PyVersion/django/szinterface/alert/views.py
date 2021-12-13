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

def sub_statics(request):
    result = '预警服务银行机构订阅情况：<br>'
    # 银行订阅企业情况
    sub_stat_sql = '''SELECT
            u.CLIENT_NAME AS 'client_name',
            count( 1 ) AS 'sub_ent_cnt' 
        FROM
            UserDB.`USER_USER_ALERT_SUB` sub
            LEFT JOIN UserDB.USER_USER_ALERT_CLIENT u ON u.CLIENT_ID = sub.CLIENT_ID 
        WHERE
            sub.CLIENT_ID IN ( '893afe5a78be4fe28cc69d520e77f7d8', '43b54e65c06849afb33ee00fb7ba87ac' ) 
            and sub.IS_SUB = '1'
        GROUP BY
            sub.CLIENT_ID
        ORDER BY 	u.CLIENT_NAME
        '''
    # 预警信息推送
    alert_push_stat_sql = '''SELECT u.CLIENT_NAME as 'client_name', SUM(PUSH_TARGET_COUNT) as 'alert_times'FROM `ENTERPRISE_INFO_PUSH_SUMMARY` s
                        LEFT JOIN UserDB.USER_USER_ALERT_CLIENT u on u.CLIENT_ID = s.CLIENT_ID
                        where s.CLIENT_ID IN ( '893afe5a78be4fe28cc69d520e77f7d8', '43b54e65c06849afb33ee00fb7ba87ac' ) 
                        GROUP BY s.CLIENT_ID ORDER BY u.CLIENT_NAME'''
    with connections['szcreditmysqldb'].cursor() as c:
        c.execute(sub_stat_sql)
        sub_clt_summary = dictfetchall(c)
        for clt in sub_clt_summary:
            result += '【{}】 订阅企业【 {}】 家 。<br>'.format(clt['client_name'], clt['sub_ent_cnt'])
        result += '为机构提供信用风险预警：<br>'
        c.execute(alert_push_stat_sql)
        alert_push_clt_summary = dictfetchall(c)
        for clt in alert_push_clt_summary:
            result += '【{}】 提供信用风险预警：【{}】次 。<br>'.format(clt['client_name'], clt['alert_times'])
    return HttpResponse(result)
