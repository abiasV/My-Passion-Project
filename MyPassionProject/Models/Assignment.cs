using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }

        public string AssignmentType { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; } // ToDo, Doing, Done

        // an assignment has a project id
        // a project has many assignments
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        // an assignment can be completed by many persons
        public ICollection<Person> Persons { get; set; }

    }


    public class AssignmentDto
    {
        public int AssignmentId { get; set; }

        public string AssignmentType { get; set; }

        public DateTime DueDate { get; set; }

        public string Status { get; set; } // ToDo, Doing, Done

        public int ProjectId { get; set; }

        public string ProjectName { get; set; }
    }
}