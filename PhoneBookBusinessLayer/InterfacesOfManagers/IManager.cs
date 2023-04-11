using PhoneBookEntityLayer.Mappings.ResultModels;
using System.Linq.Expressions;

namespace PhoneBookBusinessLayer.InterfacesOfManagers
{
    public interface IManager<T, Id>
    {
        IDataResult<T> Add(T model); //ekleme icin IResult degil IDataResult kullandik. Cunku eklenen verinin idsine ihtiyac duyarsak geriye donen datanan idyi alabiliriz
        IResult Update(T model);
        IResult Delete(T model);
        IDataResult<ICollection<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        IDataResult<T> GetByConditions(Expression<Func<T, bool>>? filter = null);
        IDataResult<T> GetById(Id id);
    }
}
