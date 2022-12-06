
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SNICKERS.EF.Models;

namespace SNICKERS.Shared.DTO
{
    public class EnrollmentDTO
    {
        public int StudentId { get; set; }
        [StringLength(50)]

        public int SectionId { get; set; }
        [StringLength(50)]

        public DateTime EnrollDate { get; set; }

        public int FinalGrade { get; set; }

        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        [StringLength(30)]

        public string ModifiedBy { get; set; } = null!;
        public DateTime ModifiedDate { get; set; }
        public int SchoolId { get; set; }


    }
}