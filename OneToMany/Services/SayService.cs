using System;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;
using OneToMany.Models;
using OneToMany.Services.Interface;

namespace OneToMany.Services
{
    public class SayService : ISayService
    {
        private readonly AppDbContext _context;

        public SayService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Say>> GetAllAsync()
        {
            return await _context.Says.Where(m => !m.SoftDeleted).ToListAsync();
        }
    }
}

