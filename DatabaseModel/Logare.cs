namespace DatabaseModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Logare")]
       
        public partial class Logare
        {
           [Key]
           public int Id { get; set; }
           [StringLength(50)]
           public string Parola { get; set; }
        }
    
}
