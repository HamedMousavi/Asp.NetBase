using System;

namespace Web.Api.Logging
{
    public interface ILogger : IDisposable
    {
        void Error(Exception exception, string message);
        //void Error(string message);
        //void Debug(string message);
        void Trace(string message);
    }
}
