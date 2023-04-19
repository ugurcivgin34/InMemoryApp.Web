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

                SlidingExpiration = TimeSpan.FromSeconds(10),

                Priority = CacheItemPriority.High
                //Low: Bu öğe, diğer öğeler kaldırılmadan önce kaldırılabilir.Önbellekte kalması isteğe bağlıdır.
                //Normal: Bu öğe, diğer öğeler kaldırılmadan önce kaldırılabilir, ancak normal öncelikli öğeler, düşük öncelikli öğelerden daha uzun süre kalır.
                //High: Bu öğe, düşük öncelikli ve normal öncelikli öğelerden önce kaldırılmaz. Yüksek öncelikli öğelerin önbellekte kalma süresi normal öncelikli öğelere göre daha uzundur.
                //NotRemovable: Bu öğe, önbellekteki diğer tüm öğeler kaldırıldıktan sonra bile kaldırılamaz. Bu öğeler genellikle önbellekte önemli bilgileri içerir ve kesinlikle kaldırılmamalıdır.
                //Bu nedenle, "Priority = CacheItemPriority.High" ifadesi, bir öğenin yüksek öncelikli olarak belirlendiğini gösterir ve önbellekte diğer düşük ve normal öncelikli öğelerden önce kaldırılmayacak bir şekilde tutulur.
            };

            //Bu fonksyon ile data memory dan silinince neden silindiğinin gibi açıklama yaparak bilgi verme sağlanıyor
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });

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
            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.zaman = zamancache;
            ViewBag.callback = callback;
            return View();
        }
    }
}
