using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            PathFinder fileLogWritter = new PathFinder(new FileLogWritter());
            PathFinder consoleLogWritter = new PathFinder(new ConsoleLogWritter());
            PathFinder secureFileLogWritter = new PathFinder(new SecureFileLogWritter());
            PathFinder secureConsoleLogWritter = new PathFinder(new SecureConsoleLogWritter());
            PathFinder consoleAndSecureFileLogWritter = new PathFinder(new ConsoleAndSecureFileLogWritter());

            consoleLogWritter.Find("It works");
        }
    }

    interface ILogger
    {
        public void WriteError(string message);
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

    class FileLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class SecureFileLogWritter : FileLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    class SecureConsoleLogWritter : ConsoleLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    class ConsoleAndSecureFileLogWritter : SecureFileLogWritter
    {
        public override void WriteError(string message)
        {
            ConsoleLogWritter consoleLogWritter = new ConsoleLogWritter();
            consoleLogWritter.WriteError(message);
            base.WriteError(message);
        }
    }
}
