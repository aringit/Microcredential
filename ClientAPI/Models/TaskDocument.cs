using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Models
{
    public partial class TaskDocument
    {
        [Key]
        [Column("DOCUID")]
        public long Docuid { get; set; }
        [Required]
        [StringLength(500)]
        public string DocumentName { get; set; }
        [Column("CID")]
        public long Cid { get; set; }
        [Column("TSKID")]
        public long Tskid { get; set; }
        [Required]
        [StringLength(250)]
        public string UplodedBy { get; set; }
        [MaxLength(50)]
        public byte[] FileData { get; set; }
        [StringLength(50)]
        public string FileType { get; set; }
        public string Filestream { get; set; }
    }
}
