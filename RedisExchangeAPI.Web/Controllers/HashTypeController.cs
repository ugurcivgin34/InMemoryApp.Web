using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web.Controllers
{
    // HashTypeController sınıfı, Redis veritabanındaki hash tipli verileri işleyen bir sınıf
    public class HashTypeController : BaseController
    {
        // Hash anahtarının adını tutan değişken
        public string hashKey { get; set; } = "sozluk";

        // Yapıcı fonksiyon, RedisService nesnesini parametre olarak alır ve BaseController sınıfına gönderir
        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        // Index action'ı, veritabanındaki hash verisini okur ve görüntüler
        public IActionResult Index()
        {
            // İsim ve değer çiftlerini saklamak için bir Dictionary oluşturur
            Dictionary<string, string> list = new Dictionary<string, string>();
            // Eğer anahtar veritabanında mevcutsa
            if (db.KeyExists(hashKey))
            {
                // Hash verisini okur ve string olarak isim ve değer çiftlerini list'e ekler
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }

            // İsim ve değer çiftlerini görünüme gönderir
            return View(list);
        }

        // İsim ve değeri eklemek için kullanılan HTTP POST işlemi
        [HttpPost]
        public IActionResult Add(string name, string val)
        {
            // Hash'e yeni bir isim ve değer ekler
            db.HashSet(hashKey, name, val);

            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }

        // Belirtilen ismi hash'ten silmek için kullanılan işlem
        public IActionResult DeleteItem(string name)
        {
            // Belirtilen ismi hash'ten siler
            db.HashDelete(hashKey, name);
            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }
    }

}
