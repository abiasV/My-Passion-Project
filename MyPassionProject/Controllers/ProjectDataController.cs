using MyPassionProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;


namespace MyPassionProject.Controllers
{

    public class ProjectDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Projects in the system.
        /// </summary>
        /// <example>
        /// GET: api/ProjectData/ListProjects
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProjectDto))]
        public IHttpActionResult ListProjects()
        {
            List<Project> Projects = db.Projects.ToList();
            List<ProjectDto> ProjectDtos = new List<ProjectDto>();

            Projects.ForEach(p => ProjectDtos.Add(new ProjectDto()
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                Description = p.Description,
                
            }));

            return Ok(ProjectDtos);
        }

        /// <summary>
        /// Returns all Projects in the system.
        /// </summary>
        /// <param name="id">The primary key of the Project</param>
        /// <example>
        /// GET: api/ProjectData/FindProject/4
        /// </example>
        [ResponseType(typeof(ProjectDto))]
        [HttpGet]
        public IHttpActionResult FindProject(int id)
        {
            Project Project = db.Projects.Find(id);
            ProjectDto ProjectDto = new ProjectDto()
            {
                ProjectId = Project.ProjectId,
                ProjectName = Project.ProjectName,
                Description = Project.Description,
            };
            if (Project == null)
            {
                return NotFound();
            }

            return Ok(ProjectDto);
        }

        /// <summary>
        /// Updates a particular Project in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Project ID primary key</param>
        /// <param name="project">JSON FORM DATA of an Project</param>
        /// <example>
        /// POST: api/ProjectData/UpdateProject/3
        /// FORM DATA: Project JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {

                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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
        /// Adds a Project to the system
        /// </summary>
        /// <param name="project">JSON FORM DATA of a Project</param>
        /// <example>
        /// POST: api/ProjectData/AddProject
        /// FORM DATA: Project JSON Object
        /// </example>
        [ResponseType(typeof(Project))]
        [HttpPost]
        public IHttpActionResult AddProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectId }, project);
        }

        /// <summary>
        /// Deletes a Project from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Project</param>
        /// <example>
        /// POST: api/ProjectData/DeleteProject/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Project))]
        [HttpPost]
        public IHttpActionResult DeleteProject(int id)
        {
            Project Project = db.Projects.Find(id);
            if (Project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(Project);
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

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectId == id) > 0;
        }
    }
}
