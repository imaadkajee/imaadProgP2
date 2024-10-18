using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ImaadProgP2.Models
{
    public class Claims
    {
        public int Id { get; set; }

        [Required]
        public string LecturerID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime ClaimsPeriodStart { get; set; }

        [Required]
        public DateTime ClaimsPeriodEnd { get; set; }

        [Required]
        public double HoursWorked { get; set; }

        [Required]
        public double RatePerHour { get; set; }

        public double TotalAmount { get; set; }

        public string? DescriptionOfWork { get; set; }

        public string? DocumentPath { get; set; }

        public string ApprovalStatus { get; set; } = "Pending Loading ";

        public string? ApprovedBy { get; set; }
    }
}
