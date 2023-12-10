using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserApiService.Services;

namespace UserApiService.Controllers
{
[Route("api/authentication")]
[AllowAnonymous]
[EnableCors]
public class ADAuthenticationController : ControllerBase
    {

        private readonly AzureAuthenticationService _authService;

        public ADAuthenticationController(AzureAuthenticationService authService)
        {
            _authService = authService;
        }

        // GET api/authentication/receivetoken
        [HttpGet]
        [Route("receivetoken")]
        public async Task<IActionResult> GetToken([FromQuery] string code)
        {
            Console.WriteLine("Authorization Code Received: " + code);
            var tokenResult = await _authService.GetTokenFromCode(code);
            Console.WriteLine("Token Received: " + tokenResult.AccessToken);
            Console.WriteLine("Refresh Token: " + tokenResult.RefreshToken);
            return Ok(tokenResult);
        }

            [HttpPost]
            [Route("refreshtoken")]
            public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
            {
            if (model == null || string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest("Refresh token is required.");
            }
            var tokenResult = await _authService.GetNewToken(model);
            Console.WriteLine("Token Received: " + tokenResult.AccessToken);
            Console.WriteLine("Refresh Token: " + tokenResult.RefreshToken);
            return Ok(tokenResult);
        }

#region Helper Classes
        public class RefreshTokenModel
        {
            [JsonProperty("refreshToken")]
            public string RefreshToken { get; set; }
        }

        public class TokenResponse
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("scope")]
            public string Scope { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

        }

    }
#endregion

}