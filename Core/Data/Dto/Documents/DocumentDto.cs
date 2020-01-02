using System;
using Core.Data.Dto.Nsi;

namespace Core.Data.Dto.Documents {
    public class DocumentDto {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public string Ngr { get; set; }
        public int? GosNumber { get; set; }
        public string AcceptNumber { get; set; }
        public string RegNumber { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public Guid SectionId { get; set; }
        public string SectionName { get; set; }

        public DateTime? AcceptedDate { get; set; }

        public DateTime EditionDate { get; set; }
        public string DisplayEditionDate => EditionDate.ToString("dd MMM yyyy");

        public DateTime? EntryDate { get; set; }
        public DateTime? RegJustDate { get; set; }
        public DateTime? RegSystDate { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string PrintDepartment { get; set; }
        public bool? IssetOtherEditions { get; set; }
        public bool? IsArchive { get; set; }
    }
}
