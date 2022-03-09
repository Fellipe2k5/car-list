using CarApi.Data.Converter.Implementations;
using CarApi.Data.VO;
using CarApi.Hypermedia.Utils;
using CarApi.Model;
using CarApi.Repository;
using System.Collections.Generic;

namespace CarApi.Business.Implementations
{
    public class CarBusinessImplementation : ICarBusiness
    {

        private readonly IRepository<Car> _repository;

        private readonly CarConverter _converter;

        public CarBusinessImplementation(IRepository<Car> repository)
        {
            _repository = repository;
            _converter = new CarConverter();
        }

        // Method responsible for returning all people,
        public List<CarVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PagedSearchVO<CarVO> FindWithPagedSearch(
            string make, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"select * from cars p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(make)) query = query + $" and p.make like '%{make}%' ";
            query += $" order by p.make {sort} limit {size} offset {offset}";

            string countQuery = @"select count(*) from cars p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(make)) countQuery = countQuery + $" and p.make like '%{make}%' ";

            var cars = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<CarVO>
            {
                CurrentPage = page,
                List = _converter.Parse(cars),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        // Method responsible for returning one car by ID
        public CarVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        // Method responsible to crete one new car
        public CarVO Create(CarVO car)
        {
            var carEntity = _converter.Parse(car);
            carEntity = _repository.Create(carEntity);
            return _converter.Parse(carEntity);
        }

        // Method responsible for updating one car
        public CarVO Update(CarVO car)
        {
            var carEntity = _converter.Parse(car);
            carEntity = _repository.Update(carEntity);
            return _converter.Parse(carEntity);
        }

        // Method responsible for deleting a car from an ID
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}

