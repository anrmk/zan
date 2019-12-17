using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;
using Core.Data.Entities.Nsi;

namespace Core.Data.Entities.Documents {
    public class DocumentEntity: Entity<Guid> {
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        public string Info { get; set; }

        /// <summary>
        /// Номер группы регистрации
        /// </summary>
        public string Ngr { get; set; }

        /// <summary>
        /// Номер в Госреестре НПА РК
        /// </summary>
        public int? GosNumber { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public string AcceptNumber { get; set; }

        /// <summary>
        /// Номер регистрации в МЮ
        /// </summary>
        public string RegNumber { get; set; }

        /// <summary>
        /// Статус документа
        /// </summary>
        [ForeignKey("Status")]
        public int? NsiDocumentStatusEntity_Id { get; set; }
        public virtual NsiDocumentStatusEntity Status { get; set; }

        /// <summary>
        /// Раздел законодательства
        /// </summary>
        [ForeignKey("Section")]
        public Guid? NsiDocumentSectionEntity_Id { get; set; }
        public virtual NsiDocumentSectionEntity Section { get; set; }

        /// <summary>
        /// Язык документа
        /// </summary>
        [ForeignKey("Language")]
        public int? NsiLanguageEntity_Id { get; set; }
        public virtual NsiLanguageEntity Language { get; set; }

        /// <summary>
        /// Орган разработчик
        /// </summary>
        [ForeignKey("DevAgency")]
        public Guid? NsiDevAgencyEntity_Id { get; set; }
        public virtual NsiDevAgencyEntity DevAgency { get; set; }

        /// <summary>
        /// Орган госрегистрации
        /// </summary>
        [ForeignKey("RegAgency")]
        public Guid? NsiRegAgencyEntity_Id { get; set; }
        public virtual NsiRegAgencyEntity RegAgency { get; set; }

        /// <summary>
        /// Регион действия
        /// </summary>
        [ForeignKey("AcceptedRegion")]
        public Guid? NsiRegionEntity_Id { get; set; }
        public virtual NsiRegionEntity AcceptedRegion { get; set; }

        /// <summary>
        /// Место принятия
        /// </summary>
        [ForeignKey("InitRegion")]
        public Guid? NsiInitRegionEntity_Id { get; set; }
        public virtual NsiInitRegionEntity InitRegion { get; set; }

        /// <summary>
        /// Сфера правоотношений
        /// </summary>
        [ForeignKey("Classifier")]
        public Guid? NsiClassifierEntity_Id { get; set; }
        public virtual NsiClassifierEntity Classifier { get; set; }

        /// <summary>
        /// Юридическая сила акта
        /// </summary>
        [ForeignKey("LawForce")]
        public Guid? NsiLawForceEntity_Id { get; set; }
        public virtual NsiLawForceEntity LawForce { get; set; }

        /// <summary>
        /// Дата принятия
        /// </summary>
        public DateTime? AcceptedDate { get; set; }

        /// <summary>
        /// Дата редакции документа
        /// </summary>
        public DateTime EditionDate { get; set; }

        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        public DateTime? EntryDate { get; set; }

        /// <summary>
        /// Дата регистрации в МЮ РК
        /// </summary>
        public DateTime? RegJustDate { get; set; }

        /// <summary>
        /// Дата регистрации в информационном фонде
        /// </summary>
        public DateTime? RegSystDate { get; set; }

        /// <summary>
        /// Дата публикации
        /// </summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>
        /// Печатный орган
        /// </summary>
        public string PrintDepartment { get; set; }

        /// <summary>
        /// Признак, имеются ли другие редакции
        /// </summary>
        public bool? IssetOtherEditions { get; set; }

        /// <summary>
        /// Признак архивности
        /// </summary>
        public bool? IsArchive { get; set; }
    }
}
