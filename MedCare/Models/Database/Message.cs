using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.Database
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Receiver")]
        public string To { get; set; }

        [Display(Name = "Sender")]
        public string From { get; set; }

        public Boolean Read { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:r}", ApplyFormatInEditMode = true)]
        [Display(Name = "Message Date")]
        public DateTime TimeSent { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Body")]
        public string Content { get; set; }
    }
}
