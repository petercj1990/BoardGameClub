namespace BoardGameClubEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PlaySessions")]
    public class PlaySession
    {
        
        [Key]
        public int Id { get; set; }
        public System.DateTime DatePlayed { get; set; }
        public System.TimeSpan PlayTime { get; set; }
        public int BoardGame_Id { get; set; }

        public virtual BoardGame BoardGame { get; set; }
    }
}
