using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
  public class Config
  {
    // scopes define the resources in your system
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      return new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Email(),
        new IdentityResources.Profile(),
        new IdentityResource(
            name: "custom.profile",
            displayName: "Custom Profile",
            claimTypes: new[] { JwtClaimTypes.Email })
      };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
      return new List<ApiResource>
      {
        new ApiResource("api", "My API", new[] { JwtClaimTypes.Email }),
        new ApiResource("api_admin", "My API", new[] { JwtClaimTypes.Email, JwtClaimTypes.Role })
      };
    }

    // clients want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients()
    {
      // client credentials client
      return new List<Client>
      {
        // resource owner password grant client
        new Client
        {
          AllowedScopes =
          {
            IdentityServerConstants.StandardScopes.Email,
            "api"
          },

          AlwaysIncludeUserClaimsInIdToken = true,
          AlwaysSendClientClaims = true,

          ClientId = "ro.client",
          AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

          ClientSecrets =
          {
            new Secret("secret".Sha256())
          }
        },

        new Client
        {
          AllowedScopes =
          {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Email,
            IdentityServerConstants.StandardScopes.Profile,
            "api_admin"
          },
          
          AlwaysIncludeUserClaimsInIdToken = true,
          AlwaysSendClientClaims = true,

          ClientId = "ro.client.admin",
          AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

          ClientSecrets =
          {
            new Secret("admin_client_secret".Sha256())
          }
        }
      };

    }
  }
}
