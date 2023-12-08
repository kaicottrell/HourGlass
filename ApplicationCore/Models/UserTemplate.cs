using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
	public class UserTemplate
	{
		public int Id { get; set; }
		public int TemplateId { get; set; }
		[ForeignKey("TemplateId")]
		public virtual required Template Template { get; set; }
		public int ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		public virtual required ApplicationUser ApplicationUser { get; set; }
	}
}
