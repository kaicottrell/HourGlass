using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ApplicationCore.Models
{
	public class ApplicationUser
	{
		public int Id { get; set; }
		public string? FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;

		[NotMapped]
		public string? FullName { get { return FirstName + " " + LastName; } }
		[Display(Name = "Profile Picture")]
		public string? Photo { get; set; } = "/images/default-profile.jpg";
		[Display(Name = "Date of Birth")]
		public DateTime? BirthDate { get; set; }


	}
}
