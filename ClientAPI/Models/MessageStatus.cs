using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Models
{
    public partial class MessageStatus
    {
        [Key]
        [Column("MSGID")]
        public long Msgid { get; set; }
        [Column("COMID")]
        public long Comid { get; set; }
        [Column("MessageStatus")]
        [StringLength(50)]
        public string MessageStatus1 { get; set; }
    }
}
