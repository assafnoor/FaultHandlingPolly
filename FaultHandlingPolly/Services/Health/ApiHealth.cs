using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace FaultHandlingPolly.Services.Health
{
    public class ApiHealth : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            var url = "https://matchilling-chuck-norris-jokes-v1.p.rapidapi.com/jokes/random";
            var clinet = new RestClient();
            var request = new RestRequest(url, Method.Get);
            request.AddHeader("accept", "application/json");
            request.AddHeader("X-RapidAPI-Key", "SIGN-UP-FOR-KEY");
            request.AddHeader("X-RapidAPI-Host", "matchilling-chuck-norris-jokes-v1.p.rapidapi.com");

            var response =  clinet.Execute(request);
            if (response.IsSuccessful)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy());
            }
        }
    }
}
