using System;
namespace OneToMany.Services.Interface
{
	public interface ISettingService
	{
		Task<Dictionary<string, string>> GetAllAsync();
	}
}

