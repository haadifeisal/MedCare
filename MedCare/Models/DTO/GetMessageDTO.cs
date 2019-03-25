using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class GetMessageDTO
    {
        public int Id { get; set; }
        public string From { get; set; }

        public Boolean Read { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:r}", ApplyFormatInEditMode = true)]
        [Display(Name = "Message Date")]
        public string TimeSent { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Body")]
        public string Content { get; set; }
    }
}
