using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Models
{
    public partial class AuditorClientComments
    {
        [Key]
        [Column("COMID")]
        public long Comid { get; set; }
        [Required]
        public string Comment { get; set; }
        [Column("CID")]
        public long Cid { get; set; }
        [Column("DOCUID")]
        public long Docuid { get; set; }
        [Column("AUID")]
        public long Auid { get; set; }
        [Column("TSKID")]
        public long Tskid { get; set; }
    }
}
