
namespace Core.Data.Dto.Nsi {
    public class NsiDto<T> {
        public T Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameRu { get; set; }
        public string NameKk { get; set; }
        public string NameEn { get; set; }
        public int? Sort { get; set; }
        public bool Visibility { get; set; }

        public string GetName(string culture) {
            switch(culture) {
                case "ru":
                    return NameRu;
                case "en":
                    return NameEn;
                case "kk":
                    return NameKk;
                default:
                    return Name;
            }
        }
    }
}
