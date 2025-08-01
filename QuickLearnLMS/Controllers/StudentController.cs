using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickLearnLMS.Data;
using QuickLearnLMS.Models;

namespace QuickLearnLMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly LMSContext _context;

        public StudentController(LMSContext context)
        {
            _context = context;
        }
        public IActionResult Dashboard()
        {
            int? studentId = HttpContext.Session.GetInt32("UserID");
            if (studentId == null)
                return RedirectToAction("Login", "Account");

            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.StudentID == studentId)
                .ToList();

            var dashboardItems = enrollments.Select(e =>
            {
                var course = e.Course;
                return new StudentDashboardViewModel
                {
                    Course = course,
                    Assignment = _context.Assignments.FirstOrDefault(a => a.CourseID == course.CourseID),
                    Quiz = _context.Quizzes.FirstOrDefault(q => q.CourseID == course.CourseID)
                };
            }).ToList();

            return View(dashboardItems); // This should be the same view your Razor is using
        }



        // STEP 2: View/Download Materials
        public IActionResult CourseMaterials(int id)
        {
            var materials = _context.Materials.Where(m => m.CourseID == id).ToList();
            return View(materials);
        }

        // STEP 3: Submit Assignment
        [HttpGet]
        public IActionResult SubmitAssignment(int assignmentId)
        {
            ViewBag.AssignmentID = assignmentId;
            return View();
        }

        [HttpPost]
        public IActionResult SubmitAssignment(int assignmentId, IFormFile uploadedFile)
        {
            int? studentId = HttpContext.Session.GetInt32("UserID");
            if (uploadedFile != null && studentId != null)
            {
                var filePath = Path.Combine("wwwroot/uploads", uploadedFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedFile.CopyTo(stream);
                }

                var submission = new Submission
                {
                    AssignmentID = assignmentId,
                    StudentID = studentId.Value,
                    FilePath = "/uploads/" + uploadedFile.FileName
                };

                _context.Submissions.Add(submission);
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpGet]
        public IActionResult AttemptQuiz(int quizId)
        {
            var quiz = _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefault(q => q.QuizID == quizId);

            return View(quiz);
        }


        [HttpPost]
        public IActionResult AttemptQuiz(IFormCollection form)
        {
            if (!int.TryParse(form["quizId"], out int quizId))
                return BadRequest("Invalid quizId");

            int? studentId = HttpContext.Session.GetInt32("UserID");
            if (studentId == null)
                return RedirectToAction("Login", "Account");

            int score = 0;

            var questions = _context.Questions
                .Include(q => q.Options)
                .Where(q => q.QuizID == quizId)
                .ToList();

            foreach (var question in questions)
            {
                string key = $"selectedOption_{question.QuestionID}";

                if (form.ContainsKey(key) && int.TryParse(form[key], out int selectedOptionId))
                {
                    var selectedOption = _context.Options.FirstOrDefault(o => o.OptionID == selectedOptionId);

                    if (selectedOption != null && selectedOption.IsCorrect)
                    {
                        score++;
                    }
                }
            }

            var result = new QuizResult
            {
                QuizID = quizId,
                StudentID = studentId.Value,
                Score = score
            };

            _context.QuizResults.Add(result);
            _context.SaveChanges();

            return RedirectToAction("QuizResult", new { quizId });
        }




        // STEP 5: View Quiz Result
        public IActionResult QuizResult(int quizId)
        {
            int? studentId = HttpContext.Session.GetInt32("UserID");

            var result = _context.QuizResults
      .Where(q => q.QuizID == quizId && q.StudentID == studentId)
      .OrderByDescending(q => q.ResultID) // Get latest attempt
      .FirstOrDefault();


            return View(result);
        }
    }
}

