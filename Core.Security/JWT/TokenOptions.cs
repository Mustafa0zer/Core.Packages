using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.JWT;

public class TokenOptions
{
    public string Audience { get; set; } // hitap noktası
    public string Issuer { get; set; } // token veren taraf
    public int AccessTokenExpiration { get; set; } // süre
    public string SecurityKey { get; set; } //güvenlik anahtarı
    public int RefreshTokenTTL { get; set; } //TTL değeri for refresh

    public TokenOptions()
    {
        Audience = string.Empty;
        Issuer = string.Empty; 
        SecurityKey = string.Empty;
    }

    public TokenOptions(string audience, string ıssuer, int accessTokenExpiration, string securityKey, int refreshTokenTTL)
    {
        Audience = audience;
        Issuer = ıssuer;
        AccessTokenExpiration = accessTokenExpiration;
        SecurityKey = securityKey;
        RefreshTokenTTL = refreshTokenTTL;
    }
}
