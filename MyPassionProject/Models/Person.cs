using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string PersonFirstName { get; set; }

        public string PersonLastName { get; set; }


        // A person can complete many assignments
        public ICollection<Assignment> Assignments { get; set; }
    }

    public class PersonDto
    {
        public int PersonId { get; set; }

        public string PersonFirstName { get; set; }

        public string PersonLastName { get; set; }
    }
}