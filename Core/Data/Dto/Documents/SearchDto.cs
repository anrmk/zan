using System;
using System.Collections.Generic;

namespace Core.Data.Dto.Documents {
    public class SearchDto {
        #region Search Settings
        /// <summary>
        /// Критерий поиска
        /// Искать по тексту или названию
        /// </summary>
        public string SearchDb { get; set; } = "1";

        /// <summary>
        /// Строка поиска
        /// </summary>
        public string SearchText { get; set; } = "";

        public bool Regex { get; set; } = false;

        /// <summary>
        /// Близость слов
        /// </summary>
        public bool IsNear { get; set; } = true;

        /// <summary>
        /// Совпадение
        /// </summary>
        public int LogicalMulti { get; set; } = 1;

        /// <summary>
        /// Окончание
        /// </summary>
        public int EndOfWords { get; set; } = 1;
        #endregion

        public int Draw { get; set; }
        public int Length { get; set; }
        public int Start { get; set; }

        public Guid? Id { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 100;
        public int Total { get; set; } = 0;
        public int TotalPages {
            get {
                return ((Total + Limit) / Limit);
            }
        }
        //public SortEnum Sort { get; set; } = SortEnum.ByRelevance;
        public bool SordByDesc { get; set; } = true;

        public string Spells { get; set; }

        #region Requisities
        public string SelectedStatuses { get; set; }
        public string UnselectedStatuses { get; set; }
        public string SelectedRegistered { get; set; }
        public string UnselectedRegistered { get; set; }

        public string AcceptNumber { get; set; }
        public DateTime? AcceptedDateStart { get; set; }
        public DateTime? AcceptedDateFinish { get; set; }

        public string RegNumber { get; set; }
        public DateTime? RegJustDateStart { get; set; }
        public DateTime? RegJustDateFinish { get; set; }

        public string NgrWord { get; set; }
        public string GrWord { get; set; }
        #endregion

        public IList<int?> Languages { get; set; }
        public IList<int?> Statuses { get; set; }
        public IList<Guid?> AcceptedRegions { get; set; }
        public IList<Guid?> DocumentTypes { get; set; }
        public IList<Guid?> DocumentSections { get; set; }
    }
}
