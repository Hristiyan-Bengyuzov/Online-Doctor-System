namespace OnlineDoctorSystem.Data.Models
{
    using System;

    using OnlineDoctorSystem.Data.Common.Models;

    public class Review : BaseDeletableModel<Guid>
    {
        public Review()
        {
            this.Id = Guid.NewGuid();
        }

        public double Rating { get; set; }

        public string Text { get; set; }
    }
}