using Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using XCourse.Core.Entities;
using XCourse.Core.ViewModels.TeachersViewModels;
using XCourse.Infrastructure.Data;
using XCourse.Services.Interfaces.SubjectServices;

namespace XCourse.Web.Areas.Teachers.Controllers
{
    [Area("Teachers")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;
        //var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        private readonly XCourseContext _context;

        public SubjectController(ISubjectService subjectService, XCourseContext context)
        {
            _subjectService = subjectService;
            _context = context;

        }

        // GET: Teachers/Subject
        public async Task<IActionResult> Index()
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var Teacher = await _subjectService.GetTeacherByUserId(userID);
            var subjects = await _subjectService.GetSubjectsByTeacherId(Teacher.ID);
            return View(subjects);
        }

        // GET: SubjectController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            return View(subject);
        }

        // GET: SubjectController/Create
        public async Task<IActionResult> Create()
        {
            //var viewModel = new SubjectCreateVM
            //{
            //    Topics = await _subjectService.GetDistinctTopicsAsync()
            //};    // need maintenance in service
            var viewModel = new SubjectCreateVM
            {
                Topics = _context.Subjects
                    .Select(s => s.Topic)
                    .Distinct()
                    .Select(t => new SelectListItem { Value = t, Text = t })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: SubjectController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectCreateVM model)
        {

            var existingSubject = await _subjectService.GetSubjectByCriteriaAsync(
                model.Topic, model.Major, model.Year, model.Semester);

            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var teacher = await _subjectService.GetTeacherByUserId(userID);

            if (existingSubject == null)
            {
                // Create a new subject
                var newSubject = new Subject
                {
                    Topic = model.Topic,
                    Major = model.Major,
                    Year = model.Year,
                    Semester = model.Semester
                };

                if (teacher != null)
                {
                    await _subjectService.AddSubjectAsync(newSubject, teacher.ID);
                }

                return RedirectToAction("Details", new { id = newSubject.ID });
            }
            else
            {
                // Check if the teacher already has the subject
                var teacherSubjects = await _subjectService.GetSubjectsByTeacherId(teacher.ID);
                if (teacherSubjects.Any(s => s.ID == existingSubject.ID))
                {
                    ModelState.AddModelError(string.Empty, "You already have this subject assigned.");
                    model.Topics = (await _subjectService.GetAllSubjectsAsync())
                        .Select(s => s.Topic)
                        .Distinct()
                        .Select(t => new SelectListItem { Value = t, Text = t })
                        .ToList();
                    return View(model);
                }
                // Assign the existing subject to the teacher
                await _subjectService.AddSubjectAsync(existingSubject, teacher.ID);
                return RedirectToAction("Details", new { id = existingSubject.ID });
            }
        }



        // GET: SubjectController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var subject = await _subjectService.GetSubjectByIdAsync(id);
            return View(subject);
        }

        // POST: SubjectController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var teacher = await _subjectService.GetTeacherByUserId(userID);

                if (teacher != null)
                {
                    var subject = await _subjectService.GetSubjectByIdAsync(id);
                    if (subject != null && subject.Teachers != null)
                    {
                        var teacherSubject = subject.Teachers.FirstOrDefault(t => t.ID == teacher.ID);
                        if (teacherSubject != null)
                        {
                            subject.Teachers.Remove(teacherSubject);
                            await _subjectService.UpdateSubjectAsync(subject);
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
