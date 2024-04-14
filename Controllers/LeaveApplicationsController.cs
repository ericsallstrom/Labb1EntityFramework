using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labb1EntityFramework.Data;
using Labb1EntityFramework.Models;
using Labb1EntityFramework.Utility;
using Microsoft.Extensions.Logging;

namespace Labb1EntityFramework.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly LeaveAppDbContext _context;
        private readonly ILogger<LeaveApplicationsController> _logger;

        public LeaveApplicationsController(LeaveAppDbContext context, ILogger<LeaveApplicationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index(string searchInput, DateTime? startDate, DateTime? endDate)
        {
            IQueryable<LeaveApplication> leaveApplicationsQuery = _context.LeaveApplications
                .Include(la => la.Employee)
                .OrderByDescending(la => la.ApplicationDate);

            if (!string.IsNullOrEmpty(searchInput))
            {
                leaveApplicationsQuery = leaveApplicationsQuery.Where(la =>
                la.Employee!.FirstName.ToLower().Contains(searchInput.ToLower()) ||
                la.Employee.LastName.ToLower().Contains(searchInput.ToLower()));
            }

            if (startDate.HasValue)
            {
                leaveApplicationsQuery = leaveApplicationsQuery.Where(la => la.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                leaveApplicationsQuery = leaveApplicationsQuery.Where(la => la.EndDate <= endDate.Value);
            }

            return View(await leaveApplicationsQuery.ToListAsync());
        }

        public async Task<IActionResult> Search(string input, DateTime? startDate, DateTime? endDate)
        {
            if (string.IsNullOrEmpty(input))
            {
                return View();
            }

            IQueryable<Employee> query = _context.Employees
                 .Where(e => e.FullName.ToLower().Contains(input.ToLower()) &&
                    _context.LeaveApplications.Any(la => la.FkEmployeeId == e.Id));

            if (startDate.HasValue || endDate.HasValue)
            {
                query = query.Where(e =>
                    _context.LeaveApplications.Any(la => la.FkEmployeeId == e.Id &&
                    (!startDate.HasValue || la.StartDate >= startDate) &&
                    (!endDate.HasValue || la.EndDate <= endDate))
                );
            }

            var employeesWithLeaveApp = await query.ToListAsync();

            return View(employeesWithLeaveApp);
        }

        public IActionResult Filter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter(DateTime? startDate, DateTime? endDate)
        {
            return RedirectToAction(nameof(Index), new { startDate, endDate });
        }

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            int daysApplied = GetDaysAppliedFor(leaveApplication.Id);

            ViewData["DaysApplied"] = daysApplied;

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {
            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypes"] = Enum.GetValues(typeof(LeaveType))
                                        .Cast<LeaveType>()
                                        .Select(lt => new SelectListItem
                                        {
                                            Text = EnumHelper.GetEnumName(lt),
                                            Value = ((int)lt).ToString()
                                        });

            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationDate,StartDate,EndDate,LeaveType,FkEmployeeId")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.FkEmployeeId);

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.FkEmployeeId);
            ViewData["LeaveTypes"] = Enum.GetValues(typeof(LeaveType))
                                       .Cast<LeaveType>()
                                       .Select(lt => new SelectListItem
                                       {
                                           Text = EnumHelper.GetEnumName(lt),
                                           Value = ((int)lt).ToString()
                                       });

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationDate,StartDate,EndDate,LeaveType,FkEmployeeId")] LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
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

            ViewData["FkEmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.FkEmployeeId);

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);

            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }

        private int GetDaysAppliedFor(int leaveAppId)
        {
            var leaveApplication = _context.LeaveApplications.Find(leaveAppId);

            if (leaveApplication == null)
            {
                return 0;
            }

            TimeSpan totalDays = leaveApplication.EndDate - leaveApplication.StartDate;

            return totalDays.Days + 1; // Include both start and end date
        }
    }
}
