using System.Net.Http;
using System.Threading.Tasks;

namespace OdeToFood.Services
{
    public interface IOdeToFoodHttpClient
    {
        Task<HttpClient> GetClient();
    }
}