using Microsoft.Extensions.Logging;
using Serilog;

namespace Faraboom.Framework.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddFilelog(this ILoggingBuilder builder)
        {
            return builder.AddSerilog();
        }
    }
}
