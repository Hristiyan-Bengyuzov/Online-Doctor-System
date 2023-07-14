﻿namespace OnlineDoctorSystem.Data.Models
{
    using System;
    using System.Collections.Generic;

    using OnlineDoctorSystem.Data.Common.Models;
    using OnlineDoctorSystem.Data.Models.Enums;

    public class Patient : BaseDeletableModel<string>
    {
        public Patient()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Consultations = new HashSet<Consultation>();
            this.Prescriptions = new HashSet<Prescription>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string ImageUrl { get; set; }

        public Town Town { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Consultation> Consultations { get; set; }

        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}