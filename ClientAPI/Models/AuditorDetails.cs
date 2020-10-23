using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Models
{
    public partial class AuditorDetails
    {
        [Key]
        [Column("AUID")]
        public long Auid { get; set; }
        [Required]
        [StringLength(500)]
        public string AuditorName { get; set; }
    }
}
