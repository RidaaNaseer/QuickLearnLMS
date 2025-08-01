using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickLearnLMS.Data;
using QuickLearnLMS.Models;

namespace QuickLearnLMS.Controllers
{
    public class TeacherController : Controller
    {
        private readonly LMSContext _context;

        public TeacherController(LMSContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            return View(); // Views/Teacher/Dashboard.cshtml
        }
        // View Assigned Courses
        public IActionResult MyCourses()
        {
            int? teacherId = HttpContext.Session.GetInt32("UserID");

            if (teacherId == null)
                return RedirectToAction("Login", "Account");

            var courses = _context.Courses
                .Where(c => c.TeacherID == teacherId)
                .ToList();

            return View(courses);
        }

        [HttpGet]
        public IActionResult UploadMaterial()
        {
            int? teacherId = HttpContext.Session.GetInt32("UserID");
            var courses = _context.Courses.Where(c => c.TeacherID == teacherId).ToList();
            ViewBag.Courses = courses;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMaterial(int courseId, IFormFile uploadedFile)
        {
            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var fileName = Path.GetFileName(uploadedFile.FileName);
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var path = Path.Combine(uploadDir, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }

                var material = new Material
                {
                    CourseID = courseId,
                    FilePath = "/uploads/" + fileName
                };

                _context.Materials.Add(material);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult AddAssignment()
        {
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAssignment(Assignment assignment, IFormFile AssignmentFile)
        {
            if (AssignmentFile != null && AssignmentFile.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assignments");

                // Ensure the folder exists
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = Path.GetFileName(AssignmentFile.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AssignmentFile.CopyToAsync(stream);
                }

                assignment.FilePath = "/assignments/" + fileName;
            }

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }


        [HttpGet]
        public IActionResult CreateQuiz()
        {
            int? teacherId = HttpContext.Session.GetInt32("UserID");
            var courses = _context.Courses.Where(c => c.TeacherID == teacherId).ToList();
            ViewBag.Courses = courses;
            return View();
        }

        [HttpPost]
        public IActionResult CreateQuiz(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [HttpPost]
        public IActionResult CreateQuizWithQuestions(string Title, int CourseID, List<Question> Questions)
        {
            var quiz = new Quiz
            {
                Title = Title,
                CourseID = CourseID
            };

            _context.Quizzes.Add(quiz);
            _context.SaveChanges();

            foreach (var q in Questions)
            {
                q.QuizID = quiz.QuizID;

                // ✅ Get correct option index from form data
                var correctIndexStr = HttpContext.Request.Form[$"Questions[{Questions.IndexOf(q)}].CorrectOptionIndex"];
                int correctIndex = int.TryParse(correctIndexStr, out int ci) ? ci : -1;

                var optionsList = q.Options.ToList();
                for (int i = 0; i < optionsList.Count; i++)
                {
                    optionsList[i].IsCorrect = (i == correctIndex);
                }
                q.Options = optionsList; // update back if needed


                _context.Questions.Add(q);
                _context.Options.AddRange(q.Options);
            }

            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


    }
}