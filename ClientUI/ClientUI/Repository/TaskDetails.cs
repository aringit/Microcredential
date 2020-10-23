using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientUI
{
    public partial class TaskDetails
    {
        [Key]
        [Column("TSKID")]
        public long Tskid { get; set; }
        [Required]
        [StringLength(500)]
        public string TaskDescription { get; set; }
        [Column("CID")]
        public long Cid { get; set; }
        [Column("PID")]
        public long Pid { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Status { get; set; }
    }
}
