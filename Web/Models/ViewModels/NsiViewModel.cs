using System;

namespace Web.Models.ViewModels.Nsi {
    public class NsiViewModel {
        public Guid? Id { get; set; }
        public int? IdInt { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public int Count { get; set; }

        public NsiViewModel(string code, string name) {
            Code = code;
            NameRu = name;
        }
        public NsiViewModel() {

        }
    }
}
