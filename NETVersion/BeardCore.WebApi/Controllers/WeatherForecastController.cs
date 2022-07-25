// Copyright (c) HuGuodong 2022. All Rights Reserved.

using BeardCore.Commons.Log;
using BeardCore.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using BeardCore.Commons.Extensions.Cache;
using OfficeOpenXml;

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

        private readonly IMemoryCache _memoryCache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                
                throw new NotImplementedException("错误");
            }
            catch (NotImplementedException ex)    
            {
                _logger.LogError(ex, "cuowu");
                _logger.LogWarning(ex, "警告1");
                _logger.LogWarning("警告2");
                _logger.LogInformation("消息");
                _logger.LogDebug("调试");
                // Log4netHelper.Error("根据父级功能编码查询所有子集功能，主要用于页面操作按钮权限,代码生成异常", ex);
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

        [HttpGet]
        public async Task<IActionResult> MemoryCache()
        {
            
            var time =  _memoryCache.GetRecord<string>("time");
            if (time == null)
            {
                time = "缓存xxx";
                _memoryCache.SetRecord<string>("time", time, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            }

            return Ok(time);
        }

        public IActionResult GetExcel()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Test");
                worksheet.Cells["A1"].Value = "胡国栋";
                var excelData = package.GetAsByteArray();
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "MyWorkbook.xlsx";
                return File(excelData, contentType, fileName);
            }
        }
    }
}
