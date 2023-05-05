﻿using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };

            string jsonproduct = JsonConvert.SerializeObject(product); //Bu şekilde de yapabiliriz

            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct); //Bu şekilde de yapabiliriz

            _distributedCache.Set("product:1", byteproduct);

            //await _distributedCache.SetStringAsync("product:1", jsonproduct, cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");

            string jsonproduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);

            ViewBag.product = p;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");

            return View();
        }
    }
}
