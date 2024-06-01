using System;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;

namespace OneToMany.Services.Interface
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;

        public SettingService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Dictionary<string, string>> GetAllAsync()
        {
            return await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
        }
    }
}

