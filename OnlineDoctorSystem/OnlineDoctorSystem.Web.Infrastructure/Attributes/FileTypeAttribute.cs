using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnlineDoctorSystem.Web.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileTypeAttribute : ValidationAttribute
    {
        public string[] AllowedTypes { get; set; }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                if (AllowedTypes == null || AllowedTypes.Length == 0)
                {
                    return new ValidationResult(GetErrorMessage());
                }

                if (!AllowedTypes.Any(t => file.ContentType.EndsWith(t)))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        private string GetErrorMessage()
        {
            return $"Допустими формати на изображение: {string.Join(", ", AllowedTypes)}.";
        }
    }
}
