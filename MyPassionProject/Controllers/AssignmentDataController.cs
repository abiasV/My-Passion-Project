using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyPassionProject.Models;
using MyPassionProject.Migrations;
using System.Net.NetworkInformation;
//using System.Diagnostics;
//using System.Web.Mvc;

namespace MyPassionProject.Controllers
{
    public class AssignmentDataController : ApiController
    {

        // curl -H "Cookie: .AspNet.ApplicationCookie=9UigFSYGK-uUnyus4L4pws6aJQnAvDoSBk8j2xpqqiByMTLwHNdkQS6CWUQ-TGGwyurwdz7rdF7h0M0RnjdsM_VXR3c4skh7wKLP2F-x5NRAdEUW3PCMtlazwI89COzcIq_oC-wwiYPu4TNFfB-u2KtM2HZpZmp_oRxIFRgmU9li1LY_oM_2aV6jOn1EKtKJvQEIUoqHutj6Vv4sUI8fcgo2SWA3mfRDC1v6dtkbWX4IKO1iyVjSYkn1LUlV9yUC_jtENIXcLWKxRomQ1cWcn1m8NH7t9HdOBBKBGASsl5ik-vwtoW0j452aVSJLWlj_kqdbpEn-63oF3vRFrv2-FGS5Za2_fLYkW_BhHqLRa9ecSAgvc6jUhRtyCJSYK0-5X8VsGNd9p6lzekkSiDcgRBUlN0XSEsiuCuxJKtX1c74f72Pn_6-Z6thsTvHLIVHM3CVG3RRvDua_oHAazixrHGBoRcM1z1yXpYXP_ypJ3g8" https://localhost:44346/api/AssignmentData/ListAssignments

        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all assignments in the system.
        /// </summary>
        // GET: api/AssignmentData/ListAssignments
        // C:\Users\...\repos\MyPassionProject\MyPassionProject\jsondata>curl https://localhost:44346/api/AssignmentData/ListAssignments
        [HttpGet]
        [ResponseType(typeof(AssignmentDto))]
        
        public IHttpActionResult ListAssignments()
        {
            List<Assignment> Assignments = db.Assignments.ToList();
            List<AssignmentDto> AssignmentDtos = new List<AssignmentDto>();

            Assignments.ForEach(a => AssignmentDtos.Add(new AssignmentDto()
            {
                AssignmentId = a.AssignmentId,
                AssignmentType = a.AssignmentType,
                DueDate = a.DueDate,
                Status = a.Status,
                ProjectName = a.Project.ProjectName,
                ProjectId = a.Project.ProjectId
            }));

            return Ok(AssignmentDtos);
        }

        // <param name="id">The primary key of the assignment</param>
        // GET: api/AssignmentData/FindAssignment/4
        // C:\Users\...\repos\MyPassionProject\MyPassionProject\jsondata>curl https://localhost:44346/api/AssignmentData/FindAssignment/6
        [HttpGet]
        [ResponseType(typeof(AssignmentDto))]
        public IHttpActionResult FindAssignment(int id)
        {
            Assignment Assignment = db.Assignments.Find(id);
            AssignmentDto AssignmentDto = new AssignmentDto()
            {
                AssignmentId = Assignment.AssignmentId,
                AssignmentType = Assignment.AssignmentType,
                DueDate = Assignment.DueDate,
                Status = Assignment.Status,
                ProjectName = Assignment.Project.ProjectName,
                ProjectId = Assignment.Project.ProjectId
            };

            if (Assignment == null)
            {
                return NotFound();
            }

            return Ok(AssignmentDto);
        }

        //////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gathers information about all assignments related to a particular project ID
        /// </summary>
        /// <returns>
        /// <param name="id">Project ID.</param>
        /// <example>
        /// GET: api/AssignmentData/ListAssignmentsForProjects/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AssignmentDto))]
        public IHttpActionResult ListAssignmentsForProjects(int id)
        {
            List<Assignment> Assignments = db.Assignments.Where(a => a.ProjectId == id).ToList();
            List<AssignmentDto> AssignmentDtos = new List<AssignmentDto>();

            Assignments.ForEach(a => AssignmentDtos.Add(new AssignmentDto()
            {
                AssignmentId = a.AssignmentId,
                AssignmentType = a.AssignmentType,
                DueDate = a.DueDate,
                Status = a.Status,
                ProjectName = a.Project.ProjectName,
                ProjectId = a.Project.ProjectId
            }));

            return Ok(AssignmentDtos);
        }

