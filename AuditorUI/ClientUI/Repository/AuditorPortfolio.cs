using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientUI.Repository
{
    public class AuditorPortfolio
    {
        [Key]
        [Column("PID")]
        public long Pid { get; set; }
        [StringLength(500)]
        public string PortfolioName { get; set; }
    }
}