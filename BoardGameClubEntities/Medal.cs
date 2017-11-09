namespace BoardGameClubEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Medal")]
    public class Medal
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Player_Id { get; set; }

        public virtual Player Player { get; set; }
    }
}
