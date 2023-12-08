using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
	public class Session
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
		public DateTime SessionStart { get; set; }
		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
		public DateTime SessionEnd { get; set; }
		[Required]
        public TimeSpan Duration { get; set; }
        int TemplateId { get; set; }
		[ForeignKey("TemplateId")]
		public virtual required Template Template { get; set; }
		
		

	}
}
