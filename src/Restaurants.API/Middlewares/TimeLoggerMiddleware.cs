
namespace Restaurants.API.Middlewares
{
    public class TimeLoggerMiddleware(ILogger<TimeLoggerMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await next.Invoke(context);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            if(elapsedMs > 4000)
            {
                logger.LogWarning("Execution Long time request Verb : {verb}, Path : {path}, took {elapsedMs} ms", 
                    context.Request.Method,
                    context.Request.Path,
                    elapsedMs);
            }
        }
    }
}
