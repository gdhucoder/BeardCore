// Copyright (c) HuGuodong 2022. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using log4net.Repository;
using System.Diagnostics;

namespace BeardCore.Commons.Log
{
    
    public class Log4netHelper
    {
        private static ILoggerRepository _repository;
        private static readonly ConcurrentDictionary<string, ILog> Loggers = new ConcurrentDictionary<string, ILog>();

        /// <summary>
        /// 读取配置文件，使其生效。如果未找到配置文件，抛出异常。
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="configFilePath"></param>
        /// <exception cref="Exception"></exception>
        public static void SetConfig(ILoggerRepository repository, string configFilePath)
        {
            _repository = repository;
            var fileInfo = new FileInfo(configFilePath);
            if (!fileInfo.Exists)
            {
                throw new Exception("未找到配置文件" + configFilePath);
            }
            XmlConfigurator.ConfigureAndWatch(_repository, fileInfo);
        }

        private static ILog GetLogger(string source)
        {
            if(Loggers.ContainsKey(source))
            {
                return Loggers[source]; 
            }
            else
            {
                var logger = LogManager.GetLogger(_repository.Name, source);
                Loggers.TryAdd(source, logger);
                return logger;
            }
        }

        #region Log a message object

        /// <summary>
        /// 调试信息日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Debug(string msg)
        {
            ILog logger = GetLogger("Debug");
            if (logger.IsDebugEnabled)
            {
                var stackTrace = new StackTrace();
                var stackFrame = stackTrace.GetFrame(1);
                var methodBase = stackFrame.GetMethod();
                var message = "方法名称：" + methodBase.Name + "\r\n日志内容：" + msg;
                logger.Info(message);
            }
        }
        /// <summary>
        /// 错误信息日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Error(string msg)
        {
            ILog logger = GetLogger("Error");
            if (logger.IsErrorEnabled)
            {
                var stackTrace = new StackTrace();
                var stackFrame = stackTrace.GetFrame(1);
                var methodBase = stackFrame.GetMethod();
                var message = "方法名称：" + methodBase.Name + "\r\n日志内容：" + msg;
                logger.Info(message);
            }
        }
        /// <summary>
        /// 异常错误信息日志
        /// </summary>
        /// <param name="throwMsg">异常抛出信息</param>
        /// <param name="ex">异常信息</param>
        public static void Error(string throwMsg, Exception ex)
        {
            ILog logger = GetLogger("Error");
            if (logger.IsErrorEnabled)
            {

                var message =
                    $"抛出信息：{throwMsg} \r\n异常类型：{ex.GetType().Name} \r\n异常信息：{ex.Message} \r\n堆栈调用：\r\n{ex.StackTrace}";
                logger.Error(message);
            }
        }
        /// <summary>
        /// 异常错误信息
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="throwMsg">异常抛出信息</param>
        /// <param name="ex">异常信息</param>
        public static void Error(Type source, object throwMsg, Exception ex)
        {
            ILog logger = GetLogger("Error");
            if (logger.IsErrorEnabled)
            {
                var message =
                    $"抛出信息：{throwMsg} \r\n异常类型：{ex.GetType().Name} \r\n异常信息：{ex.Message} \r\n【堆栈调用】：\r\n{ex.StackTrace}";
                logger.Error(message);
            }
        }
        /// <summary>
        /// 关键信息日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Info(string msg)
        {
            ILog logger = GetLogger("Info");
            if (logger.IsInfoEnabled)
            {
                var stackTrace = new StackTrace();
                var stackFrame = stackTrace.GetFrame(1);
                var methodBase = stackFrame.GetMethod();
                var message = "方法名称：" + methodBase.Name + "\r\n日志内容：" + msg;
                logger.Info(message);
            }
        }

        /// <summary>
        /// 警告信息日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Warn(string msg)
        {
            ILog logger = GetLogger("Warn");
            if (logger.IsWarnEnabled)
            {
                var stackTrace = new StackTrace();
                var stackFrame = stackTrace.GetFrame(1);
                var methodBase = stackFrame.GetMethod();
                var message = "方法名称：" + methodBase.Name + "\r\n日志内容：" + msg;
                logger.Info(message);
            }
        }
        /// <summary>
        /// 失败信息日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Fatal(string msg)
        {
            ILog logger = GetLogger("Fatal");
            if (logger.IsFatalEnabled)
            {
                var stackTrace = new StackTrace();
                var stackFrame = stackTrace.GetFrame(1);
                var methodBase = stackFrame.GetMethod();
                var message = "方法名称：" + methodBase.Name + "\r\n日志内容：" + msg;
                logger.Info(message);
            }
        }
        #endregion


        /// <summary>
        /// 关键信息日志
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="msg">日志信息</param>
        public static void Info(string path, string msg)
        {
            ILog logger = GetLogger("Info");
            if (logger.IsInfoEnabled)
            {
                var stackTrace = new StackTrace();
                var stackFrame = stackTrace.GetFrame(1);
                var methodBase = stackFrame.GetMethod();
                var message = "方法名称：" + methodBase.Name + "\r\n日志内容：" + msg;
                logger.Info(message);
            }
        }
    }
}
