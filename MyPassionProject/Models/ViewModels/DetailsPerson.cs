using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class DetailsPerson
    {
        public PersonDto SelectedPerson { get; set; }
        public IEnumerable<AssignmentDto> KeptAssignments { get; set; }
    }
}