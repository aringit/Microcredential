using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Models
{
    public partial class ClientPortfolio
    {
        [Key]
        [Column("PID")]
        public long Pid { get; set; }
        [StringLength(500)]
        public string PortfolioName { get; set; }
    }
}
