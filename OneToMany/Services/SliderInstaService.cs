using System;
using Microsoft.EntityFrameworkCore;
using OneToMany.Data;
using OneToMany.Models;
using OneToMany.Services.Interface;
namespace OneToMany.Services
{
    public class SliderInstaService : ISliderInstaSerivce
    {

        private readonly AppDbContext _context;

        public SliderInstaService(AppDbContext context )
        {
            _context = context;
        }



        public async Task<List<SliderInsta>> GetAllAsync()
        {
            return await _context.SliderInstas.Where(m => !m.SoftDeleted).ToListAsync();
        }
    }
}

