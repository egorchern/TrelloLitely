using Microsoft.AspNetCore.Authorization;

namespace EzyClassroomz.Api.Classes;

public static class AuthorizationPolicies
{
    public readonly static ICollection<string> Policies = new List<string>
    {
        "readBoard",
        "writeBoard",
        "deleteBoard",
        "readTicket",
        "writeTicket",
        "deleteTicket",
        "viewRestricted"
    };

    public static void InitializePolicies(AuthorizationOptions options)
    {
        foreach (var policyName in Policies)
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.RequireClaim(policyName, "true");
            });
        }
    }
}