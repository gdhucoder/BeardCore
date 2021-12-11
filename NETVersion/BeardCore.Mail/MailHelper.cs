// Copyright (c) HuGuodong 2022. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentEmail.MailKitSmtp;
using FluentEmail.Core;
using FluentEmail.Core.Models;

namespace BeardCore.Mail
{
    public static class MailHelper
    {
        public static async Task<SendResponse> Send()
        {
            var opt = new SmtpClientOptions
            {
                Password = "xx",
                User = "guodong_hu@126.com",
                Server = "smtp.126.com",
                Port = 25,
                RequiresAuthentication = true
            };
            Email.DefaultSender = new MailKitSender(opt);   
            var mail = await Email.
                    From("guodong_hu@126.com")
                    .To("guodong_hu@126.com")
                    .Subject("你好")
                    .Body("这是一封测试邮件") 
                    .SendAsync();
            Console.WriteLine(mail);
            return mail;
        }
    }
}
