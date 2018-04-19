namespace BoardGameClubEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BoardGames")]
    public class BoardGame
    {
        //public BoardGame()
        //{
        //    this.PlaySessions = new HashSet<PlaySession>();
        //    this.Libraries = new HashSet<Library>();
        //}

        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalPlays { get; set; }
        public int BGGID { get; set; }
        public Nullable<int> MinPlayer { get; set; }
        public Nullable<int> MaxPlayer { get; set; }
        public Nullable<int> PlayingTime { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Nullable<int> Teams { get; set; }
  }
}