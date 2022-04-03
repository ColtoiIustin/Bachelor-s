namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Plantatie")]
    public partial class Plantatie
    {
        [Key]

        public int IdPlanta { get; set; }

        [StringLength(50)]
        public string Planta { get; set; }

        [StringLength(50)]
        public string Soi { get; set; }

        [StringLength(50)]
        public string SuprafataCultivata { get; set; }

        [StringLength(50)]
        public string Recoltare { get; set; }
    }
}
