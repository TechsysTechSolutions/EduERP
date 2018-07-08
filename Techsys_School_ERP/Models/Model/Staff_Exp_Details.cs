
namespace Techsys_School_ERP.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Staff_Exp_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long? Staff_Id { get; set; }

        public int? School_Id { get; set; }

    
        public int Designation_Id { get; set; }

        public long? From_Year { get; set; }

        public long? To_Year { get; set; }

        [StringLength(50)]
        public string Subject_Id { get; set; }

        public long? Academic_Year { get; set; }

        public bool? Is_Active { get; set; }

        public bool? Is_Deleted { get; set; }

        public DateTime? Created_On { get; set; }

        public int Created_By { get; set; }

        public DateTime? Updated_On { get; set; }

        public int? Updated_By { get; set; }

        public byte[] Upload_Document1 { get; set; }

        public byte[] Upload_Document2 { get; set; }

        public byte[] Upload_Document3 { get; set; }
    }
}
