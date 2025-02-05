using PermissionPOC.attributes;

namespace PermissionPOC.Models;

public class Vehicle
{
    [Permission("viewFloorPrice")] //editFloorPrice
    public string FloorPrice { get; set; }
    
    [Permission("viewCarfaxLink")] 
    public string CarfaxLink { get; set; }
    
    [Permission("viewAdditionalDisclouser")] 
    public string AdditionalDisclouser { get; set; }
}

