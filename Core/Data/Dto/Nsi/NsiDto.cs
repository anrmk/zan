
namespace Core.Data.Dto.Nsi {
    public class NsiDto {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public int? Sort { get; set; }
        public bool Visibility { get; set; }
    }
}
