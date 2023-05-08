using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        // RedisService nesnesi
        private readonly RedisService _redisService;

        // Redis veritabanı nesnesi
        private readonly IDatabase db;

        // Yapıcı fonksiyon, RedisService nesnesini parametre olarak alır
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            // Veritabanı nesnesini RedisService üzerinden alır
            db = _redisService.GetDb(0);
        }

        // Index action'ı, veritabanına örnek değerler ekler
        public IActionResult Index()
        {
            // Veritabanına 'name' anahtarlı string değer ekler
            db.StringSet("name", "Fatih Çakıroğlu");
            // Veritabanına 'ziyaretci' anahtarlı integer değer ekler
            db.StringSet("ziyaretci", 100);

            return View();
        }

        // Show action'ı, veritabanındaki değerleri okur ve gösterir
        public IActionResult Show()
        {
            // 'name' anahtarının değerinin uzunluğunu alır
            var value = db.StringLength("name");
            // 'name' anahtarının değerini alır
            var deger = db.StringGet("name");
            // 'ziyaretci' anahtarının değerini 10 artırır
            // db.StringIncrement("ziyaretci", 10);

            // 'ziyaretci' anahtarının değerini

            // 1 azaltır ve işlem sonucunu döndürür
            // var count = db.StringDecrementAsync("ziyaretci", 1).Result;

            // 'ziyaretci' anahtarının değerini 10 azaltır (async olarak)
            db.StringDecrementAsync("ziyaretci", 10).Wait();

            // 'value' değişkenini ViewBag nesnesine ekler
            ViewBag.value = value.ToString();

            // Görünümü döndürür
            return View();
        }
    }
}