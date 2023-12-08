using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
	public class Template
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; } = string.Empty;
		public string? TemplateImage { get; set; }
		public string TemplateColor { get; set; } = "#FFFFFF";
		public int TemplateGroupId { get; set; }
		[ForeignKey("TemplateGroupId")]
		public virtual TemplateGroup? TemplateGroup { get; set; }
		
		

	}
}
