using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.PaymentMethod;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using Domain.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    public class UserCourseRepository : BaseRepository<UserCoursesHeader>, IUserCourse
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _AuthService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public UserCourseRepository(ApplicationDbContext context, IAuthService authService, UnitOfWork unitOfWork, IEmailService emailService) : base(context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _AuthService = authService;
            _emailService = emailService;
        }

        public async Task<bool> AddUserCourse(UserCourseRequest userCourseRequest)
        {
            if (userCourseRequest == null || userCourseRequest.userCoursesDetails.IsNullOrEmpty())
            {
                throw new ArgumentNullException("No Data To insert");
            }
            var UserID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            if (UserID == null) {
                throw new ArgumentNullException("cant get ur user Data to insert ");

            }
            int userIdInt = 0;
            decimal totalPrice=0;
            List<string> coursesName = new List<string>();
            if (!int.TryParse(UserID, out userIdInt))
            {
                throw new Exception("Invalid UserID"); 
            }

            foreach (var userCoursesDetails in userCourseRequest.userCoursesDetails)
            {
                // Fetch product from database
                var CourseData = await _unitOfWork.Courses.Find(x => x.courseID == userCoursesDetails.courseID);
                if (CourseData == null)
                {
                    throw new Exception($"Product with ID {userCoursesDetails.courseID} not found.");
                }

                userCoursesDetails.coursePrice = CourseData.CoursePrice;
                totalPrice = totalPrice + CourseData.CoursePrice;
                coursesName.Add(CourseData.courseName);

            }


            //userCourseRequest.Total = userCourseRequest.userCoursesDetails.Sum(x => x.coursePrice);
            userCourseRequest.Total = totalPrice;

            decimal tax = userCourseRequest.Total * 0.15m;
            userCourseRequest.Payment.Amount = tax+ userCourseRequest.Total;


            UserCoursesHeader userCoursesHeader = new UserCoursesHeader
            {
                UserID = userIdInt,
                Discount = 0,
                Tax = userCourseRequest.Tax,
                Total = userCourseRequest.Total,
                userCoursesDetails = userCourseRequest.userCoursesDetails,
            };


            


            if (userCourseRequest.Payment.PaymentType.ToLower() == "paypal")
            {
                PaypalPayment paypal = new PaypalPayment
                {
                    UserID = userIdInt,
                    Country = userCourseRequest.Payment.Country,
                    State = userCourseRequest.Payment.State,
                    Amount = userCourseRequest.Payment.Amount,
                    PaypalEmail = userCourseRequest.Payment.PaypalEmail

                };

                // user chose PayPal
                await _unitOfWork.PaypalPayment.Add(paypal);
            }
            else if (userCourseRequest.Payment.PaymentType.ToLower() == "Credit Card"|| userCourseRequest.Payment.PaymentType.ToLower() == "creditcard")
            {
                CreditCardPayment creditCard = new CreditCardPayment
                {
                    UserID = userIdInt,
                    Country = userCourseRequest.Payment.Country,
                    State = userCourseRequest.Payment.State,
                    Amount = userCourseRequest.Payment.Amount,
                    CardName = userCourseRequest.Payment.CardName,
                    CardNumber = userCourseRequest.Payment.CardNumber,
                    CVV = userCourseRequest.Payment.CVV,
                    ExpiryDate = userCourseRequest.Payment.ExpiryDate,

                };
                // user chose Credit Card
               await _unitOfWork.CreditCardPayment.Add(creditCard);
            }
            var Data = Add(userCoursesHeader);
            await _unitOfWork.SaveChangesAsync();

            await SendPurchaseMsg(coursesName);

            return true;


        }
        private async Task SendPurchaseMsg(List<string> coursesName)
        {

            var email = _AuthService.GetClaim(ClaimTypes.Email);
            var name = _AuthService.GetClaim(ClaimTypes.Name);
            var Data =  await _unitOfWork.Users.Find(x => x.UserEmail == email);
            if (Data.TypeSign== null) 
            {
                return;
            }

            var SubjectMsg = $"🎉 Thank You for Your Purchase, {name}! Your Courses Are Ready";

            // Build course list in HTML
            string courseList = "";
            if (coursesName != null && coursesName.Count > 0)
            {
                courseList = "<ul>";
                foreach (var course in coursesName)
                {
                    courseList += $"<li>{System.Net.WebUtility.HtmlEncode(course)}</li>";
                }
                courseList += "</ul>";
            }

            var Message = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                              <meta charset='UTF-8'>
                              <style>
                                body {{ font-family: Arial, sans-serif; color: #333; line-height: 1.6; }}
                                .container {{ max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee; border-radius: 10px; background: #f9f9f9; }}
                                h1 {{ color: #4CAF50; }}
                                .courses {{ background: #fff; padding: 15px; border-radius: 8px; margin-top: 15px; }}
                                .TeamMsg {{ margin-top: 20px; font-size: 0.9em; color: #666; }}
                              </style>
                            </head>
                            <body>
                              <div class='container'>
                                <h1>Thank you for your purchase, {name}! 🎉</h1>
                                <p>Hi {name}, 👋</p>
                                <p>We're excited to let you know that your courses are now available in your dashboard. 🚀</p>

                                {(string.IsNullOrEmpty(courseList) ? "" : $"<div class='courses'><h3>📚 Your Courses:</h3>{courseList}</div>")}

                                <p>Best of luck on your learning journey. We’re here to support you every step of the way. 💡</p>
            
                                <p class='TeamMsg'>Happy Learning,<br/>ByWay Team</p>
                              </div>
                            </body>
                            </html>";

            await _emailService.SendEmailAsync(email, SubjectMsg, Message);
        }
    }
}
