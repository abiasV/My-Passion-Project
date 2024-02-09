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
using System.Runtime.InteropServices;


namespace MyPassionProject.Controllers
{
    public class PersonController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PersonController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346/api/");
        }

        // GET: Person/List
        public ActionResult List()
        {
            //objective: communicate with our person data api to retrieve a list of persons
            //curl https://localhost:44346/api/PersonData/ListPersons


            string url = "PersonData/ListPersons";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PersonDto> Persons = response.Content.ReadAsAsync<IEnumerable<PersonDto>>().Result;

            return View(Persons);
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            DetailsPerson ViewModel = new DetailsPerson();

            //objective: communicate with our person data api to retrieve one Person
            //curl https://localhost:44346/api/PersonData/FindPerson/{id}

            string url = "PersonData/FindPerson/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            PersonDto SelectedPerson = response.Content.ReadAsAsync<PersonDto>().Result;
            Debug.WriteLine("Person received : ");
            Debug.WriteLine(SelectedPerson.PersonFirstName);

            ViewModel.SelectedPerson = SelectedPerson;

            //show all assignments under the control of this person
            url = "AssignmentData/ListAssignmentsForPerson/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AssignmentDto> KeptAssignments = response.Content.ReadAsAsync<IEnumerable<AssignmentDto>>().Result;

            ViewModel.KeptAssignments = KeptAssignments;


            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Person/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Person/Create
        [HttpPost]
        public ActionResult Create(Person person)
        {
            Debug.WriteLine("the json payload is :");
            //objective: add a new person into our system using the API
            //curl -H "Content-Type:application/json" -d @Person.json https://localhost:44346/api/PersonData/AddPerson
            string url = "PersonData/AddPerson";


            string jsonpayload = jss.Serialize(person);
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

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "PersonData/FindPerson/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PersonDto SelectedPerson = response.Content.ReadAsAsync<PersonDto>().Result;
            return View(SelectedPerson);
        }

        // POST: Person/Update/5
        [HttpPost]
        public ActionResult Update(int id, Person person)
        {

            string url = "PersonData/UpdatePerson/" + id;
            string jsonpayload = jss.Serialize(person);
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

        // GET: Person/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "PersonData/FindPerson/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PersonDto SelectedPerson = response.Content.ReadAsAsync<PersonDto>().Result;
            return View(SelectedPerson);
        }

        // POST: Person/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "PersonData/DeletePerson/" + id;
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