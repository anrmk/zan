﻿using System;
using System.Collections.Generic;

namespace Web.Models.Document {
    public class SearchString {
        public string Value { get; set; }
        public bool Regex { get; set; } = false;

    }

    public class SearchViewModel {

        public SearchString Search { get; set; }

        public int Draw { get; set; }
        public int Length { get; set; }
        public int Start { get; set; }

        #region Search Filter
        public IList<int?> Languages { get; set; }
        public IList<int?> Statuses { get; set; }
        public IList<Guid?> AcceptedRegions { get; set; }
        public IList<Guid?> DocumentTypes { get; set; }
        public IList<Guid?> DocumentSections { get; set; }
        #endregion

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

        /// <summary>
        /// Близость слов
        /// </summary>
        public bool IsNear { get; set; } = true;

        /// <summary>
        /// Совпадение
        /// </summary>
        public LogicalMultiEnum LogicalMulti { get; set; } = LogicalMultiEnum.And;

        /// <summary>
        /// Окончание
        /// </summary>
        public EndOfWordsEnum EndOfWords { get; set; } = EndOfWordsEnum.Any;
        #endregion

        public SortEnum Sort { get; set; } = SortEnum.ByRelevance;

        public bool SortByDesc { get; set; } = false;
        /*
        public Guid? Id { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 100;
        public int Total { get; set; } = 0;
        public int TotalPages { get { return ((Total / Size) + 1); } }
        

        public string Spells { get; set; }
        */

        #region Requisities
        //public string SelectedStatuses { get; set; }
        //public string UnselectedStatuses { get; set; }
        //public string SelectedRegistered { get; set; }
        //public string UnselectedRegistered { get; set; }

        //public string AcceptNumber { get; set; }
        //public DateTime? AcceptedDateStart { get; set; }
        //public DateTime? AcceptedDateFinish { get; set; }

        //public string RegNumber { get; set; }
        //public DateTime? RegJustDateStart { get; set; }
        //public DateTime? RegJustDateFinish { get; set; }

        //public string NgrWord { get; set; }
        //public string GrWord { get; set; }
        #endregion
    }

    public enum SortEnum {
        ByRelevance = 1,
        ByLawForce = 2,
        ByAcceptedDate = 3,
        ByUpdatedDate = 4
    }

    public enum LogicalMultiEnum {
        And = 1,
        Or = 2
    }

    public enum EndOfWordsEnum {
        Any = 1,
        Equal = 2,
        Default = 3
    }
}
