using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class SendMessageDTO
    {
        [Required]
        public string To { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Maximum limit of characters: 100")]
        public string Subject { get; set; }

        [Required]
        [StringLength(4000, ErrorMessage = "Maximum limit of characters: 4000")]
        public string Content { get; set; }
    }
}
