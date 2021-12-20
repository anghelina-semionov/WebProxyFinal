using MenuAPI.Models;
using System.Net.Http;

namespace MenuAPI.Services
{
    public interface ISyncService<T> where T : MongoDocument
    {
        HttpResponseMessage Upsert(T record);


        HttpResponseMessage Delete(T record);


    }
}
