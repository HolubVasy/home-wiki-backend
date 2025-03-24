using System.Text;
using home_wiki_backend.DAL.Exceptions;
using home_wiki_backend.Shared.Enums;

namespace Andersen.Infrastructure.API
{
    /// <summary>
    /// Middleware to handle exceptions and provide a consistent error response.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private const int DefaultErrorCode =
            StatusCodes.Status500InternalServerError;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger to log errors.</param>
        /// <param name="env">The hosting environment.</param>
        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task that represents the completion of request processing.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArticleServiceException ex)
            {
                await HandleExceptionAsync(context, ex,
                    "Article Service Error", ErrorCode.Unexpected);
            }
            catch (CategoryServiceException ex)
            {
                await HandleExceptionAsync(context, ex,
                    "Category Service Error", ErrorCode.Unexpected);
            }
            catch (TagServiceException ex)
            {
                await HandleExceptionAsync(context, ex,
                    "Tag Service Error", ErrorCode.Unexpected);
            }
            catch (GenericRepositoryException ex)
            {
                await HandleExceptionAsync(context, ex,
                    "Repository Error", ErrorCode.DatabaseException);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex,
                    "Unexpected Error", ErrorCode.Unexpected);
            }
        }

        /// <summary>
        /// Handles the exception and writes the error response.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="prefix">The error message prefix.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns>A task that represents the completion of error handling.</returns>
        private async Task HandleExceptionAsync(HttpContext context,
            Exception ex, string prefix, ErrorCode errorCode)
        {
            int statusCode = DefaultErrorCode;
            // For our custom exceptions, we return a 500 error.
            if (ex is ArticleServiceException ||
                ex is CategoryServiceException ||
                ex is TagServiceException ||
                ex is GenericRepositoryException)
            {
                statusCode = StatusCodes.Status500InternalServerError;
            }
            else
            {
                statusCode = DefaultErrorCode;
            }

            // Compose an error message.
            string errorMsg = $"{prefix}: {ex.Message}";
            if (!_env.IsProduction())
            {
                errorMsg += $" StackTrace: {ex.StackTrace}";
            }

            var errorResult = new ErrorResultModel(errorMsg, errorCode);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            _logger.LogError(ex, ComposeErrorMessage(ex, statusCode));
            await context.Response.WriteAsync(errorResult.ToString());
        }

        /// <summary>
        /// Composes the error message for logging.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <returns>The composed error message.</returns>
        private static string ComposeErrorMessage(Exception ex, int statusCode)
        {
            var sb = new StringBuilder();
            sb.Append($"HTTP {statusCode} - Exception: {ex.Message}. ");
            if (ex.InnerException != null)
            {
                sb.Append($"Inner Exception: {ex.InnerException.Message}. ");
            }
            sb.Append($"StackTrace: {ex.StackTrace}");
            return sb.ToString();
        }
    }
}
