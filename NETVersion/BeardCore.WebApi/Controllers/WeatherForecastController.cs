// Copyright (c) HuGuodong 2022. All Rights Reserved.

using BeardCore.Commons.Log;
using BeardCore.Mail;
using Microsoft.AspNetCore.Mvc;

namespace BeardCore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
               throw new NotImplementedException("错误");
            }
            catch (Exception ex)    
            {
                Log4netHelper.Error("根据父级功能编码查询所有子集功能，主要用于页面操作按钮权限,代码生成异常", ex);
            }
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet]
        public async Task<IActionResult> TestSendEmail()
        {
           var res  = await MailHelper.Send();
            Console.WriteLine("hello");
           return Ok(res);
        }
    }
}
