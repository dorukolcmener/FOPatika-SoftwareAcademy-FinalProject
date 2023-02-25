using JWT;
using JWT.Algorithms;
using JWT.Builder;

namespace ApartmentManagement.AuthorizationOperations;

public class TokenHandler
{
    private readonly string _key;
    private readonly JwtEncoder _encoder;

    public TokenHandler(string key)
    {
        _key = key;
    }

    // Write token encoder method
    public string WriteToken(int userId)
    {
        return new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(_key)
            .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
            .AddClaim("userId", userId)
            .Encode();
    }

    // Read token decoder method throw if expired
    public int ReadToken(string token)
    {
        var sUId = new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(_key)
            .MustVerifySignature()
            .Decode<IDictionary<string, object>>(token)["userId"].ToString();
        return int.Parse(sUId);
    }
}