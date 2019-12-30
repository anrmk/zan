using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Extensions {
    public class Pager<T> {
        /// <summary>
        /// Общее количество записей
        /// </summary>
        public int RecordsTotal { get; private set; }
        public int RecordsFiltered => RecordsTotal;

        public int Start { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public IEnumerable<T> Data { get; private set; }

        public Pager(IEnumerable<T> list, int totalItems, int? page, int pageSize = 20) {
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if(startPage <= 0) {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if(endPage > totalPages) {
                endPage = totalPages;
                if(endPage > 10) {
                    startPage = endPage - 9;
                }
            }

            RecordsTotal = totalItems;
            Start = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            Data = list;
        }
    }

    public class PagerQuery {
        public int Take { get; set; } = 20;
        public int Skip { get; set; } = 0;
        public List<PagerSortQuery> Sort { get; set; }
    }

    public class PagerSortQuery {
        public bool Desc { get; set; } = false;
        public string Selector { get; set; }
    }

    public class PagerExtended<T>: Pager<T> {
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
        public int LogicalMulti { get; set; } = 1;

        /// <summary>
        /// Окончание
        /// </summary>
        public int EndOfWords { get; set; } = 1;
        #endregion

        public IList<SelectListItem> Languages { get; set; }
        public IList<int> Statuses { get; set; }
        public IList<Guid> AcceptedRegions { get; set; }

        public PagerExtended(IEnumerable<T> list, int totalItems, int? pager, int pageSize = 0) 
            : base(list, totalItems, pager, pageSize) {

        }
    }
}
