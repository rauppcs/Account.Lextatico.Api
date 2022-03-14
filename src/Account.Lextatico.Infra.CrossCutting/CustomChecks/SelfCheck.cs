using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Account.Lextatico.Infra.CrossCutting.CustomChecks
{
    public class SelfCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy, "API up!"));
        }
    }
}
