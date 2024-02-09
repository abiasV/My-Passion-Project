using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class DetailsAssignment
    {
        public AssignmentDto SelectedAssignment { get; set; }
        public IEnumerable<PersonDto> ResponsiblePersons { get; set; }
        public IEnumerable<PersonDto> AvailablePersons { get; set; }
    }
}