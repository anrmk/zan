using System;
using System.Collections.Generic;
using System.Linq;

using Core.Data.Dto.Documents;

namespace Core.Extensions {
    public static class DocumentDtoExtension {
        public static IEnumerable<int> GetLanguages(this ICollection<DocumentDto> collection, string ngr, DateTime editionDate) {
            if(collection != null && collection.Count > 0) {
                return collection.Where(x => x.Ngr == ngr && x.EditionDate == editionDate).GroupBy(x => x.LanguageId).Select(x => x.Key);
            }
            return null;
        }

        public static IEnumerable<DocumentDto> GetAvailableLanguages(this ICollection<DocumentDto> collection, string ngr, DateTime editionDate) {
            if(collection != null && collection.Count > 0) {
                return collection.Where(x => x.Ngr == ngr && x.EditionDate == editionDate).ToList();
            }
            return null;
        }

        public static IEnumerable<DocumentDto> GetAvailableVersions(this ICollection<DocumentDto> collection, string ngr, int langId) {
            if(collection != null && collection.Count > 0) {
                return collection.Where(x => x.Ngr == ngr && x.LanguageId == langId).ToList();
            }
            return null;
        }
    }
}
