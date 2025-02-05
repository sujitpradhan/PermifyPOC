using System.Text;
using Newtonsoft.Json;
using PermissionPOC.attributes;
using PermissionPOC.Models;

namespace PermissionPOC.services;

public interface IAssetService
{
    Task<string> RetrivePermission();
}

public class AssetService(HttpClient httpClient) : IAssetService
{
    
    public async Task<string> RetrivePermission()
    {
        var apiResponse = await MakeApiCallAsync();
        var vehicle = new Vehicle
        {
            FloorPrice = "0.01",
            AdditionalDisclouser = "disclose",
            CarfaxLink = "www.carfaxexp.com",
        };
        ApplyPermissions(vehicle, apiResponse); 
        var jsonResult = JsonConvert.SerializeObject(vehicle, Formatting.Indented);
         return jsonResult;
    }


    private static void ApplyPermissions(Vehicle vehicle, ApiResponse apiResponse) //replace with Object 
    {
        var type = vehicle.GetType();
        
        foreach (var property in type.GetProperties())
        {
            var permissionAttribute = (PermissionAttribute)Attribute.GetCustomAttribute(property, typeof(PermissionAttribute));

            if (permissionAttribute == null) continue;
            foreach (var permissionKey in permissionAttribute.Values)
            {
                if (!apiResponse.Results.TryGetValue(permissionKey, out var apiResult)) continue;
                var permissionValue = apiResult switch
                {
                    "CHECK_RESULT_DENIED" => "ACCESS_DENIED",
                    "CHECK_RESULT_ALLOWED" => (string)property.GetValue(vehicle) ?? (string)property.GetValue(vehicle),
                    _ => "Undefined" 
                };
                property.SetValue(vehicle, permissionValue);
            }
        }


    }

    private async Task<ApiResponse> MakeApiCallAsync()
        {
            // Code for simulate an API call if permify is not configured
            // var response = new ApiResponse
            // {
            //     Results = new Dictionary<string, string>
            //     {
            //         { "viewFloorPrice", "CHECK_RESULT_DECLINE" },
            //         { "viewCarfaxLink", "CHECK_RESULT_ALLOWED" },
            //         { "viewAdditionalDisclouser", "CHECK_RESULT_ALLOWED" }
            //     }
            // };
            // return await Task.FromResult(response);
            
            
            var requestBody = new
            {
                metadata = new
                {
                    snap_token = "",
                    schema_version = "cuhjoh80hv84ejlcnprg",
                    only_permission = true,
                    depth = 20
                },
                entity = new
                {
                    type = "organization",
                    id = "org_1"
                },
                subject = new
                {
                    type = "user",
                    id = "user_2",
                    relation = ""
                }
            };
            
            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );
            
            const string apiUrl = "http://localhost:3476/v1/tenants/t1/permissions/subject-permission";
            var response = await httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode) return null;
            var responseData = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseData);

            return apiResponse;

        }
}

public class ApiResponse
{
    public Dictionary<string, string> Results { get; set; }
}