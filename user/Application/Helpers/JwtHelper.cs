  using System.Text.Json;
namespace PTManagementSystem.Application.Helpers
{
public class JwtHelper
{
    public static Dictionary<string, object> DecodePayload(string token)
    {
        var parts = token.Split('.');
        if (parts.Length < 2)
            throw new ArgumentException("Token formatı geçersiz.");

        var payload = parts[1];

        // Base64 padding düzeltme
        payload = payload.Replace('-', '+').Replace('_', '/');
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }

        var bytes = Convert.FromBase64String(payload);
        var json = System.Text.Encoding.UTF8.GetString(bytes);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<Dictionary<string, object>>(json, options)!;
    }
}

}