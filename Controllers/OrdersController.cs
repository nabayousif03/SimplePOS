using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePOS.Models; // Ensure this matches your namespace
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<orders>> CreateOrder(orders order)
        {
            
            order.Status = "pending"; 
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPendingOrders), new { id = order.Id }, order);
        }
        
        public async Task<IActionResult> MarkOrderComplete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = "completed";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}