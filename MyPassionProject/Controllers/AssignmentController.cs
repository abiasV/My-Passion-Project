using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using MyPassionProject.Models;
using MyPassionProject.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Security.Policy;

namespace MyPassionProject.Controllers
{
    public class AssignmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AssignmentController() 
        {
      
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // GET: Assignment/List
        public ActionResult List()
        {
            //objective: communicate with our assignment data api to retrieve a list of assignments
            //curl https://localhost:44346/api/assignmentdata/listassignments

            string url = "assignmentdata/ListAssignments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AssignmentDto> assignments = response.Content.ReadAsAsync<IEnumerable<AssignmentDto>>().Result;

            //Debug.WriteLine("Number of assignments received : ");
            //Debug.WriteLine(assignments.Count());

            return View(assignments);
        }

        // GET: Assignment/Details/5
        public ActionResult Details(int id)
        {
            DetailsAssignment ViewModel = new DetailsAssignment();
            //objective: communicate with our assignment data api to retrieve one assignment
            //curl https://localhost:44346/api/assignmentdata/findassignment/{id}

            string url = "assignmentdata/findassignment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine(response.StatusCode);

            AssignmentDto SelectedAssignment = response.Content.ReadAsAsync<AssignmentDto>().Result;

            Debug.WriteLine("assignment received : ");
            Debug.WriteLine(SelectedAssignment.AssignmentType);
                       

            ViewModel.SelectedAssignment = SelectedAssignment;

            //show assigned Persons with this assignment
            string eurl = "PersonData/ListPersonsForAssignment/" + id;
            HttpResponseMessage responseMessage= client.GetAsync(eurl).Result;
            IEnumerable<PersonDto> ResponsiblePersons = responseMessage.Content.ReadAsAsync<IEnumerable<PersonDto>>().Result;

            ViewModel.ResponsiblePersons = ResponsiblePersons;

            string reurl = "PersonData/ListPersonsNotAssignedToAssignment/" + id;
            HttpResponseMessage responseMsg = client.GetAsync(reurl).Result;
            IEnumerable<PersonDto> AvailablePersons = responseMsg.Content.ReadAsAsync<IEnumerable<PersonDto>>().Result;

            ViewModel.AvailablePersons = AvailablePersons;


            return View(ViewModel);
        }

        //POST: Assignment/Assign/{assignmentId}/{personId}
        [HttpPost]
         
        public ActionResult Assign(int id, int personId)
        {
            
            Debug.WriteLine("Attempting to Assign assignment :" + id + " with person " + personId);

            //call our api to Assign assignment with person
            string url = "AssignmentData/AssignAssignmentWithPerson/" + id + "/" + personId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //Get: Assignment/UnAssign/{id}?personId={personId}
        [HttpGet]
         
        public ActionResult UnAssign(int id, int personId)
        {
            
            Debug.WriteLine("Attempting to UnAssign assignment :" + id + " with person: " + personId);

            //call our api to Assigned Assignment with Person
            string url = "AssignmentData/UnAssignAssignmentToPerson/" + id + "/" + personId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Assignment/New
        
        public ActionResult New()
        {

            string url = "ProjectData/ListProjects";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ProjectDto> ProjectOptions = response.Content.ReadAsAsync<IEnumerable<ProjectDto>>().Result;

            return View(ProjectOptions);

        }

        // POST: Assignment/Create
        [HttpPost]
         
        public ActionResult Create(Assignment assignment)
        {
            
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(assignment.AssignmentType);
            //curl -H "Content-Type:application/json" -d @assignment.json https://localhost:44346/api/AssignmentData/AddAssignment 
            string url = "AssignmentData/AddAssignment";


            string jsonpayload = jss.Serialize(assignment);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Assignment/Edit/5
         
        public ActionResult Edit(int id)
        {
            // grab the assignment info

            //objective: communicate with our assignment data api to retrieve one assignment
            //curl https://localhost:44346/api/assignmentdata/findassignment/{id}

            UpdateAssignment ViewModel = new UpdateAssignment();

            string url = "assignmentdata/findassignment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            AssignmentDto SelectedAssignment = response.Content.ReadAsAsync<AssignmentDto>().Result;
            ViewModel.SelectedAssignment = SelectedAssignment;

            // all Project to choose from when updating this assignment
            //the existing assignment information
            url = "ProjectData/ListProjects/";
            response = client.GetAsync(url).Result;
            IEnumerable<ProjectDto> ProjectOptions = response.Content.ReadAsAsync<IEnumerable<ProjectDto>>().Result;

            ViewModel.ProjectOptions = ProjectOptions;

            return View(ViewModel);

        }

        // POST: Assignment/Update/5
        [HttpPost]
         
        public ActionResult Update(int id, Assignment assignment)
        {
            
            string url = "assignmentdata/updateAssignment/" + id;
            string jsonpayload = jss.Serialize(assignment);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Assignment/Delete/5
         
        public ActionResult DeleteConfirm(int id)
        {
            string url = "AssignmentData/FindAssignment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AssignmentDto SelectedAssignment = response.Content.ReadAsAsync<AssignmentDto>().Result;
            return View(SelectedAssignment);
        }

        // POST: Assignment/Delete/5
        [HttpPost]
         
        public ActionResult Delete(int id)
        {
            
            string url = "AssignmentData/DeleteAssignment/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
