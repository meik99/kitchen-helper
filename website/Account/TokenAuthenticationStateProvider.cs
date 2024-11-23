using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components;

namespace website.Account {
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsIdentity identity = new ClaimsIdentity();
        private Blazored.LocalStorage.ISyncLocalStorageService localStorage;

        public TokenAuthenticationStateProvider(Blazored.LocalStorage.ISyncLocalStorageService localStorage) {
            this.localStorage = localStorage;
        }
        
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = localStorage.GetItemAsString("token");            
            identity = GetIdentityFromToken(token ?? "");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void StateFromResponse(TokenAuthenticationResponse response) {
            var expires = DateTimeOffset.FromUnixTimeSeconds(response.Exp);

            if (expires > DateTime.Now && !string.IsNullOrEmpty(response.Token)) {
                identity = GetIdentityFromToken(response.Token);
            }
        }

        public ClaimsIdentity GetIdentityFromToken(string token) {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            if(tokenHandler.CanReadToken(token)) {
                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                if (jwtToken.ValidTo > DateTime.Now) {
                    identity = new ClaimsIdentity(
                        new List<Claim>(){
                            new Claim(ClaimTypes.Sid, jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id")?.Value ?? ""),
                            new Claim(ClaimTypes.Actor, jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value ?? ""),
                            new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value ?? ""),
                            new Claim(ClaimTypes.Role, "User"),
                        },
                        "Payload JWT"                            
                    );
                    
                    localStorage?.SetItemAsString("token", token);

                    return identity;
                }
            }

            return new ClaimsIdentity();
        }
    }
}