using CarApi.Model.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarApi.Model
{
    [Table("cars")]
    public class Car : BaseEntity
    {
        [Column("make")]
        public string Make { get; set; }

        [Column("model")]
        public string Model { get; set; }

        [Column("color")]
        public string Color { get; set; }

        [Column("plate")]
        public string Plate { get; set; }

        [Column("date_make")]
        public DateTime DateMake { get; set; }
    }

}
