using System;
using OneToMany.Models;

namespace OneToMany.Services.Interface
{
	public interface ISayService
	{
		Task<List<Say>> GetAllAsync();
    }
}

