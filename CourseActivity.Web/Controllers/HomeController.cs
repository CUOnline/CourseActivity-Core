using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CourseActivity.Web.Models;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rss.Providers.Canvas.Helpers;
using CourseActivity.Interfaces.BLL;
using CourseActivity.Models;
using Microsoft.AspNetCore.Http;
using SimpleCsvParser;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;

namespace CourseActivity.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseUploadBLL courseUploadBll;
        private readonly HttpClient canvasClient;
        private readonly CanvasApiAuth canvasApiAuth;

        public HomeController(IHttpClientFactory httpClientFactory, IOptions<CanvasApiAuth> authOptions, ICourseUploadBLL courseUploadBll)
        {
            this.courseUploadBll = courseUploadBll;
            this.canvasClient = httpClientFactory.CreateClient("CanvasClient");
            this.canvasApiAuth = authOptions.Value;
        }

        public async Task<IActionResult> Course(string id)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();
            if (!authenticateResult.Succeeded)
            {
                return RedirectToAction("ExternalLogin", new { courseId = id});
            }
            
            var model = new HomeViewModel();
            if (HttpContext.User.IsInRole(RoleNames.AccountAdmin) || HttpContext.User.IsInRole(RoleNames.Teacher))
            {
                var apiCourse = await canvasClient.GetStringAsync($"/api/v1/courses/{id}");
                var jobj = JObject.Parse(apiCourse);
                model.CourseName = jobj["name"].ToString();
                model.CourseId = id;
                model.Authorized = true;
            }
            else
            {
                // return unauthorized view
                model.Authorized = false;
            }

            return View(model);
        }

        public JsonResult GetCourseData(string courseId)
        {
            var courseUpload = courseUploadBll.GetAll().FirstOrDefault(x => x.CourseId == courseId);

            if (courseUpload != null && !string.IsNullOrWhiteSpace(courseUpload.CSVData))
            {
                var courseData = CsvParser.Parse<CourseCsvEntry>(courseUpload.CSVData);

                for (var i = 0; i < courseData.Count; ++i)
                {
                    if (courseData[i].Category == "wiki")
                    {
                        courseData[i].Category = "pages";
                    }

                    if (courseData[i].Category == "topics")
                    {
                        courseData[i].Category = "discussions";
                    }

                    if (courseData[i].Category == "roster")
                    {
                        courseData[i].Category = "people";
                    }

                    if (courseData[i].Category == "external_tools")
                    {
                        courseData[i].Category = "external Tools";
                    }

                    if (courseData[i].Category == "external_urls")
                    {
                        courseData[i].Category = "external Urls";
                    }
                }

                var model = new List<Category>();

                foreach (var categories in courseData.GroupBy(x => x.Category))
                {
                    var newCategory = new Category();
                    newCategory.Items = new List<Item>();

                    foreach (var items in categories.GroupBy(x => x.Title))
                    {
                        var item = new Item();
                        item.Students = new List<ActivityItem>();

                        foreach (var students in items)
                        {
                            var student = new ActivityItem
                            {
                                Name = students.DisplayName,
                                Views = students.Views,
                                Participations = students.Participations,
                                FirstAccess = students.FirstAccess,
                                LastAccess = students.LastAccess
                            };
                            item.Students.Add(student);
                        }

                        item.Name = items.Key;
                        item.Views = item.Students.Sum(x => x.Views);
                        item.Participations = item.Students.Sum(x => x.Participations);
                        item.FirstAccess = item.Students.OrderBy(x => x.FirstAccess).FirstOrDefault()?.FirstAccess;
                        item.LastAccess = item.Students.OrderByDescending(x => x.LastAccess).FirstOrDefault()?.LastAccess;
                        newCategory.Items.Add(item);
                    }

                    newCategory.Name = categories.Key;
                    newCategory.Views = newCategory.Items.Sum(x => x.Views);
                    newCategory.Participations = newCategory.Items.Sum(x => x.Participations);
                    newCategory.FirstAccess = newCategory.Items.OrderBy(x => x.FirstAccess).FirstOrDefault()?.FirstAccess;
                    newCategory.LastAccess = newCategory.Items.OrderByDescending(x => x.LastAccess).FirstOrDefault()?.LastAccess;
                    model.Add(newCategory);
                }

                return new JsonResult(model);
            }

            return new JsonResult("");
        }

        public IActionResult Upload(CourseReport report)
        {
            var courseUpload = courseUploadBll.GetAll().FirstOrDefault(x => x.CourseId == report.CourseId);
            if (courseUpload != null)
            {
                return RedirectToAction("Course", new { id = courseUpload.CourseId});
            }
            else
            {
                courseUploadBll.Add(new CourseUpload()
                {
                    CourseId = report.CourseId, 
                    CSVData = report.CsvData
                });

                return Ok();
            }
        }

        public IActionResult CheckData(string courseId)
        {
            var courseUpload = courseUploadBll.GetAll().FirstOrDefault(x => x.CourseId == courseId);
            if (courseUpload != null)
            {
                return Json('1');
            }
            else
            {
                return Json('0');
            }
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult DownloadCsv(string courseId)
        {
            var courseUpload = courseUploadBll.GetAll().FirstOrDefault(x => x.CourseId == courseId);

            if (courseUpload != null)
            {
                var csvBytes = Encoding.ASCII.GetBytes(courseUpload.CSVData);
                return File(csvBytes, "text/csv", courseId + ".csv");
            }
            else
            {
                return RedirectToAction("DownloadCsvFailed", new {courseId = courseId});
            }
        }

        public IActionResult DownloadCsvFailed(string courseId)
        {            
            var model = new ReloadDataViewModel()
            {
                CourseId = courseId,
                CanvasUrl = canvasApiAuth.BaseUrl
            };
            return View();
        }

        public IActionResult ReloadData(string courseId)
        {
            var course = courseUploadBll.GetAll().FirstOrDefault(x => x.CourseId == courseId);
            if (course != null)
            {
                courseUploadBll.Delete(course.Id);
            }

            var model = new ReloadDataViewModel()
            {
                CourseId = courseId,
                CanvasUrl = canvasApiAuth.BaseUrl
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region LoginHelper
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string courseId)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult("Canvas", Url.Action("ExternalLoginCallback", "Home", new { courseId = courseId }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string courseId)
        {
            return RedirectToAction("Course", new { id = courseId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLogout(string provider)
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("LoggedOut");
        }

        public ActionResult LoggedOut()
        {
            return View();
        }

        // Used for XSRF protection when adding external logns
        private const string XsrfKey = "XsrfId";

        internal class ChallengeResult : UnauthorizedResult
        {
            private readonly string LoginProvider;
            private readonly string RedirectUri;
            private readonly string UserId;

            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                this.LoginProvider = provider;
                this.RedirectUri = redirectUri;
                this.UserId = userId;
            }

            public override void ExecuteResult(ActionContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Parameters.Add(XsrfKey, UserId);
                }
                context.HttpContext.ChallengeAsync(LoginProvider, properties);
            }
        }

        private async Task<string> GetCurrentUserEmail()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();
            if (authenticateResult != null)
            {
                var emailClaim = authenticateResult.Principal.Claims.Where(cl => cl.Type == ClaimTypes.Email).FirstOrDefault();

                return emailClaim?.Value;
            }

            return string.Empty;
        }
        #endregion
    }
}
