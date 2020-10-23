using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Models
{
    public partial class DocumentUpload
    {
        [Key]
        [Column("DOCUID")]
        public long Docuid { get; set; }
        [Required]
        [StringLength(500)]
        public string DocumentName { get; set; }
        [StringLength(500)]
        public string UploadLocation { get; set; }
        [Column("CID")]
        public long? Cid { get; set; }
    }
}
