using Evolution.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Evolution.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }
       
        public void LogInformation(string infoCode, string message, params object[] args)
        {
            _logger.LogInformation(string.Format("{0}${1}${2}",infoCode, message, JsonConvert.SerializeObject(args)));
        }

        public void LogWarning(string warningCode, string message, params object[] args)
        {
            _logger.LogWarning(string.Format("{0}${1}${2}", warningCode, message, JsonConvert.SerializeObject(args)));
        }

        public void LogError(string errorCode, string message, params object[] args)
        {
            _logger.LogError(string.Format("{0}${1}${2}", errorCode, message, JsonConvert.SerializeObject(args)));
        }

        public void LogCritical(string errorCode, string message, params object[] args)
        {
            _logger.LogCritical(string.Format("{0}${1}${2}", errorCode, message, JsonConvert.SerializeObject(args)));
        }

        public void LogTrace(string errorCode, string message, params object[] args)
        {
            _logger.LogTrace(string.Format("{0}${1}${2}", errorCode, message, JsonConvert.SerializeObject(args)));
        }

        public void LogDebug(string errorCode, string message, params object[] args)
        {
            _logger.LogDebug(string.Format("{0}${1}${2}", errorCode, message, JsonConvert.SerializeObject(args)));
        }
    }
}
