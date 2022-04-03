namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stoc")]
    public partial class Stoc
    {
        [Key]
        
        public int IdProdus { get; set; }

        [StringLength(50)]
        public string Produs { get; set; }

        public decimal? Cantitate { get; set; }

        public decimal? Pret { get; set; }
    }
}
