using Serilog;
using ShoppingCartAPI.Repository.Interface;
using ILogger = Serilog.ILogger;

namespace ShoppingCartAPI.Repository.Repositories
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private readonly ILogger _logger;

        public AppLogger()
        {
            _logger = Log.Logger.ForContext<T>();
        }

        public void LogInformation(string message, params object[] args)
        {
            _logger.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.Warning(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.Error(ex, message, args);
        }
    }
}