        /// <summary>
        /// Gathers information about Assignments related to a particular Person
        /// </summary>
        /// <param name="id">person ID.</param>
        /// <example>
        /// GET: api/AssignmentData/ListAssignmentsForPerson/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AssignmentDto))]
        public IHttpActionResult ListAssignmentsForPerson(int id)
        {
            //all Assignments that have Persons which match with our ID
            List<Assignment> Assignments = db.Assignments.Where(a => a.Persons.Any(p => p.PersonId == id)).ToList();
            List<AssignmentDto> AssignmentDtos = new List<AssignmentDto>();

            Assignments.ForEach(a => AssignmentDtos.Add(new AssignmentDto()
            {
                AssignmentId = a.AssignmentId,
                AssignmentType = a.AssignmentType,
                DueDate = a.DueDate,
                Status = a.Status,
                ProjectName = a.Project.ProjectName,
                ProjectId = a.Project.ProjectId
            }));

            return Ok(AssignmentDtos);
        }


        /// <summary>
        /// Assign a particular person with a particular assignment
        /// </summary>
        /// <param name="assignmentid">The assignment Id primary key</param>
        /// <param name="personid">The person Id primary key</param>
        /// <example>
        /// POST api/AssignmentData/AssignAssignmentWithPerson/6/1
        /// </example>
        [HttpPost]
        [Route("api/AssignmentData/AssignAssignmentWithPerson/{assignmentid}/{personid}")]
         
        public IHttpActionResult AssignAssignmentWithPerson(int assignmentid, int personid)
        {

            Assignment SelectedAssignment = db.Assignments.Include(a => a.Persons).Where(a => a.AssignmentId == assignmentid).FirstOrDefault();
            Person SelectedPerson = db.Persons.Find(personid);

            if (SelectedAssignment == null || SelectedPerson == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input assignment id is: " + assignmentid);
            Debug.WriteLine("selected assignment type is: " + SelectedAssignment.AssignmentType);
            Debug.WriteLine("input person id is: " + personid);
            Debug.WriteLine("selected Person name is: " + SelectedPerson.PersonFirstName);


            SelectedAssignment.Persons.Add(SelectedPerson);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Removes an assignation between a particular person and a particular assignment
        /// </summary>
        /// <param name="assignmentid">The assignment Id primary key</param>
        /// <param name="personid">The person Id primary key</param>
        /// <example>
        /// POST api/AssignmentData/UnAssignAssignmentToPerson/9/1
        /// </example>
        [HttpPost]
        [Route("api/AssignmentData/UnAssignAssignmentToPerson/{assignmentid}/{personid}")]
         
        public IHttpActionResult UnAssignAssignmentToPerson(int assignmentid, int personid)
        {

            Assignment SelectedAssignment = db.Assignments.Include(a => a.Persons).Where(a => a.AssignmentId == assignmentid).FirstOrDefault();
            Person SelectedPerson = db.Persons.Find(personid);

            if (SelectedAssignment == null || SelectedPerson == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input assignment id is: " + personid);
            Debug.WriteLine("selected assignment type is: " + SelectedAssignment.AssignmentType);
            Debug.WriteLine("input person id is: " + assignmentid);
            Debug.WriteLine("selected Person name is: " + SelectedPerson.PersonFirstName);


            SelectedAssignment.Persons.Remove(SelectedPerson);
            db.SaveChanges();

            return Ok();
        }

        //////////////////////////////////////////////////////////////////////////////////////


        // <param name="id">Represents the assignment ID primary key</param>
        // <param name="assignment">JSON FORM DATA of an assignment</param>
        // POST: api/AssignmentData/UpdateAssignment/5
        // C:\jsondata>curl -d @Assignment.json -H "Content-type:application/json" https://localhost:44346/api/AssignmentData/updateAssignment/8
        [ResponseType(typeof(void))]
        [HttpPost]
         
        public IHttpActionResult UpdateAssignment(int id, Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assignment.AssignmentId)
            {
                return BadRequest();
            }

            db.Entry(assignment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }            

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an assignment to the system
        /// </summary>
        /// <param name="assignment">JSON FORM DATA of an assignment</param>
        /// <returns>
        // POST: api/AssignmentData/AddAssignment
        // C:\Users\...\repos\MyPassionProject\MyPassionProject\jsondata>curl -d @Assignment.json -H "Content-type:application/json" https://localhost:44346/api/AssignmentData/AddAssignment
        [ResponseType(typeof(Assignment))]
        [HttpPost]
         
        public IHttpActionResult AddAssignment(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Assignments.Add(assignment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = assignment.AssignmentId }, assignment);
            //return Ok();
        }

        /// <summary>
        /// Deletes an assignment from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the assignment</param>
        /// <returns>
        // POST: api/AssignmentData/DeleteAssignment/5
        // C:\Users\...\repos\MyPassionProject\MyPassionProject\jsondata>curl -d "" https://localhost:44346/api/AssignmentData/deleteassignment/9
        [ResponseType(typeof(Assignment))]
        [HttpPost]
         
        public IHttpActionResult DeleteAssignment(int id)
        {
            Assignment Assignment = db.Assignments.Find(id);
            if (Assignment == null)
            {
                return NotFound();
            }

            db.Assignments.Remove(Assignment);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssignmentExists(int id)
        {
            return db.Assignments.Count(e => e.AssignmentId == id) > 0;
        }
    }
}