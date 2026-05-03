/* using System.Net.Http.Json;
using Honeytor.Models;

namespace Honeytor.Services
{
    public class DeviceApiService
    {
        private readonly HttpClient _http;

        public DeviceApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Entry>> GetEntriesAsync(int deviceId, DateTime from, string PIN)
        {
            var fromStr = from.ToString("yyyyMMdd");
            var url = $"http://admin.domatom.net/api/hiveapi/files?device={deviceId}&pin={PIN}&from={fromStr}";
            Console.WriteLine(url);

            var response = await _http.GetAsync(url);
            //response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse>();

            return result?.HiveFiles ?? new List<Entry>();
        }
    }
}

*/

using System.Net.Http.Json;
using Honeytor.Data;
using Honeytor.Models;
using Microsoft.EntityFrameworkCore;

namespace Honeytor.Services
{
    public class DeviceApiService
    {
        private readonly HttpClient _http;
        private readonly ApplicationDbContext _context;

        public DeviceApiService(HttpClient http, ApplicationDbContext context)
        {
            _http = http;
            _context = context;
        }

        public async Task<List<Entry>> GetEntriesAsync(int deviceId, DateTime from, string PIN)
        {
            var fromStr = from.ToString("yyyyMMdd");
            var url = $"http://admin.domatom.net/api/hiveapi/files?device={deviceId}&pin={PIN}&from={fromStr}";

            try
            {
                var response = await _http.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                var apiEntries = result?.HiveFiles;

                if (apiEntries.Any())
                {
                    await SaveEntries(apiEntries);
                    return apiEntries;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API ERROR: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return await _context.Entries
                .AsNoTracking()
                .Where(e => e.Id == deviceId)
                .OrderByDescending(e => e.Stored)
                .ToListAsync();
        }

        private async Task SaveEntries(List<Entry> entries)
        {
            foreach (var e in entries)
            {
                var exists = await _context.Entries
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == e.Id);

                if (!exists)
                {
                    _context.Entry(e).State = EntityState.Added;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        _context.Entry(e).State = EntityState.Detached;
                    }
                }
            }
        }
    }
}