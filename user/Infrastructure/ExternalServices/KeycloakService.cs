using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PTManagementSystem.Application.Interfaces;
using user.Presentation.DTOs;

namespace PTManagementSystem.Infrastructure.ExternalServices
{
    public class KeycloakService : IKeycloakService
    {
        private readonly HttpClient _httpClient;
        private readonly KeycloakSettings _settings;

        public KeycloakService(HttpClient httpClient, IOptions<KeycloakSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        private async Task<string> GetAdminTokenAsync()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "admin-cli"),
                new KeyValuePair<string, string>("username", _settings.AdminUsername),
                new KeyValuePair<string, string>("password", _settings.AdminPassword),
                new KeyValuePair<string, string>("grant_type", "password")
            });

            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/realms/master/protocol/openid-connect/token", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("access_token").GetString();
        }

public async Task<string> GetAccessTokenAsync(string username, string password)
{
    var content = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("client_id", _settings.ClientId),
        new KeyValuePair<string, string>("client_secret", _settings.ClientSecret),
        new KeyValuePair<string, string>("username", username),
        new KeyValuePair<string, string>("password", password),
        new KeyValuePair<string, string>("grant_type", "password")
    });

    var response = await _httpClient.PostAsync(
        $"{_settings.BaseUrl}/realms/{_settings.Realm}/protocol/openid-connect/token", content);

    var json = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var error = doc.RootElement.TryGetProperty("error", out var errProp) ? errProp.GetString() : "unknown_error";
            var description = doc.RootElement.TryGetProperty("error_description", out var descProp) ? descProp.GetString() : "no description";

            throw new ApplicationException($"Token not received: {error} - Description: {description}");
        }
        catch (JsonException)
        {
            
            throw new ApplicationException($"Token not received. Status: {response.StatusCode} - Content: {json}");
        }
    }

    using var successDoc = JsonDocument.Parse(json);
    return successDoc.RootElement.GetProperty("access_token").GetString();
}

        public async Task<bool> CreateUserAsync(string username, string email, string password)
        {
            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var userPayload = new
            {
                username,
                email,
                enabled = true,
                credentials = new[]
                {
                    new
                    {
                        type = "password",
                        value = password,
                        temporary = false
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(userPayload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users", content);
            if (!response.IsSuccessStatusCode)
{
    var errorContent = await response.Content.ReadAsStringAsync();
    throw new ApplicationException($"Keycloak user creation error: {response.StatusCode} - {errorContent}");
}

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(string userId, Dictionary<string, object> attributes)
        {
            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var content = new StringContent(JsonSerializer.Serialize(attributes), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.DeleteAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AssignRoleAsync(string userId, string role)
        {
            var roleId = await GetRoleIdByNameAsync(role);
            if (string.IsNullOrEmpty(roleId)) return false;

            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var rolePayload = new[]
            {
                new { id = roleId, name = role }
            };

            var content = new StringContent(JsonSerializer.Serialize(rolePayload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}/role-mappings/realm", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRoleAsync(string userId, string role)
        {
            var roleId = await GetRoleIdByNameAsync(role);
            if (string.IsNullOrEmpty(roleId)) return false;

            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var rolePayload = new[]
            {
        new { id = roleId, name = role }
    };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}/role-mappings/realm"),
                Content = new StringContent(JsonSerializer.Serialize(rolePayload), Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}/role-mappings/realm");

            if (!response.IsSuccessStatusCode) return Enumerable.Empty<string>();

            var json = await response.Content.ReadAsStringAsync();
            var roles = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
            return roles.Select(r => r["name"].ToString());
        }
     public async Task<IEnumerable<string>> GetUserRolesIncludingGroupsAsync(string userId)
{
    var adminToken = await GetAdminTokenAsync();
    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

    var userRoles = new List<string>();

    // Kullanıcının direkt atanmış realm rollerini alma
    var userRolesResponse = await _httpClient.GetAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}/role-mappings/realm");
    if (userRolesResponse.IsSuccessStatusCode)
    {
        var userRolesJson = await userRolesResponse.Content.ReadAsStringAsync();
        var userRolesList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(userRolesJson);
        if (userRolesList != null)
        {
            userRoles.AddRange(userRolesList.Select(r => r["name"].ToString()));
        }
    }

    //  Kullanıcının gruplarını alma
    var groupsResponse = await _httpClient.GetAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}/groups");
    if (groupsResponse.IsSuccessStatusCode)
    {
        var groupsJson = await groupsResponse.Content.ReadAsStringAsync();
        var groups = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(groupsJson);

        if (groups != null)
        {
            foreach (var group in groups)
            {
                if (group.TryGetValue("id", out var groupIdObj))
                {
                    var groupId = groupIdObj.ToString();

                    //  Grup rollerini alma
                    var groupRolesResponse = await _httpClient.GetAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/groups/{groupId}/role-mappings/realm");
                    if (groupRolesResponse.IsSuccessStatusCode)
                    {
                        var groupRolesJson = await groupRolesResponse.Content.ReadAsStringAsync();
                        var groupRolesList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(groupRolesJson);
                        if (groupRolesList != null)
                        {
                            userRoles.AddRange(groupRolesList.Select(r => r["name"].ToString()));
                        }
                    }
                }
            }
        }
    }

    return userRoles.Distinct();
}

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("client_id", _settings.ClientId),
                new KeyValuePair<string, string>("client_secret", _settings.ClientSecret)
            });

            var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/realms/{_settings.Realm}/protocol/openid-connect/token/introspect", content);

            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("active").GetBoolean();
        }

        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users?email={email}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
            return users.FirstOrDefault()?["id"]?.ToString();
        }

        private async Task<string> GetRoleIdByNameAsync(string roleName)
        {
            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/roles/{roleName}");

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var role = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            return role["id"].ToString();
        }

        public async Task<KeycloakUserDto> GetUserInfoAsync(string userId)
        {
            var adminToken = await GetAdminTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/admin/realms/{_settings.Realm}/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null; 
            }

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            return new KeycloakUserDto
            {
                Email = user.ContainsKey("email") ? user["email"]?.ToString() : null,
                EmailVerified = user.ContainsKey("emailVerified") && bool.TryParse(user["emailVerified"]?.ToString(), out var verified) && verified
            };
        }

    }
}
