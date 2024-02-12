using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace real_time_backend
{
    public class LoggingService
    {

        private static readonly Semaphore _semaphore = new(initialCount: 3, maximumCount: 3);
        public void Write(string message)
        {
            var writeOperation = new Thread(new ParameterizedThreadStart(Worker));
            writeOperation.Start(message);
        }

        private static void Worker(object? message)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"logs.txt");
            using (StreamWriter writer = new(filePath, append: true))
            {
                _semaphore.WaitOne();
                writer.WriteLine(message);
            }

            _semaphore.Release();
        }

    }
}