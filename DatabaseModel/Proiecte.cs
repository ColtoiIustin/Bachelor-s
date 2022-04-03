namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Proiecte")]
    public partial class Proiecte
    {
        [Key]
        
        public int IdProiect { get; set; }

        [StringLength(50)]
        public string Proiect { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataInceperii { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DataFinalizarii { get; set; }
    }
}
