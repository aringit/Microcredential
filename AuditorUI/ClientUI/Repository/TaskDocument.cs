using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientUI.Repository
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
        public string UplodedBy { get; set; }
        public byte[] FileData { get; set; }
        public string FileType { get; set; }
        public string Filestream { get; set; }
    }
}
