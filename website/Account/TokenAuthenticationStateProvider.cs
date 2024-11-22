using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace website.Account {
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsIdentity identity = new ClaimsIdentity();

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void StateFromResponse(TokenAuthenticationResponse response) {
            if (new DateTime(response.Exp) > DateTime.Now && !string.IsNullOrEmpty(response.Token)) {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                if(tokenHandler.CanReadToken(response.Token)) {
                    JwtSecurityToken token = tokenHandler.ReadJwtToken(response.Token);

                    if (token.ValidTo > DateTime.Now) {
                        identity = new ClaimsIdentity(
                            token.Claims,
                            "Token"                            
                        );
                    }                    
                }
                
            }
        }
    }
}