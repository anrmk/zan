using System;
using System.Collections.Generic;

namespace Core.Data.Dto.Documents {
    public class DocumentDto {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }

        public string Ngr { get; set; }
        public int? GosNumber { get; set; }
        public string AcceptNumber { get; set; }
        public string RegNumber { get; set; }

        public int? StatusId { get; set; }
        public string Status { get; set; }

        public Guid? SectionId { get; set; }
        public string Section { get; set; }

        public Guid? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }

        public int LanguageId { get; set; }
        public string Language { get; set; }

        public Guid? DevAgencyId { get; set; }
        public string DevAgency { get; set; }

        public Guid? RegAgencyId { get; set; }
        public string RegAgency { get; set; }

        public Guid? RegionActionId { get; set; }
        public string RegionAction { get; set; }

        public Guid? AcceptedRegionId { get; set; }
        public string AcceptedRegion { get; set; }

        public Guid? InitRegionId { get; set; }
        public string InitRegion { get; set; }

        public Guid? ClassifierId { get; set; }
        public string Classifier { get; set; }

        public Guid? LawForceId { get; set; }
        public string LawForce { get; set; }

        public DateTime? AcceptedDate { get; set; }
        public DateTime EditionDate { get; set; }
        public string DisplayEditionDate => EditionDate.ToString("dd/MM/yyyy");
        public DateTime? EntryDate { get; set; }
        public DateTime? RegJustDate { get; set; }
        public DateTime? RegSystDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string PrintDepartment { get; set; }
        public bool? IssetOtherEditions { get; set; }
        public bool? IsArchive { get; set; }

        public DocumentBodyDto Content { get; set; }
        public List<DocumentDto> Versions { get; set; }

        //  public IEnumerable<NsiDto<int>> AvailableLanguages => DocumentDtoExtension.GetLanguages(Versions);

    }
}
