using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePOS.Models; // Ensure this matches your namespace
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplePOS.Data;

namespace SimplePOS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;
        
        public OrdersController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<orders>>> GetPendingOrders()
        {
            
            var pendingOrders = await _context.Orders
                                      .Where(o => o.Status == "pending")
                                      .ToListAsync();
            return Ok(pendingOrders);
        }
        
        [HttpPost]
        public async Task<ActionResult<orders>> CreateOrder(OrdersForm order)
        {
            
            order.Status = "pending"; 
            order.DeliveryDate = DateTime.UtcNow;

            var NewOrd = new orders
            {
                Id = Guid.NewGuid(),
                totalPrice = order.totalPrice,
                Status = order.Status,
                DeliveryDate = order.DeliveryDate,
            };

            _context.Orders.Add(NewOrd);
            await _context.SaveChangesAsync();
            return Ok(order);
        }
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkOrderComplete(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (order.Status == "cancelled")
            {
                return BadRequest("Order is already cancelled");
            }

            order.Status = "completed";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Order has been completed",
                status = order.Status
                
            });
        }
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            if (order.Status == "completed")
            {
                return BadRequest("Cannot cancel order");
            }
            order.Status = "cancelled";
            await _context.SaveChangesAsync();
            return Ok("Order has been cancelled");
        }
    }
}