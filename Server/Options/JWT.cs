using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.Options;

public class JWT
{
    public const string Position = "JWTConfiguration";
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int LifetimeHour { get; set; }

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}