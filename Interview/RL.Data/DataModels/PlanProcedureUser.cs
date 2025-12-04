using RL.Data.DataModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RL.Data.DataModels
{
    public class PlanProcedureUser : IChangeTrackable
    {
        [Key]
        public int Id { get; set; }

        public int PlanProcedureId { get; set; }
        public virtual PlanProcedure PlanProcedure { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
