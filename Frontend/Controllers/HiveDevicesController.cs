using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Honeytor.Data;
using Honeytor.Models;
using Honeytor.Services;

using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace Honeytor.Controllers
{
    public class HiveDevicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DeviceApiService _api;

        public HiveDevicesController(ApplicationDbContext context, DeviceApiService api)
        {
            _context = context;
            _api = api;
        }

        // GET: HiveDevices
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.HiveDevice.ToListAsync());
        }

        // GET: HiveDevices/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hiveDevice = await _context.HiveDevice
                .FirstOrDefaultAsync(m => m.HiveDeviceId == id);
            if (hiveDevice == null)
            {
                return NotFound();
            }

            var from = hiveDevice.TimeFrom;

            var pin = hiveDevice.PIN ?? "";
            var DeviceID = hiveDevice.DeviceReference;

            var entries = await _api.GetEntriesAsync(DeviceID, from, pin);

            Console.WriteLine(JsonSerializer.Serialize(entries));

            return View(new HiveDetails { HiveDevice = hiveDevice, Entries = entries});
        }

        // GET: HiveDevices/Create
        [Authorize(Roles = "Admin,Mederator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HiveDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mederator")]
        public async Task<IActionResult> Create([Bind("HiveDeviceId,DeviceReference,PIN,TimeFrom")] HiveDevice hiveDevice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hiveDevice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hiveDevice);
        }

        // GET: HiveDevices/Edit/5
        [Authorize(Roles = "Admin,Mederator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hiveDevice = await _context.HiveDevice.FindAsync(id);
            if (hiveDevice == null)
            {
                return NotFound();
            }
            return View(hiveDevice);
        }

        // POST: HiveDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mederator")]
        public async Task<IActionResult> Edit(int id, [Bind("HiveDeviceId,DeviceReference,PIN,TimeFrom")] HiveDevice hiveDevice)
        {
            if (id != hiveDevice.HiveDeviceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hiveDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HiveDeviceExists(hiveDevice.HiveDeviceId))
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
            return View(hiveDevice);
        }

        // GET: HiveDevices/Delete/5
        [Authorize(Roles = "Admin,Mederator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hiveDevice = await _context.HiveDevice
                .FirstOrDefaultAsync(m => m.HiveDeviceId == id);
            if (hiveDevice == null)
            {
                return NotFound();
            }

            return View(hiveDevice);
        }

        // POST: HiveDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Mederator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hiveDevice = await _context.HiveDevice.FindAsync(id);
            if (hiveDevice != null)
            {
                _context.HiveDevice.Remove(hiveDevice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HiveDeviceExists(int id)
        {
            return _context.HiveDevice.Any(e => e.HiveDeviceId == id);
        }
    }
}
