using CarApi.Data.Converter.Contract;
using CarApi.Data.VO;
using CarApi.Model;
using System.Collections.Generic;
using System.Linq;

namespace CarApi.Data.Converter.Implementations
{
    public class CarConverter : IParser<CarVO, Car>, IParser<Car, CarVO>
    {
        public Car Parse(CarVO origin)
        {
            if (origin == null) return null;
            return new Car
            {
                Id = origin.Id,
                Make = origin.Make,
                Model = origin.Model,
                Color = origin.Color,
                Plate = origin.Plate,
                DateMake = origin.DateMake
            };
        }

        public CarVO Parse(Car origin)
        {
            if (origin == null) return null;
            return new CarVO
            {
                Id = origin.Id,
                Make = origin.Make,
                Model = origin.Model,
                Color = origin.Color,
                Plate = origin.Plate,
                DateMake = origin.DateMake
            };
        }

        public List<Car> Parse(List<CarVO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<CarVO> Parse(List<Car> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}
