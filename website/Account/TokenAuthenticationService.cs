using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace website.Account
{
    public class TokenAuthenticationService
    {
        private readonly HttpClient httpClient;

        public TokenAuthenticationService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<TokenAuthenticationResponse?> Login(TokenAuthenticationRequest request)
        {
            try
            {
                var result = await httpClient.PostAsJsonAsync("api/users/login", request);

                if (result.IsSuccessStatusCode)
                {
                    using var contentStream = await result.Content.ReadAsStreamAsync();
                    var response = await JsonSerializer.DeserializeAsync<TokenAuthenticationResponse>(contentStream);

                    return response;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        internal async Task<TokenAuthenticationResponse?> Refresh(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,"api/users/refresh-token");

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using var response = await httpClient.SendAsync(request);
                                
                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<TokenAuthenticationResponse>(contentStream);

                    return result;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}