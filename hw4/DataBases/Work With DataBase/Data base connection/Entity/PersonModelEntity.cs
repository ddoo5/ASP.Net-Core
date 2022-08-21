using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContractControlCentre.DB.Entities
{
    public class PersonEntity<TUniqueId>
    {
        [Key]
        [Column("Id")]
        public TUniqueId Id { get; set; }

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }
    }

    [Table("Persons", Schema = "Work")]
    public sealed class PersonModelEntity : PersonEntity<int>
    {
        [Column("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Column("Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Column("Company")]
        public string Company { get; set; } = string.Empty;

        [Column("Age")]
        public int Age { get; set; } = 0;
    }
}

