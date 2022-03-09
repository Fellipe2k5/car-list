using CarApi.Hypermedia;
using CarApi.Hypermedia.Abstract;
using System;
using System.Collections.Generic;

namespace CarApi.Data.VO
{
    public class CarVO : ISupportsHyperMedia
    {
        public long Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string Plate { get; set; }

        public DateTime DateMake { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();

    }
}