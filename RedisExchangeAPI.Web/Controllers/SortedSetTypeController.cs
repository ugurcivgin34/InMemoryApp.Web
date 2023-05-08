using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    // SortedSetTypeController sınıfı, Redis veritabanındaki sıralı küme (sorted set) tipli verileri işleyen bir sınıf
    public class SortedSetTypeController : Controller
    {
        // RedisService nesnesi
        private readonly RedisService _redisService;

        // Redis veritabanı nesnesi
        private readonly IDatabase db;

        // Sıralı küme anahtarının adını tutan değişken
        private string listKey = "sortedsetnames";

        // Yapıcı fonksiyon, RedisService nesnesini parametre olarak alır
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            // Veritabanı nesnesini RedisService üzerinden alır
            db = _redisService.GetDb(3);
        }

        // Index action'ı, veritabanındaki sıralı küme (sorted set) verisini okur ve görüntüler
        public IActionResult Index()
        {
            // İsimlerin listesini saklamak için bir HashSet oluşturur
            HashSet<string> list = new HashSet<string>();
            // Eğer anahtar veritabanında mevcutsa
            if (db.KeyExists(listKey))
            {
                // Sıralı küme (sorted set) verisini okur ve string olarak isimleri list'e ekler
                db.SortedSetScan(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });

                // Sıralı küme (sorted set) verisini tersten sıralayarak ilk 5 elemanını list'e ekler
                db.SortedSetRangeByRank(listKey, 0, 5, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }

            // İsimler listesini görünüme gönderir
            return View(list);
        }

        // İsim ve skoru eklemek için kullanılan HTTP POST işlemi
        [HttpPost]
        public IActionResult Add(string name, int score)
        {
            // Sıralı kümeye yeni bir isim ve skor ekler
            db.SortedSetAdd(listKey, name, score);
            // Sıralı küme (sorted set) anahtarının süresini 1 dakika olarak belirler
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }

        // Belirtilen ismi sıralı kümelerden silmek için kullanılan işlem
        public IActionResult DeleteItem(string name)
        {
            // Belirtilen ismi sıralı kümeden siler
            db.SortedSetRemove(listKey, name);

            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }
    }

}
