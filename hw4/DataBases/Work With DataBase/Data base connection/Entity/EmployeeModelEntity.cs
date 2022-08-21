using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractControlCentre.DB.Entities
{
    public class EmployeeEntity<TUniqueId>
    {
        [Key]
        [Column("Id")]
        public TUniqueId Id { get; set; }

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }
    }


    [Table("Employee", Schema = "Work")]
    public sealed class EmployeeModelEntity : EmployeeEntity<int>
    {
        [Column("First Name")]
        public string FirstName { get; set; }

        [Column("Last Name")]
        public string LastName { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Company")]
        public string Company { get; set; }

        [Column("Age")]
        public int Age { get; set; }
    }
}

