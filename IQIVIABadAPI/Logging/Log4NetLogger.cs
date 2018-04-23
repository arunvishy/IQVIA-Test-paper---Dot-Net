using System;
using System.IO;
using System.Security.Principal;
using log4net;
using log4net.Config;

namespace IQVIABadAPI.Logging
{
    /// <summary>
    /// Log4net Logger logic
    /// </summary>
    public static class Log4NetLogger
    {

        /// <summary>
        /// Static constructor.
        /// </summary>
        static Log4NetLogger()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(
                                                               AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                               "Log4Net.config")));
        }

        public enum LogType
        {
            General,
            Notify,
            Information
        }

        public static void LogError(string message, Exception error, string logClassName)
        {
            ILog logger = LogManager.GetLogger(logClassName);
            if ((error.InnerException != null))
            {
                error = error.InnerException;
            }
            if (logger.IsErrorEnabled)
            {
                logger.Error(message, error);
            }
        }

        public static void LogError(string message, IPrincipal user, Uri url, Exception error, string logClassName)
        {
            SetOptionalParametersOnLogger(user, url);
            LogError(message, error, logClassName);
        }

        public static void LogInfo(string message, LogType type, string logClassName)
        {
            ILog logger = null;
            if (type == LogType.Notify)
            {
                logger = LogManager.GetLogger(LogType.Notify.ToString());
            }
            else
            {
                logger = LogManager.GetLogger(logClassName);
            }
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }

        public static void LogWarning(string message, Exception error, string logClassName)
        {
            ILog logger = LogManager.GetLogger(logClassName);
            if ((error.InnerException != null))
            {
                error = error.InnerException;
            }
            if (logger.IsWarnEnabled)
            {
                logger.Warn(message, error);
            }
        }

        public static void LogWarning(string message, IPrincipal user, Uri url, Exception error, string logClassName)
        {
            SetOptionalParametersOnLogger(user, url);
            LogWarning(message, error, logClassName);
        }

        private static void SetOptionalParametersOnLogger(IPrincipal user, Uri url)
        {
            //set user to log4net context, so we can use %X{user} in the appenders
            if ((user != null) && user.Identity.IsAuthenticated)
            {
                MDC.Set("user", user.Identity.Name);
            }
            //set url to log4net context, so we can use %X{url} in the appenders
            MDC.Set("url", url.ToString());
        }

        public static void UserTracingLog(string message, LogType type, string logClassName)
        {
            ILog logger = null;
            if (type == LogType.Notify)
            {
                logger = LogManager.GetLogger(LogType.Information.ToString());
            }
            else
            {
                logger = LogManager.GetLogger(logClassName);
            }
            if (logger.IsInfoEnabled)
            {
                logger.Info(message);
            }
        }
    }
}
