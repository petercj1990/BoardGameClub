using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameClubEntities
{
    public class Participant
    {
        public Participant()
        {
        }
        [Key]
        public int Id { get; set; }
        public int Player_Id { get; set; }
        public int PlaySession_Id { get; set; }
        public bool? Winner { get; set; }
        public int? Team { get; set; }
    }
}
