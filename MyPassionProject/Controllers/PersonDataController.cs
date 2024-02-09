using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyPassionProject.Models;

namespace MyPassionProject.Controllers
{
    public class PersonDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Persons in the system.
        /// </summary>
        /// CONTENT: all Persons in the database, including their associated projects.
        /// </returns>
        /// <example>
        /// GET: api/PersonData/ListPersons
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PersonDto))]
        public IHttpActionResult ListPersons()
        {
            List<Person> Persons = db.Persons.ToList();
            List<PersonDto> PersonDtos = new List<PersonDto>();

            Persons.ForEach(p => PersonDtos.Add(new PersonDto()
            {
                PersonId = p.PersonId,
                PersonFirstName = p.PersonFirstName,
                PersonLastName = p.PersonLastName
            }));

            return Ok(PersonDtos);
        }

        /// <summary>
        /// Returns all Persons in the system associated with a particular assignment.
        /// </summary>
        /// CONTENT: all Persons in the database completing a particular assignment
        /// </returns>
        /// <param name="id">assignment Primary Key</param>
        /// <example>
        /// GET: api/PersonData/ListPersonsForAssignment/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PersonDto))]
        public IHttpActionResult ListPersonsForAssignment(int id)
        {
            List<Person> Persons = db.Persons.Where(p => p.Assignments.Any(a => a.AssignmentId == id)).ToList();
            List<PersonDto> PersonDtos = new List<PersonDto>();

            Persons.ForEach(p => PersonDtos.Add(new PersonDto()
            {
                PersonId = p.PersonId,
                PersonFirstName = p.PersonFirstName,
                PersonLastName = p.PersonLastName
            }));

            return Ok(PersonDtos);
        }

        /// <summary>
        /// Returns Persons in the system not completing a particular assignment.
        /// </summary>
        /// CONTENT: all Persons in the database not assigned a particular assignment
        /// </returns>
        /// <param name="id">Assignment Primary Key</param>
        /// <example>
        /// GET: api/PersonData/ListPersonsNotAssignedToAssignment/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PersonDto))]
        public IHttpActionResult ListPersonsNotAssignedToAssignment(int id)
        {
            List<Person> Persons = db.Persons.Where(
                p => !p.Assignments.Any(
                    a => a.AssignmentId == id)
                ).ToList();
            List<PersonDto> PersonDtos = new List<PersonDto>();

            Persons.ForEach(p => PersonDtos.Add(new PersonDto()
            {
                PersonId = p.PersonId,
                PersonFirstName = p.PersonFirstName,
                PersonLastName = p.PersonLastName
            }));

            return Ok(PersonDtos);
        }

        /// <summary>
        /// Returns all Persons in the system.
        /// </summary>
        /// <param name="id">The primary key of the Person</param>
        /// <example>
        /// GET: api/PersonData/FindPerson/5
        /// </example>
        [ResponseType(typeof(PersonDto))]
        [HttpGet]
        public IHttpActionResult FindPerson(int id)
        {
            Person Person = db.Persons.Find(id);
            PersonDto PersonDto = new PersonDto()
            {
                PersonId = Person.PersonId,
                PersonFirstName = Person.PersonFirstName,
                PersonLastName = Person.PersonLastName
            };
            if (Person == null)
            {
                return NotFound();
            }

            return Ok(PersonDto);
        }

        /// <summary>
        /// Updates a particular Person in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Person ID primary key</param>
        /// <param name="person">JSON FORM DATA of an Person</param>
        /// <example>
        /// POST: api/PersonData/UpdatePerson/5
        /// FORM DATA: Person JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.PersonId)
            {

                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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
        /// Adds an Person to the system
        /// </summary>
        /// <param name="person">JSON FORM DATA of an person</param>
        /// <example>
        /// POST: api/PersonData/AddPerson
        /// FORM DATA: Person JSON Object
        /// </example>
        [ResponseType(typeof(Person))]
        [HttpPost]
        public IHttpActionResult AddPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Persons.Add(person);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = person.PersonId }, person);
        }

        /// <summary>
        /// Deletes an Person from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the person</param>
        /// <example>
        /// POST: api/PersonData/DeletePerson/5
        /// </example>
        [ResponseType(typeof(Person))]
        [HttpPost]
        public IHttpActionResult DeletePerson(int id)
        {
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.Persons.Remove(person);
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

        private bool PersonExists(int id)
        {
            return db.Persons.Count(e => e.PersonId == id) > 0;
        }
    }
}