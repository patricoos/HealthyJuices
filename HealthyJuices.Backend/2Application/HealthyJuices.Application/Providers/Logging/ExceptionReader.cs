using System;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HealthyJuices.Application.Services.Logging
{
    public static class ExceptionReader
    {
        public static string Read(Exception ex)
        {
            try
            {
                return ex switch
                {
                    DbUpdateException dbUpdateException => dbUpdateException.ReadDbException(),
                    _ => ex.ReadException()
                };
            }
            catch
            {
                return ex?.ToString() ?? $"{nameof(ExceptionReader)} could not read given exception";
            }
        }

        private static string ReadDbException(this DbUpdateException @this)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(@this.ReadException());

            foreach (var entry in @this.Entries)
            {
                stringBuilder.AppendLine($"Issue with {entry.Entity} with state {entry.State}");
                stringBuilder.AppendLine("The values: ");

                foreach (var entityValue in entry.Collections)
                    stringBuilder.AppendLine($"{entityValue.Metadata.Name} : {entityValue.CurrentValue}");

                stringBuilder.AppendLine(string.Empty);
            }

            return stringBuilder.ToString();
        }

        private static string ReadException(this Exception exception)
        {
            var text = string.Empty;

            var deepCount = 1;

            var deepException = exception;

            while (deepException.InnerException != null)
            {
                text += $"{Environment.NewLine}{deepCount}) {deepException.Source} - {deepException.Message}";
                deepException = deepException.InnerException;
                deepCount++;
            }

            text += $"{Environment.NewLine}{deepCount}) {deepException.Source} - {deepException.Message}";

            text += $"{Environment.NewLine}" +
                    $"StackTrace: {deepException.StackTrace}";

            return text;
        }
    }
}