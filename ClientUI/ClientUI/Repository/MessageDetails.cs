using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientUI
{
    public partial class MessageDetails
    {
        [Key]
        [Column("MID")]
        public long Mid { get; set; }
        [Required]
        public string CommentDetails { get; set; }
        [Column("CID")]
        public long Cid { get; set; }
        [Required]
        [Column("AUID")]
        [StringLength(250)]
        public string Auid { get; set; }
        [Required]
        [StringLength(50)]
        public string CommentedBy { get; set; }
        [Column("TSKID")]
        public long Tskid { get; set; }
    }
}
