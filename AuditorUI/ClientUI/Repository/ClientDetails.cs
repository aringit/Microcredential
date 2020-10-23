using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ClientUI
{
   
    public class ClientDetails
    {
        [Key]
        [Column("CID")]
        public long Cid { get; set; }
        [Required]

        [Column("ClientName")]
        [StringLength(500)]
        public string ClientName { get; set; }

        [Column("PID")]
       public long Pid { get; set; }
    }
}
