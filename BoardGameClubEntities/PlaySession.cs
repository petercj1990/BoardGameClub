namespace BoardGameClubEntities
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PlaySessions")]
    public class PlaySession
    {
        public PlaySession()
        {

        }
        
        [Key]
        public int Id { get; set; }
        public System.DateTime DatePlayed { get; set; }
        public System.TimeSpan PlayTime { get; set; }
        [Column("BoardGameId")]
        public int BoardGameId { get; set; }
        public bool? Recorded { get; set; }
        public virtual BoardGame BoardGame { get; set; }
    }
}
