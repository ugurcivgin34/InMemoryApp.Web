using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    // Redis veritabanı ile iletişim kuran bir sınıf
    public class RedisService
    {
        // Redis sunucusunun adresini tutan özel değişken
        private readonly string _redisHost;

        // Redis sunucusunun portunu tutan özel değişken
        private readonly string _redisPort;
        // Redis bağlantı nesnesi
        private ConnectionMultiplexer _redis;

        // Redis veritabanı nesnesi
        public IDatabase db { get; set; }

        // Yapıcı fonksiyon, IConfiguration parametresi ile yapılandırma dosyasından ayarları alır
        public RedisService(IConfiguration configuration)
        {
            // Redis:Host değerini _redisHost değişkenine atar
            _redisHost = configuration["Redis:Host"];

            // Redis:Port değerini _redisPort değişkenine atar
            _redisPort = configuration["Redis:Port"];
        }

        // Redis sunucusuna bağlanan fonksiyon
        public void Connect()
        {
            // Bağlantı ayarlarını oluşturur
            var configString = $"{_redisHost}:{_redisPort}";

            // Bağlantıyı gerçekleştirir
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        // Verilen db numarasına göre Redis veritabanı nesnesi döndüren fonksiyon
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
