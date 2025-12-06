using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePOS.Models; // Ensure this matches your namespace
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePOS.Controllers
{
    [Route("api/[controller]")] // This makes the URL: http://localhost:xxxx/api/orders
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;

        // Constructor: This injects the database connection so we can use it
        public OrdersController(DataContext context)
        {
            _context = context;
        }

        // 1. GET: api/orders/pending
        // The Kitchen uses this to see what to cook
        [HttpGet("pending")]
        public async Task<ActionResult<IEnumerable<orders>>> GetPendingOrders()
        {
            // Fetch only orders where Status is NOT 'completed'
            var pendingOrders = await _context.Orders
                                      .Where(o => o.Status == "pending")
                                      .ToListAsync();
            return Ok(pendingOrders);
        }

        // 2. POST: api/orders
        // The Cashier uses this to submit a new order
        [HttpPost]
        public async Task<ActionResult<orders>> CreateOrder(orders order)
        {
            // Force these default values just in case
            order.Status = "pending"; 
            order.OrderDate = DateTime.Now;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPendingOrders), new { id = order.Id }, order);
        }

        // 3. PUT: api/orders/5/complete
        // The Kitchen uses this to mark order #5 as Done
        [HttpPut("{id}/complete")]
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