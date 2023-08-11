using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using OnlineDoctorSystem.Common;
using OnlineDoctorSystem.Data.Models;
using OnlineDoctorSystem.Data.Models.Enums;
using OnlineDoctorSystem.Services.Data.Interfaces;

namespace OnlineDoctorSystem.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterDoctor : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterDoctor> logger;
        private readonly IDoctorsService doctorsService;

        public RegisterDoctor(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterDoctor> logger,
            IDoctorsService doctorsService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.doctorsService = doctorsService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
			returnUrl ??= this.Url.Content("~/Doctor/ThankYou");
			ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Any(x => this.Input.Image.FileName.EndsWith(x)))
            {
                this.ModelState.AddModelError("Image", "Invalid file type.");
            }

			if (this.ModelState.IsValid)
            {
                CloudinaryDotNet.Account account = new CloudinaryDotNet.Account
                        (
                            Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME"),
                            Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
                            Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
                        );

                var cloudinary = new Cloudinary(account);

                var file = this.Input.Image;

                var uploadResult = new ImageUploadResult();

                var imageUrl = string.Empty;

                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        using var stream = file.OpenReadStream();
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.Name, stream),
                            Transformation = new Transformation().Width(256).Height(256).Gravity("face").Radius("max").Border("2px_solid_white").Crop("thumb"),
                        };

                        uploadResult = cloudinary.Upload(uploadParams);
                    }

                    imageUrl = uploadResult.Url.ToString();
                }

                var user = new ApplicationUser { UserName = this.Input.Email, Email = this.Input.Email };

                var doctor = new Doctor()
                {
                    Name = this.Input.Name,
                    Phone = this.Input.Phone,
                    TownId = this.Input.TownId,
                    SpecialtyId = this.Input.SpecialtyId,
                    BirthDate = this.Input.BirthDate,
                    Gender = this.Input.Gender,
                    SmallInfo = this.Input.SmallInfo,
                    Education = this.Input.Education,
                    Qualifications = this.Input.Qualifications,
                    Biography = this.Input.Biography,
                    ImageUrl = imageUrl,
                    Latitude = this.Input.Latitude,
                    Longitude = this.Input.Longitude,
                };

                var result = await this.userManager.CreateAsync(user, this.Input.Password);

                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new
                        {
                            area = "Identity",
                            userId = user.Id,
                            code,
                            returnUrl,
                        },
                        protocol: this.Request.Scheme);


                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl });
                    }
                    else
                    {
                        await this.userManager.AddToRoleAsync(user, GlobalConstants.DoctorRole);

                        await this.signInManager.SignInAsync(user, isPersistent: false);
                        await this.doctorsService.AddDoctorToDbAsync(user.Id, doctor);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "������� ��� � ������������")]
            [MinLength(3, ErrorMessage = "����� ������ �� �� ������ �� ������� 3 �������")]
            [MaxLength(40, ErrorMessage = "����� ������ �� �� ������ �� �������� 40 �������")]
            public string Name { get; set; }

            [Required(ErrorMessage = "��������� � ������������")]
            [Phone]
            public string Phone { get; set; }

            [Required]
            public int TownId { get; set; }

            [Required(ErrorMessage = "��������� ���� � ������������")]
            [DataType(DataType.Date)]
            public DateTime BirthDate { get; set; }

            [Required]
            public Gender Gender { get; set; }

            [Required]
            public int SpecialtyId { get; set; }

            [Required(ErrorMessage = "������ � ������������")]
            [MinLength(30)]
            [MaxLength(200)]
            public string SmallInfo { get; set; }

            [Required(ErrorMessage = "������ � ������������")]
            [MinLength(30)]
            [MaxLength(200)]
            public string Education { get; set; }

            [Required(ErrorMessage = "������ � ������������")]
            [MinLength(30)]
            [MaxLength(200)]

            public string Qualifications { get; set; }

            [Required(ErrorMessage = "������ � ������������")]
            [MinLength(30)]
            [MaxLength(200)]
            public string Biography { get; set; }

            [Required(ErrorMessage = "������ � ������������")]
            [EmailAddress]
            public string Email { get; set; }

            [Required(ErrorMessage = "������ � ������������")]
            [MinLength(6, ErrorMessage = "�������� ������ �� �� ������ �� ������� 6 �������")]
            [MaxLength(25, ErrorMessage = "�������� ������ �� �� ������ �� �������� 25 �������")]
            [PasswordPropertyText]
            public string Password { get; set; }

            [Required(ErrorMessage = "������ � ������������")]
            [Compare("Password", ErrorMessage = "�������� ������ �� ��������.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "�������� � ������������")]
            public IFormFile Image { get; set; }

            public double Latitude { get; set; } 

            public double Longitude { get; set; } 
        }
    }
}