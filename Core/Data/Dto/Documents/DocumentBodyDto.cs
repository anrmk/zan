using Core.Extensions;

namespace Core.Data.Dto.Documents {
    public class DocumentBodyDto {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public string Body { get; set; }

        public string Title => DocumentExtension.GetTitle(Body);

        public string Content => string.Concat(DocumentExtension.GetContent(Body));
    }
}