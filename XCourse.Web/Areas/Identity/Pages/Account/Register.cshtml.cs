// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Encodings.Web;
using XCourse.Core.Entities;
using XCourse.Infrastructure.Repositories.Interfaces;

namespace XCourse.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserStore<AppUser> _userStore;
        private readonly IUserEmailStore<AppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        public AccountType AccountType;

        public RegisterModel(
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = new();

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{2,19}$",
                ErrorMessage = "Username must start with a letter and can contain only letters, numbers, underscores, and dots (3-20 characters).")]
            public string UserName { get; set; }
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [MaxLength(25, ErrorMessage = "Number of characters for first name must be less than or equal 25")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            [MaxLength(25, ErrorMessage = "Number of characters for last name must be less than or equal 25")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
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
            public AccountType AccountType { get; set; }
        }


        public async Task OnGetAsync(AccountType accountType, string returnUrl = null)
        {
            AccountType = accountType;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                Address address = new()
                {
                    Street = Input.Street,
                    Neighborhood = Input.Neighborhood,
                    City = Input.City,
                    Governorate = Input.Governorate
                };

                user.HomeAddress = address;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.DateOfBirth = Input.DateOfBirth;
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

                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (Input.AccountType == AccountType.Student)
                    {
                        Student student = new Student()
                        {
                            Year = Input.Year,
                            Major = Input.Major,
                            AppUserID = user.Id
                        };
                        _unitOfWork.Students.Add(student);
                        _unitOfWork.Save();
                    }
                    else if (Input.AccountType == AccountType.Teacher)
                    {
                        Teacher teacher = new Teacher()
                        {
                            AppUserID = user.Id,
                            IsAvailableForPrivateGroups = Input.IsAvailableForPrivateGroups,
                            PrivatePrice = Input.PrivatePrice
                        };
                        _unitOfWork.Teachers.Add(teacher);
                        _unitOfWork.Save();
                    }
                    else if (Input.AccountType == AccountType.Assistant)
                    {
                        Assistant assistant = new Assistant()
                        {
                            AppUserID = user.Id
                        };
                        _unitOfWork.Assistants.Add(assistant);
                        _unitOfWork.Save();
                    }
                    else if (Input.AccountType == AccountType.CenterAdmin)
                    {
                        CenterAdmin centerAdmin = new CenterAdmin()
                        {
                            AppUserID = user.Id
                        };
                        _unitOfWork.CenterAdmins.Add(centerAdmin);
                        _unitOfWork.Save();
                    }

                    Wallet wallet = new Wallet()
                    {
                        AppUserID = user.Id
                    };
                    _unitOfWork.Wallets.Add(wallet);
                    _unitOfWork.Save();

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                    $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<AppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<AppUser>)_userStore;
        }
    }
}
