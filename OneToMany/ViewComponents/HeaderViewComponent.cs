using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OneToMany.Services.Interface;
using OneToMany.ViewModels;
using OneToMany.ViewModels.Baskets;

namespace OneToMany.ViewComponents
{
	public class HeaderViewComponent : ViewComponent
	{
        private readonly ISettingService _settingService;
        private readonly IHttpContextAccessor _accessor;

        public HeaderViewComponent(ISettingService settingService,
                                   IHttpContextAccessor accessor)
        {
            _settingService = settingService;
            _accessor = accessor;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string,string> settingDatas = await _settingService.GetAllAsync();

            List<BasketVM> basketProduct = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketProduct = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }


            HeaderVM response = new()
            {
                Setting=settingDatas,
                BasketCount= basketProduct.Sum(m=>m.Count),
                BasketTotalPrice=basketProduct.Sum(m=>m.Count * m.Price)
            };

            return await Task.FromResult(View(response));
        }
    }
}

