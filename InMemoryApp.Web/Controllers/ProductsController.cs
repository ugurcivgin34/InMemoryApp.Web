using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductsController : Controller
    {

        private IMemoryCache _memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            ////1.yol
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}
            ////2.yol

            //if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            MemoryCacheEntryOptions options = new()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(1),

                SlidingExpiration = TimeSpan.FromSeconds(10)
            };
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);


            return View();
        }

        public IActionResult Show()
        {
            //"zaman" adlı anahtarın var olup olmadığı kontrol edilir. Eğer anahtar önbellekte mevcut değilse, lambda ifadesi kullanılarak yeni bir değer oluşturulur ve önbelleğe eklenir. Lambda ifadesi, şu anki tarih ve saat bilgisini alır ve bir dize olarak biçimlendirir.
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});


            //TryGetValue yöntemi kullanılarak, _memoryCache önbelleği içinde "zaman" anahtarına sahip bir öğe var mı diye kontrol edilir. Bu yöntem, önbellekteki değeri almak için kullanılan bir önbellek erişim yöntemidir. Eğer "zaman" anahtarına sahip bir öğe varsa, değer out parametresi aracılığıyla getirilir ve bu değer daha sonra kullanılmayacak bir _ değişkenine atanır. Bu değişken, _memoryCache'in Get yönteminde kullanılacak yer tutucu bir değişkendir.
            _memoryCache.TryGetValue("zaman", out string _);
            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
