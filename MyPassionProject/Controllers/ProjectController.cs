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

namespace MyPassionProject.Controllers
{
    public class ProjectController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProjectController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // GET: Project/List
        public ActionResult List()
        {
            //objective: communicate with our projects data api to retrieve a list of projects
            //curl https://localhost:44346/api/ProjectData/ListProjects

            string url = "ProjectData/ListProjects";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ProjectDto> Projects = response.Content.ReadAsAsync<IEnumerable<ProjectDto>>().Result;

            return View(Projects);
        }

        // GET: Project/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our projects data api to retrieve one project
            //curl https://localhost:44346/api/ProjectData/FindProject/{id}

            DetailsProject ViewModel = new DetailsProject();

            string url = "ProjectData/FindProject/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            ProjectDto SelectedProject = response.Content.ReadAsAsync<ProjectDto>().Result;
            Debug.WriteLine("Project received : ");
            Debug.WriteLine(SelectedProject.ProjectName);

            ViewModel.SelectedProject = SelectedProject;

            //showcase information about assignments related to this project
            //send a request to gather information about assignments related to a particular project ID
            url = "AssignmentData/ListAssignmentsForProjects/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AssignmentDto> RelatedAssignments = response.Content.ReadAsAsync<IEnumerable<AssignmentDto>>().Result;

            ViewModel.RelatedAssignments = RelatedAssignments;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Project/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            Debug.WriteLine("the json payload is :");
            //objective: add a new project into our system using the API
            //curl -H "Content-Type:application/json" -d @Project.json https://localhost:44346/api/ProjectData/AddProject 
            string url = "ProjectData/AddProject";


            string jsonpayload = jss.Serialize(project);
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

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "ProjectData/FindProject/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProjectDto SelectedProject = response.Content.ReadAsAsync<ProjectDto>().Result;
            
            return View(SelectedProject);
        }

        // POST: Project/Update/5
        [HttpPost]
        public ActionResult Update(int id, Project project)
        {

            string url = "ProjectData/UpdateProject/" + id;
            string jsonpayload = jss.Serialize(project);
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

        // GET: Project/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ProjectData/FindProject/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProjectDto SelectedProject = response.Content.ReadAsAsync<ProjectDto>().Result;
            return View(SelectedProject);
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ProjectData/DeleteProject/" + id;
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