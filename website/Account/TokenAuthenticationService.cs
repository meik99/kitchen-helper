using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace website.Account {
    public class TokenAuthenticationService
    {
        private readonly HttpClient httpClient;

        public TokenAuthenticationService(HttpClient httpClient) {
            this.httpClient = httpClient;
        }

        public async Task<TokenAuthenticationResponse?> Login(TokenAuthenticationRequest request) {
            try {
                var result = await httpClient.PostAsJsonAsync("api/users/login", request);

                if (result.IsSuccessStatusCode) {
                    using var contentStream = await result.Content.ReadAsStreamAsync();
                    var response = await JsonSerializer.DeserializeAsync<TokenAuthenticationResponse>(contentStream);

                    return response;
                }

            } catch(Exception e) {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}