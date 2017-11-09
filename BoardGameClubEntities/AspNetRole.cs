namespace BoardGameClubEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AspNetUserRoles")]
    public partial class AspNetRole
    {   
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
