using Elyssa.Core.Common;
using Elyssa.Core.Interfaces;
using Elyssa.PublicApi.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Elyssa.PublicApi.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ValidateCompanyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("companyId", out var companyIdObj) ||
            companyIdObj is not Guid companyId)
        {
            var error = Error.Validation(
                "MISSING_COMPANY_ID",
                "El header x-company-id es requerido",
                "x-company-id");
            context.Result = error.ToApiErrorResponse();
            return;
        }

        if (companyId == Guid.Empty)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<ValidateCompanyAttribute>>();
            logger.LogWarning("Request with empty x-company-id header");

            var error = Error.Validation(
                "INVALID_COMPANY_ID",
                "El header x-company-id no puede estar vacío",
                "x-company-id");
            context.Result = error.ToApiErrorResponse();
            return;
        }

        var companyService = context.HttpContext.RequestServices
            .GetRequiredService<ICompanyService>();
        
        var companyResult = await companyService.GetByIdAsync(
            companyId, 
            context.HttpContext.RequestAborted);

        if (!companyResult.IsSuccess)
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<ValidateCompanyAttribute>>();
            logger.LogWarning("Company {CompanyId} validation failed: {Error}", 
                companyId, companyResult.Error!.Message);

            context.Result = companyResult.Error!.ToApiErrorResponse();
            return;
        }

        await next();
    }
}
