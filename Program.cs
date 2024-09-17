using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            PathFinder fileLogWriter = new PathFinder(new FileLogWriter());
            PathFinder consoleLogWriter = new PathFinder(new ConsoleLogWriter());
            PathFinder secureFileLogWriter = new PathFinder(new SecureLogWriter(new FileLogWriter()));
            PathFinder secureConsoleLogWriter = new PathFinder(new SecureLogWriter(new ConsoleLogWriter()));
            PathFinder consoleAndSecureFileLogWriter = new PathFinder(new DoubleLogWriter(new ConsoleLogWriter(), new SecureLogWriter(new FileLogWriter())));

            consoleLogWriter.Find("It works");
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }

    class PathFinder
    {
        private ILogger _logger;

        public PathFinder(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        public void Find(string message)
        {
            _logger.WriteError(message);
        }
    }

    class FileLogWriter : ILogger
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class ConsoleLogWriter : ILogger
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class SecureLogWriter : ILogger
    {
        private ILogger _logger;

        public SecureLogWriter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logger.WriteError(message);
            }
        }
    }

    class DoubleLogWriter : ILogger
    {
        private ILogger _firstLogger;
        private ILogger _secondLogger;

        public DoubleLogWriter(ILogger firstLogger, ILogger secondLogger)
        {
            _firstLogger = firstLogger;
            _secondLogger = secondLogger;
        }

        public void WriteError(string message)
        {
            _firstLogger.WriteError(message);
            _secondLogger.WriteError(message);
        }
    }
}
