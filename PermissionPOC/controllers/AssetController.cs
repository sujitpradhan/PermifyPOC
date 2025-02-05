using Microsoft.AspNetCore.Mvc;
using PermissionPOC.services;

namespace PermissionPOC.controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetController(IAssetService assetService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        //Accept the OrdId and UserId from request with Organization and User entity
        var message = assetService.RetrivePermission();
        return Ok(message.Result);
    }
    
}