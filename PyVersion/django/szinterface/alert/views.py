from django.shortcuts import render
from django.http import HttpResponse
from django.db import connections
import requests
import json
import re
# Create your views here.

def dictfetchall(cursor):
    "Return all rows from a cursor as a dict"
    columns = [col[0] for col in cursor.description]
    return [
        dict(zip(columns, row))
        for row in cursor.fetchall()
    ]


def index(request):
    result = weekly_summary() + monthly_summary()
    return HttpResponse(result)


def weekly_summary():
    result = ''
    with connections['szcreditmysqldb'].cursor() as c:
        c.execute('''SELECT
            DATE_FORMAT( DATE_SUB( now( ), INTERVAL 6 DAY ), '%m.%d' ) AS 'begin',
            DATE_FORMAT( CURDATE( ), '%m.%d' ) AS 'end' 
        FROM
            DUAL''')
        inverval = dictfetchall(c)[0]
        first_line = '周统计：<br>统计区间：{0}-{1}'.format(inverval['begin'], inverval['end'])+'<br>'
        c.execute(
            '''select count(1) as total_times from EnterpriseInfoAlterDB.ENTERPRISE_INFO_QRY_REQ WHERE CREATE_DATE > STR_TO_DATE('2021-12-31','%Y-%m-%d')''')
        summary = dictfetchall(c)
        print(summary)
        c.execute(
            '''select count(1) as total_times from EnterpriseInfoAlterDB.ENTERPRISE_INFO_QRY_REQ WHERE CREATE_DATE > STR_TO_DATE('2021-01-01','%Y-%m-%d')''')
        last_year = dictfetchall(c)
        result += '，投产至今累计' + str(last_year[0]['total_times']) + '次'
        result += '，今年累计'+str(summary[0]['total_times'])+'次，其中'
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
        result += "<br><br><br>"
        send_feishu(result.replace('<br>','\n'))
        return result

def monthly_summary():

    template = '''月度统计：<br>新版数据接口月度统计（{0}月），统计区间：1日--{2}日<br>{0}月新版数据接口产品为各合作机构服务{3}次，投产至今{4}次，今年累计{5}次，其中'''
    month_date_sql = '''
                SELECT
                DATE_FORMAT(DATE_ADD( curdate(), INTERVAL - DAY ( curdate() ) + 1 DAY ), '%d') AS 'month_first_day',
                DATE_FORMAT(last_day(curdate()), '%d') as 'month_last_day',
                DATE_FORMAT(curdate(), '%d') as 'month_cur_day',
                DATE_FORMAT(curdate(),'%m') as 'cur_month'
                '''
    yearly_sum_sql = '''
                select count(1) as 'total_times' from EnterpriseInfoAlterDB.ENTERPRISE_INFO_QRY_REQ WHERE CREATE_DATE > STR_TO_DATE('2021-12-31','%Y-%m-%d')
                '''
    total_sum_sql = '''
                    select count(1) as 'total_times' from EnterpriseInfoAlterDB.ENTERPRISE_INFO_QRY_REQ WHERE CREATE_DATE > STR_TO_DATE('2021-12-31','%Y-%m-%d')
                    '''
    month_stat_sql = '''
                    SELECT
                    q.CLIENT_ID 'clientID',
                    u.CLIENT_NAME_CN 'clientNameCn',
                    count( 1 ) 'qryTime' 
                FROM
                    EnterpriseInfoAlterDB.ENTERPRISE_INFO_QRY_REQ q
                    LEFT JOIN UserDB.USER_USER_ALERT_CLIENT u ON u.CLIENT_ID = q.CLIENT_ID 
                WHERE
                    q.CREATE_DATE >= DATE_ADD( curdate( ), INTERVAL - DAY ( curdate( ) ) + 1 DAY ) 
                    AND q.CLIENT_ID IN ( '893afe5a78be4fe28cc69d520e77f7d8', '43b54e65c06849afb33ee00fb7ba87ac' ) 
                GROUP BY
                    q.CLIENT_ID 
                ORDER BY
                    count( 1 ) DESC'''
    with connections['szcreditmysqldb'].cursor() as c:
        c.execute(month_date_sql)
        month_date = dictfetchall(c)[0]
        month_first_day = month_date['month_first_day']
        month_last_day = month_date['month_last_day']
        month_cur_day = month_date['month_cur_day']
        cur_month = month_date['cur_month']

        c.execute(yearly_sum_sql)
        this_year_total_times = dictfetchall(c)[0]['total_times']
        c.execute(total_sum_sql)
        total_times = dictfetchall(c)[0]['total_times']

        c.execute(month_stat_sql)
        month_stat = dictfetchall(c)
        month_total_times = 0
        for clt in month_stat:
            template += clt['clientNameCn'] + str(clt['qryTime']) + "次，"
            month_total_times += clt['qryTime']
        template = template[:-1] + "。"
        template = template.format(cur_month, month_first_day, month_cur_day, month_total_times, total_times, this_year_total_times)
    feishu_txt = re.sub(r'(<br>)+', '\n', template)
    print(feishu_txt)
    send_feishu(feishu_txt)
    return template

def sub_statics(request):
    result = '上线至今，预警服务银行机构订阅情况：<br>'
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

    credit_report_stat = '信用报告查询情况：<br>'
    credit_report_stat_sql = '''SELECT
                                u.CLIENT_NAME 'client_name',
                              count( 1 ) as 'qry_times'
                            FROM
                                `ENTERPRISE_INFO_QRY_REQ` q 
                            LEFT JOIN UserDB.USER_USER_ALERT_CLIENT u on u.CLIENT_ID = q.CLIENT_ID	
                            WHERE
                                q.QRY_TYPE = 2 
                                AND q.CLIENT_ID IN ( '43b54e65c06849afb33ee00fb7ba87ac', '893afe5a78be4fe28cc69d520e77f7d8' ) 
                            GROUP BY
                                q.CLIENT_ID'''
    with connections['szcreditmysqldb'].cursor() as c:
        c.execute(credit_report_stat_sql)
        credit_report_sum = dictfetchall(c)
        for clt in credit_report_sum:
            credit_report_stat += '【{}】 查询企业公共信用报告【 {}】 次 。<br>'.format(clt['client_name'], clt['qry_times'])
    result += credit_report_stat
    return HttpResponse(result)


def send_feishu(msg):
    url = "https://open.feishu.cn/open-apis/bot/v2/hook/6590bb3f-f3ac-472c-be7d-e5754460f023"
    playload_message = {
        "msg_type" : "text",
         "content" : {
             "text":  msg
         }
    }
    headers = {
        'Content-Type': "application/json"
    }
    response = requests.request("POST", url, headers= headers, data=json.dumps(playload_message))
    print(response.text)