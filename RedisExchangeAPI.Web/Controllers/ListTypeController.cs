using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    // ListTypeController sınıfı, Redis veritabanındaki liste tipli verileri işleyen bir sınıf
    public class ListTypeController : Controller
    {
        // RedisService nesnesi
        private readonly RedisService _redisService;

        // Redis veritabanı nesnesi
        private readonly IDatabase db;

        // Liste anahtarının adını tutan değişken
        private string listKey = "names";

        // Yapıcı fonksiyon, RedisService nesnesini parametre olarak alır
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            // Veritabanı nesnesini RedisService üzerinden alır
            db = _redisService.GetDb(1);
        }

        // Index action'ı, veritabanındaki listeyi okur ve görüntüler
        public IActionResult Index()
        {
            // İsimlerin listesini saklamak için bir liste oluşturur
            List<string> namesList = new List<string>();
            // Eğer anahtar veritabanında mevcutsa
            if (db.KeyExists(listKey))
            {
                // Listeyi okur ve string olarak isimleri namesList'e ekler
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            // İsimler listesini görünüme gönderir
            return View(namesList);
        }

        // İsim eklemek için kullanılan HTTP POST işlemi
        [HttpPost]
        public IActionResult Add(string name)
        {
            // Listeye yeni bir isim ekler
            db.ListLeftPush(listKey, name);

            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }

        // Belirtilen ismi listeden silmek için kullanılan işlem
        public IActionResult DeleteItem(string name)
        {
            // Belirtilen ismi listeden async olarak siler
            db.ListRemoveAsync(listKey, name).Wait();

            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }

        // Listenin ilk öğesini silmek için kullanılan işlem
        public IActionResult DeleteFirstItem()
        {
            // Listenin ilk öğesini siler
            db.ListLeftPop(listKey);
            // Index action'ına yönlendirir
            return RedirectToAction("Index");
        }
    }

}
