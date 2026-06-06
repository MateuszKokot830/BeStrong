using Application.Interfaces.Services;
using NLog;

namespace Infrastructure.Services
{
    public class NLoggerService : INLoggerService
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        void INLoggerService.Info(string? message)
        {
            logger.Info(message ?? string.Empty);
        }

        void INLoggerService.Warning(string? message)
        {
            logger.Warn(message ?? string.Empty);
        }

        void INLoggerService.Debug(string? message)
        {
            logger.Debug(message ?? string.Empty);
        }

        void INLoggerService.Error(string? message)
        {
            logger.Error(message ?? string.Empty);
        }

        void INLoggerService.Error(Exception ex)
        {
            logger.Error(ex);
        }
    }
}