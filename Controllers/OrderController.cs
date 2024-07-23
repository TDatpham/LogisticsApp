using LogisticsApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsApp.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{

    public readonly IOrderService _OrderService; 
    public OrderController()
    {
        
    }

    [HttpPost("/create-order")]
    public async Task<IActionResult> CreateOrderTask()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.ToString()); 
        }
    }
}
