using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components;

namespace website.Account
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ClaimsIdentity identity = new ClaimsIdentity();
        private Blazored.LocalStorage.ISyncLocalStorageService localStorage;
        private TokenAuthenticationService tokenAuthenticationService;

        public TokenAuthenticationStateProvider(
            Blazored.LocalStorage.ISyncLocalStorageService localStorage,
            TokenAuthenticationService tokenAuthenticationService)
        {
            this.localStorage = localStorage;
            this.tokenAuthenticationService = tokenAuthenticationService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = localStorage.GetItemAsString("token");
            identity = GetIdentityFromToken(token ?? "");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private async Task RefreshToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            JwtSecurityToken? jwtToken = ReadTokenAsJwt(token);

            if (jwtToken != null)
            {
                if (jwtToken.ValidTo > DateTime.Now &&
                                jwtToken.ValidTo.AddMinutes(5) <= DateTime.Now)
                {
                    var authResponse = await tokenAuthenticationService.Refresh(token);

                    if (authResponse != null)
                    {
                        StateFromResponse(authResponse);                        
                        await Task.Delay(jwtToken.ValidTo.AddMinutes(-5) - DateTime.Now);
                        await RefreshToken(localStorage.GetItemAsString("token"));
                    }
                }
            }

            localStorage.RemoveItem("token");
        }

        public async void StateFromResponse(TokenAuthenticationResponse response)
        {
            var expires = DateTimeOffset.FromUnixTimeSeconds(response.Exp);

            if (expires > DateTime.Now && !string.IsNullOrEmpty(response.Token))
            {
                identity = GetIdentityFromToken(response.Token);
                await RefreshToken(response.Token);
            } else {
                localStorage.RemoveItem("token");
            }
            
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public ClaimsIdentity GetIdentityFromToken(string token)
        {
            JwtSecurityToken jwtToken = ReadTokenAsJwt(token);

            if (jwtToken != null && jwtToken.ValidTo > DateTime.Now)
            {
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


            return new ClaimsIdentity();
        }

        private JwtSecurityToken? ReadTokenAsJwt(string? token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.CanReadToken(token))
            {
                return tokenHandler.ReadJwtToken(token);
            }

            return null;
        }
    }
}