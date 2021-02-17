using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryServices.Person
{
    public interface IPersonRepository
    {
        Task<int> CountByIdAndDocument<T>(int id, string document, int type);
        Task<int> CountByDocument<T>(string document, int type);
        Task<T> FindByDocumentAndPassword<T>(string document, string password, int type);
        Task<ICollection<T>> AllByType<T>(int type);
        Task<ICollection<T>> All<T>(string proc, ICollection<dynamic> parameters = null);
    }
}
