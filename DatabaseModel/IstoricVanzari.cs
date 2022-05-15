namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("IstoricVanzari")]

    public partial class IstoricVanzari
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DataVanzare { get; set; }
        public decimal? Pret { get; set; }
    }

}
