using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using Newtonsoft.Json;
using ParsePdfApp.Dtos;
using ParsePdfApp.Models;

namespace ParsePdfApp.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string AccessToken, string RefreshToken) GenerateTokens(User user)
        {
            // Access Token Generation
            string accessToken = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm()) // Encryption algorithm
                .WithSecret(GetAccessSecret())                 // Secret key
                .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(GetAccessTokenTime()).ToUnixTimeSeconds()) // Token lifetime
                .AddClaim("id", user.Id.ToString())        // User Data
                .AddClaim("name", user.Name)
                .AddClaim("email", user.Email)
                .Encode(); // Generate token

            // Refresh Token Generation
            string refreshToken = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(GetRefreshSecret())
                .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(GetRefreshTokenTime()).ToUnixTimeSeconds())
                .AddClaim("id", user.Id.ToString())
                .AddClaim("name", user.Name)
                .AddClaim("email", user.Email)
                .Encode();

            return (accessToken, refreshToken);
        }

        public UserDto ValidateRefresh(string refreshToken)
        {
            try
            {
                string payload = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(GetRefreshSecret())
                    .WithJsonSerializer(new JsonNetSerializer())
                    .WithValidator(new JwtValidator(new JsonNetSerializer(), new UtcDateTimeProvider()))
                    .WithUrlEncoder(new JwtBase64UrlEncoder())
                    .MustVerifySignature()
                    .Decode(refreshToken);

                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(payload);

                return userDto;
            }
            catch (TokenExpiredException)
            {
                throw new Exception("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new Exception("Invalid token signature");
            }
        }

        public UserDto ValidateAccessToken(string accessToken)
        {
            try
            {
                string payload = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(GetAccessSecret())
                    .WithJsonSerializer(new JsonNetSerializer())
                    .WithValidator(new JwtValidator(new JsonNetSerializer(), new UtcDateTimeProvider()))
                    .WithUrlEncoder(new JwtBase64UrlEncoder())
                    .MustVerifySignature()
                    .Decode(accessToken);

                UserDto userDto = JsonConvert.DeserializeObject<UserDto>(payload);

                return userDto;
            }
            catch (TokenExpiredException)
            {
                throw new Exception("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                throw new Exception("Invalid token signature");
            }
        }

        private string GetAccessSecret()
        {
            return _configuration["Jwt:AccessSecret"]!;
        }

        private int GetAccessTokenTime()
        {
            return int.Parse(_configuration["Jwt:AccessTokenTime"]!);
        }

        private string GetRefreshSecret()
        {
            return _configuration["Jwt:RefreshSecret"]!;
        }

        private int GetRefreshTokenTime()
        {
            return int.Parse(_configuration["Jwt:RefreshTokenTime"]!);
        }
    }
}
