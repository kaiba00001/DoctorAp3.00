using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoctorAp.Data;
using DoctorAp.Models;
using Microsoft.AspNetCore.Authorization;

namespace DoctorAp.Controllers

{
    [Authorize]
    public class LeadController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lead


        public async Task<IActionResult> Index()
        {
            return _context.BookingLead != null ?
                        View(await _context.BookingLead.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.BookingLead'  is null.");
        }

        // GET: Lead/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookingLead == null)
            {
                return NotFound();
            }

            var bookingLeadEntity = await _context.BookingLead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingLeadEntity == null)
            {
                return NotFound();
            }

            return View(bookingLeadEntity);
        }

        // GET: Lead/Create
        public IActionResult Create()
        {
            // Retrieve the list of booked times from the database
            var bookedTimes = _context.BookingLead.Select(b => b.Time).ToList();

            // Create a list of all possible times
            var allTimes = new List<string>();

            var startTime = TimeSpan.FromHours(9); // Start time at 9:00 AM
            var endTime = TimeSpan.FromHours(18); // End time at 6:00 PM
            var timeInterval = TimeSpan.FromMinutes(60); // Time interval of 1 hour

            // Generate the available times between the start and end time with the specified interval
            for (var time = startTime; time < endTime; time += timeInterval)
            {
                allTimes.Add(time.ToString(@"hh\:mm") + " " + (time.Hours < 12 ? "AM" : "PM"));
            }

            // Exclude the booked times from the available times
            var availableTimes = allTimes.Except(bookedTimes).ToList();

            // Pass the available times to the view
            ViewBag.AvailableTimes = availableTimes;

            return View();
        }

        // POST: Lead/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,Mobile,Email,Time")] BookingLeadEntity bookingLeadEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookingLeadEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookingLeadEntity);
        }

        // GET: Lead/Edit/5
        // Only users with the "Admin" role can access this action
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookingLead == null)
            {
                return NotFound();
            }

            var bookingLeadEntity = await _context.BookingLead.FindAsync(id);
            if (bookingLeadEntity == null)
            {
                return NotFound();
            }
            return View(bookingLeadEntity);
        }

        // POST: Lead/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,Mobile,Email,Time")] BookingLeadEntity bookingLeadEntity)
        {
            if (id != bookingLeadEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingLeadEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingLeadEntityExists(bookingLeadEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookingLeadEntity);
        }

        // GET: Lead/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookingLead == null)
            {
                return NotFound();
            }

            var bookingLeadEntity = await _context.BookingLead
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookingLeadEntity == null)
            {
                return NotFound();
            }

            return View(bookingLeadEntity);
        }

        // POST: Lead/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookingLead == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BookingLead'  is null.");
            }
            var bookingLeadEntity = await _context.BookingLead.FindAsync(id);
            if (bookingLeadEntity != null)
            {
                _context.BookingLead.Remove(bookingLeadEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingLeadEntityExists(int id)
        {
            return (_context.BookingLead?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
