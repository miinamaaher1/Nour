// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;

namespace XCourse.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            Governorates = new List<string>
            {
                "Cairo", "Alexandria", "Giza", "Port Said", "Suez",
                "Dakahlia", "Sharqia", "Qalyubia", "Beheira", "Minya",
                "Helwan", "6th of October", "Ismailia", "Gharbia", "Monufia",
                "Kafr El Sheikh", "Faiyum", "Beni Suef", "Asyut", "Sohag",
                "Qena", "Aswan", "Luxor", "Red Sea", "New Valley",
                "Matrouh", "North Sinai", "South Sinai", "Damietta"
            };
        }

        public List<string> Governorates;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [RegularExpression(@"^01[0125]\d{8}$")]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [MaxLength(25, ErrorMessage = "Number of characters for first name must be less than or equal 25")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [MaxLength(25, ErrorMessage = "Number of characters for last name must be less than or equal 25")]
            [Display(Name = "Last Name")] public string LastName { get; set; }
            [Column(TypeName = "varbinary(MAX)")]
            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }
            [EnumDataType(typeof(Gender))]
            public Gender Gender { get; set; }
            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Now);

            [MaxLength(25, ErrorMessage = "Number of characters for street must be less than or equal 25")]
            public string Street { get; set; }

            [MaxLength(25, ErrorMessage = "Number of characters for Neighborhood must be less than or equal 25")]
            public string Neighborhood { get; set; }

            [MaxLength(25, ErrorMessage = "Number of characters for City must be less than or equal 25")]
            public string City { get; set; }

            [MaxLength(25, ErrorMessage = "Number of characters for Governorate must be less than or equal 25")]
            public string Governorate { get; set; }
            [EnumDataType(typeof(Year))]
            public Year? Year { get; set; }

            [EnumDataType(typeof(Major))]
            public Major? Major { get; set; }
            public bool IsAvailableForPrivateGroups { get; set; }
            public decimal? PrivatePrice { get; set; }
            public string? TagLine { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth ?? DateOnly.FromDateTime(DateTime.Now),
                Street = user.HomeAddress.Street,
                Neighborhood = user.HomeAddress.Neighborhood,
                City = user.HomeAddress.City,
                Governorate = user.HomeAddress.Governorate,
                Gender = user.Gender,
                ProfilePicture = user.ProfilePicture
            };

            if (user.AccountType == AccountType.Student)
            {
                var stud = _unitOfWork.Students.Find(s => s.AppUserID == user.Id);
                Input.Year = stud.Year;
                Input.Major = stud.Major;
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.DateOfBirth = Input.DateOfBirth;
            user.HomeAddress.Street = Input.Street;
            user.HomeAddress.Neighborhood = Input.Neighborhood;
            user.HomeAddress.City = Input.City;
            user.HomeAddress.Governorate = Input.Governorate;
            user.Gender = Input.Gender;
            
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();

                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    user.ProfilePicture = dataStream.ToArray();
                }
            }

            if (user.AccountType == AccountType.Student)
            {
                var stud = _unitOfWork.Students.Find(s => s.AppUserID == user.Id);
                stud.Year = Input.Year;
                stud.Major = Input.Major;
                _unitOfWork.Students.Update(stud);
                _unitOfWork.Save();
            }
            else if (user.AccountType == AccountType.Teacher)
            {
                Teacher teacher = _unitOfWork.Teachers.Find(s => s.AppUserID == user.Id);
                teacher.IsAvailableForPrivateGroups = Input.IsAvailableForPrivateGroups;
                teacher.PrivatePrice = Input.PrivatePrice;
                teacher.TagLine = Input.TagLine;
                _unitOfWork.Teachers.Update(teacher);
                _unitOfWork.Save();
            }

            var updateInfoResult = await _userManager.UpdateAsync(user);
            if (!updateInfoResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update info.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
