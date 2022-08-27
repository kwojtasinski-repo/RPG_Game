using Microsoft.Extensions.Logging;
using System.Threading;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class LoggerStub<T>
        where T : class
    {
        private static readonly object s_lock = new object();
        private static ILogger<T> _logger = null;

        private LoggerStub() { }

        public static ILogger<T> Create()
        {
            var loggerFactory = new LoggerFactory();
            return _logger = Create(loggerFactory);
        }
        
        public static ILogger<T> Create(ILoggerFactory loggerFactory)
        {
            if (loggerFactory is not null)
            {
                return _logger;
            }

            Monitor.Enter(s_lock);
            var tempLogger = new Logger<T>(loggerFactory);
            Interlocked.Exchange(ref _logger, tempLogger);
            Monitor.Exit(s_lock);

            return _logger;
        }
    }
}
