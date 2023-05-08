using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    // SetTypeController sınıfı, Redis veritabanındaki küme (set) tipli verileri işleyen bir sınıf
    public class SetTypeController : Controller
    {
        // RedisService nesnesi
        private readonly RedisService _redisService;

        // Redis veritabanı nesnesi
        private readonly IDatabase db;

        // Küme anahtarının adını tutan değişken
        private string listKey = "hashnames";

        // Yapıcı fonksiyon, RedisService nesnesini parametre olarak alır
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            // Veritabanı nesnesini RedisService üzerinden alır
            db = _redisService.GetDb(2);
        }

        // Index action'ı, veritabanındaki küme (set) verisini okur ve görüntüler
        public IActionResult Index()
        {
            // İsimlerin kümesini saklamak için bir HashSet oluşturur
            HashSet<string> namesList = new HashSet<string>();
            // Eğer anahtar veritabanında mevcutsa
            if (db.KeyExists(listKey))
            {
                // Küme (set) verisini okur ve string olarak isimleri namesList'e ekler
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            // İsimler kümesini görünüme gönderir
            return View(namesList);
        }

        // İsim eklemek için kullanılan HTTP POST işlemi
        [HttpPost]
        public IActionResult Add(string name)
        {
            // Küme (set) anahtarının süresini 5 dakika olarak belirler
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            // Kümeye yeni bir isim ekler
            db.SetAdd(listKey, name);

            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }

        // Belirtilen ismi kümelerden silmek için kullanılan işlem
        public async Task<IActionResult> DeleteItem(string name)
        {
            // Belirtilen ismi kümeden async olarak siler
            await db.SetRemoveAsync(listKey, name);

            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }
    }

}
