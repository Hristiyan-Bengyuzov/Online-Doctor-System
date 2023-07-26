using System.Security.Claims;
using static OnlineDoctorSystem.Common.GlobalConstants;

namespace OnlineDoctorSystem.Web.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetId(this ClaimsPrincipal user) => user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static bool IsDoctor(this ClaimsPrincipal user) => user.IsInRole(DoctorRole);

        public static bool IsPatient(this ClaimsPrincipal user) => user.IsInRole(PatientRole);

        public static bool IsAdmin(this ClaimsPrincipal user) => user.IsInRole(AdminRole);
    }
}
