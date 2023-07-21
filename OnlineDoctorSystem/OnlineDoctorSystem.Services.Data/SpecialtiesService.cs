﻿using Microsoft.EntityFrameworkCore;
using OnlineDoctorSystem.Data;
using OnlineDoctorSystem.Data.Models;

namespace OnlineDoctorSystem.Services.Data.Interfaces
{
    public class SpecialtiesService : ISpecialtiesService
    {
        private readonly OnlineDoctorDbContext context;

        public SpecialtiesService(OnlineDoctorDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<string>> AllSpecialtyNamesAsync() => await this.context.Specialties.Select(s => s.Name).ToListAsync();

        public async Task<IEnumerable<Specialty>> GetAllSpecialties() => await this.context.Specialties.ToListAsync();
    }
}