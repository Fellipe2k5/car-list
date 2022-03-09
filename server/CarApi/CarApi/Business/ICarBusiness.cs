using CarApi.Data.VO;
using CarApi.Hypermedia.Utils;
using System.Collections.Generic;

namespace CarApi.Business
{
    public interface ICarBusiness
    {
        CarVO Create(CarVO book);
        CarVO FindByID(long id);
        List<CarVO> FindAll();
        PagedSearchVO<CarVO> FindWithPagedSearch(
            string make, string sortDirection, int pageSize, int page);
        CarVO Update(CarVO book);
        void Delete(long id);
    }
}
