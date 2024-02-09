using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class DetailsProject
    {
        public ProjectDto SelectedProject { get; set; }
        public IEnumerable<AssignmentDto> RelatedAssignments { get; set; }
    }
}