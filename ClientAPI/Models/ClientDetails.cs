using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Models
{
    public partial class ClientDetails
    {
        [Key]
        [Column("CID")]
        public long Cid { get; set; }
        [Required]
        [Column("Client_Name")]
        [StringLength(500)]
        public string ClientName { get; set; }
        [Column("PID")]
        public long Pid { get; set; }
        [Required]
        [StringLength(450)]
        public string Id { get; set; }
    }
}
