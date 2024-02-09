using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPassionProject.Models.ViewModels
{
    public class UpdateAssignment
    {
        //This viewmodel is a class which stores information that we need to present to /Assignment/Update/{}

        //the existing Assignment information

        public AssignmentDto SelectedAssignment { get; set; }

        // all species to choose from when updating this animal

        public IEnumerable<ProjectDto> ProjectOptions { get; set; }
    }
}