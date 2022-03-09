using System.ComponentModel.DataAnnotations.Schema;

namespace CarApi.Model.Base
{
    public class BaseEntity
    {
        [Column("id")]
        public long Id { get; set; }
    }
}
