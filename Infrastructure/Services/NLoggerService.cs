using Application.Interfaces;
using NLog;

namespace Infrastructure.Services
{
    public class NLoggerService : INLoggerService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        void INLoggerService.Info(string message)
        {
            logger.Info(message);
        }

        void INLoggerService.Warning(string message)
        {
            logger.Warn(message);
        }

        void INLoggerService.Debug(string message)
        {
            logger.Debug(message);
        }

        void INLoggerService.Error(string message)
        {
             logger.Error(message);
        }

        void INLoggerService.Error(Exception ex)
        {
             logger.Error(ex);
        }
    }
}