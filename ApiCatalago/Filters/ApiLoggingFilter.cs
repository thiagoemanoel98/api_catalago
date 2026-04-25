using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiCatalago.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Antes do Action
        _logger.LogInformation("######################## OnActionExecuting ########################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Model state {context.ModelState.IsValid}");

        _logger.LogInformation("##################### Fim OnActionExecuting ####################");

        
    }
    
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Após o Action
        _logger.LogInformation("######################## OnActionExecuted ########################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Status code: {context.HttpContext.Response.StatusCode}");

        _logger.LogInformation("##################### Fim OnActionExecuted ####################");
    }

  
}