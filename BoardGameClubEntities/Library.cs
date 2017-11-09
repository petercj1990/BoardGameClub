namespace BoardGameClubEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Libraries")]
    public class Library
    {
        //public Library()
        //{
        //    this.AspNetUsers = new HashSet<AspNetUser>();
        //    this.BoardGames = new HashSet<BoardGame>();
        //}
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public Nullable<int> bgId { get; set; }
        public Nullable<Boolean> favorite { get; set; }
        [ForeignKey("userId")]
        [Column("userId")]
        public virtual AspNetUser AspNetUser { get; set; }
        //[ForeignKey("bgId")]
        //public virtual ICollection<BoardGame> BoardGames { get; set; }
    }
}