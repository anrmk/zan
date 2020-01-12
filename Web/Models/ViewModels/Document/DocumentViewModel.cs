using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels.Document {
    public class DocumentViewModel {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string Content { get; set; }

        [Display(Name = "Ngr")]
        public string Ngr { get; set; }

        [Display(Name = "GosNumber")]
        public int? GosNumber { get; set; }

        [Display(Name = "AcceptNumber")]
        public string AcceptNumber { get; set; }

        [Display(Name = "RegNumber")]
        public string RegNumber { get; set; }

        [Display(Name = "PrintDepartment")]
        public string PrintDepartment { get; set; }

        public int? StatusId { get; set; }
        public string Status { get; set; }

        public Guid? SectionId { get; set; }
        public string Section { get; set; }

        public Guid? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }

        public int? LanguageId { get; set; }
        public string Language { get; set; }

        public Guid? DevAgencyId { get; set; }
        public string DevAgency { get; set; }

        public Guid? RegAgencyId { get; set; }
        public string RegAgency { get; set; }

        public Guid? RegionActionId { get; set; }
        [Display(Name = "RegionAction")]
        public string RegionAction { get; set; }

        public Guid? AcceptedRegionId { get; set; }
        public string AcceptedRegion { get; set; }

        public Guid? InitRegionId { get; set; }
        public string InitRegion { get; set; }

        public Guid? ClassifierId { get; set; }
        public string Classifier { get; set; }

        public Guid? LawForceId { get; set; }
        public string LawForce { get; set; }

        [Display(Name = "AcceptedDate")]
        public DateTime? AcceptedDate { get; set; }

        public List<DocumentViewModel> Versions { get; set; }
    }
}
