using AngleSharp;
using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace OnlineDoctorSystem.Services.Scraping
{
    public class DoctorScraperService : IDoctorScraperService
    {
        private readonly OnlineDoctorDbContext context;
        private string baseUrl = "https://superdoc.bg/lekari?page={0}&region_id={1}";
        private int townId; // save townId between methods
        private IBrowsingContext browsingContext = new BrowsingContext();

        // map to convert townIds from my database to corresponding superdoc townIds
        private static Dictionary<int, int> townIdMapper = new Dictionary<int, int>()
        {
            {1, 1},
            {2, 2},
            {3, 3},
            {4, 4},
            {5, 6},
            {6, 7},
            {7, 8},
            {8, 9},
            {9, 10},
            {10, 12},
            {11, 13},
            {12, 14},
            {13, 15},
            {14, 16},
            {15, 17},
            {16, 18},
            {17, 19},
            {18, 20},
            {19, 21},
            {20, 23},
            {21, 24},
            {22, 25},
            {23, 26},
            {24, 27},
            {25, 28},
        };


        public DoctorScraperService(OnlineDoctorDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Import(int count, int townId)
        {
            this.townId = townId;
            var config = Configuration.Default.WithDefaultLoader();
            this.browsingContext = BrowsingContext.New(config);

            var links = new List<string>();

            var pages = Math.Ceiling(count / 20m); //20 doctors per page

            for (int i = 0; i < pages; i++)
            {
                var pageNum = i == 0
                    ? (int?)null
                    : i;

                var url = string.Format(baseUrl, pageNum, townIdMapper[townId]);
                var doctorsToAdd = count - links.Count;

                links.AddRange(await this.GetLinks(url, doctorsToAdd));
            }

            var addedDoctors = new List<Doctor>();
            for (int j = 0; j < count; j++)
            {
                try
                {
                    var doctor = await this.GetDoctor(links[j]);

                    if (addedDoctors.Any(d => d.Name == doctor.Name) ||
                        await this.context.Doctors.AnyAsync(d => d.Name == doctor.Name)) // should end if we end up in the same page or if doctor is already seeded
                    {
                        throw new Exception();
                    }

                    addedDoctors.Add(doctor);
                }
                catch (Exception e)
                {
                    await this.context.Doctors.AddRangeAsync(addedDoctors);
                    await this.context.SaveChangesAsync();
                    return addedDoctors.Count;
                }
            }

            await this.context.Doctors.AddRangeAsync(addedDoctors);
            await this.context.SaveChangesAsync();
            return addedDoctors.Count;
        }

        private async Task<List<string>> GetLinks(string url, int count)
        {
            var document = await this.browsingContext
              .OpenAsync(url);

            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Тази страница не е намерена"))
            {
                throw new InvalidOperationException();
            }

            var indexLinks = document.QuerySelectorAll(".search-result .search-result-link")
                .Take(count)
                .Select(x => x.GetAttribute("href"))
                .ToList();

            return indexLinks!;
        }

        private async Task<Doctor> GetDoctor(string url)
        {
            var document = await this.browsingContext
                .OpenAsync(url);

            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Тази страница не е намерена"))
            {
                throw new InvalidOperationException();
            }

            var isFromThirdParty = true;

            var originalUrl = url;

            var doctorName = document.QuerySelectorAll(".doctor-name h1").Select(x => x.TextContent).First();

            var imageUrl = document.QuerySelectorAll(".doctor-images .gallery").First().GetAttribute("href");

            var specialty = document.QuerySelectorAll(".doctor-name h2").Select(x => x.TextContent).First().Split(", ").First();

            //Get DoctorTown
            //var town = document.QuerySelectorAll(".doctor-name h3").Select(x => x.TextContent).First().Split(", ").First();

            var smallInfo = document.QuerySelectorAll(".col-lg-10 p").Select(x => x.TextContent).First().Trim();

            var doctorPersonalInfo = document.QuerySelectorAll(".doctor-description p").Select(x => x.TextContent).ToList();

            var education = doctorPersonalInfo[0];

            var previousWork = doctorPersonalInfo[1];

            var qualifications = document.QuerySelectorAll(".doctor-description ul li").Select(x => x.TextContent).ToList();

            string? doctorEmail = null;
            try
            {
                doctorEmail = document.QuerySelectorAll(".doctor-description a").Select(x => x.TextContent).First()
                    .Trim();
            }
            catch (Exception e)
            {
                doctorEmail = null;
            }

            var coordinates = document.QuerySelectorAll("script").Select(x => x.TextContent).ToList()[4];

            var (latitude, longitude) = this.GetCoordinates(coordinates);

            var doctor = new Doctor()
            {
                IsFromThirdParty = isFromThirdParty,
                LinkFromThirdParty = originalUrl,
                Name = doctorName,
                ImageUrl = imageUrl,
                SmallInfo = smallInfo,
                Qualifications = string.Join(", ", qualifications),
                Education = education,
                Biography = previousWork,
                ContactEmailFromThirdParty = doctorEmail,
                TownId = this.townId,
                Specialty = await this.GetSpecialtyFromDb(specialty),
                IsConfirmed = true,
                Phone = string.Empty,
                Latitude = latitude,
                Longitude = longitude,
            };

            return doctor;
        }

        private async Task<Specialty> GetSpecialtyFromDb(string specialtyName)
        {
            var specialty = await this.context.Specialties.FirstOrDefaultAsync(x => x.Name == specialtyName);
            if (specialty != null)
            {
                return specialty;
            }

            var newSpecialty = new Specialty() { Name = specialtyName };

            await this.context.Specialties.AddAsync(newSpecialty);
            await this.context.SaveChangesAsync();

            return newSpecialty;
        }

        private (double, double) GetCoordinates(string coordinates)
        {
            var match = Regex.Match(coordinates, @".+?""lat"":(?<lat>\d+(\.\d+)?),""lon"":(?<lon>\d+(\.\d+)?).+");
            double latitude = double.Parse(match.Groups["lat"].Value);
            double longitude = double.Parse(match.Groups["lon"].Value);

            return (latitude, longitude);
        }
    }
}