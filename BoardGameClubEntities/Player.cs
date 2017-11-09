namespace BoardGameClubEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Players")]
    public class Player
    {
        public Player()
        {
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Column("AspNetUser_Id")]
        public string AspNetUser_Id { get; set; }
        
        [ForeignKey("AspNetUser_Id")]
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
