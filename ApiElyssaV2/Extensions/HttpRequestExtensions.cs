namespace BackOfficeElyssa.Extensions
{
    public static class HttpRequestExtensions
    {
        private const string CompanyIdHeaderName = "x-company-id";

        /// <summary>
        /// Extrae el token Bearer del header Authorization
        /// </summary>
        /// <param name="request">HttpRequest actual</param>
        /// <returns>Token sin el prefijo "Bearer " o null si no existe</returns>
        public static string? GetBearerToken(this HttpRequest request)
        {
            var authHeader = request.Headers.Authorization.ToString();
            
            if (string.IsNullOrWhiteSpace(authHeader))
                return null;

            if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authHeader["Bearer ".Length..].Trim();

            return authHeader.Trim();
        }
        public static Guid? GetCompanyIdFromHeader(this HttpRequest request)
        {
            if (request?.Headers == null)
            {
                return null;
            }

            if (!request.Headers.TryGetValue(CompanyIdHeaderName, out var headerValue))
            {
                return null;
            }

            var companyIdString = headerValue.ToString();

            if (string.IsNullOrWhiteSpace(companyIdString))
            {
                return null;
            }

            if (Guid.TryParse(companyIdString, out var companyId))
            {
                return companyId;
            }

            return null;
        }
    }
}
