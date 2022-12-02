using Manager.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Manager.Infra.Interfaces{
    public interface IBaseRepository<T> where T : Base{
        //task que dizer que o metodo é assíncronos, assim os metodos funcionam em threads separadas aumentando o desempenho, sempre que puder use... 
        //T quer dizer que pode ser qualquer objeto
        Task<T> Create(T obj);

        Task<T> Update(T obj);

        Task Remove(long id);

        Task<T> Get(long id);

        Task<List<T>> Get();

        Task<List<T>> Search();

    }
}