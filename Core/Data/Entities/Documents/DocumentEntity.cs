using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;
using Core.Data.Entities.Nsi;

namespace Core.Data.Entities.Documents {
    public class DocumentEntity: BaseEntity<Guid> {
        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
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
        [Column(name: "NsiDocumentStatusEntity_Id")]
        public int? StatusId { get; set; }
        public virtual NsiDocumentStatusEntity Status { get; set; }

        /// <summary>
        /// Раздел законодательства
        /// </summary>
        [ForeignKey("Section")]
        [Column(name: "NsiDocumentSectionEntity_Id")]
        public Guid? SectionId { get; set; }
        public virtual NsiDocumentSectionEntity Section { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey("DocumentType")]
        [Column(name: "NsiDocumentTypeEntity_Id")]
        public Guid? DocumentTypeId { get; set; }
        public virtual NsiDocumentTypeEntity DocumentType { get; set; }

        /// <summary>
        /// Язык документа
        /// </summary>
        [ForeignKey("Language")]
        [Column(name: "NsiLanguageEntity_Id")]
        public int? LanguageId { get; set; }
        public virtual NsiLanguageEntity Language { get; set; }

        /// <summary>
        /// Орган разработчик
        /// </summary>
        [ForeignKey("DevAgency")]
        [Column(name: "NsiDevAgencyEntity_Id")]
        public Guid? DevAgencyId { get; set; }
        public virtual NsiDevAgencyEntity DevAgency { get; set; }

        /// <summary>
        /// Орган госрегистрации
        /// </summary>
        [ForeignKey("RegAgency")]
        [Column(name: "NsiRegAgencyEntity_Id")]
        public Guid? RegAgencyId { get; set; }
        public virtual NsiRegAgencyEntity RegAgency { get; set; }

        /// <summary>
        /// Регион действия 
        /// </summary>
        [ForeignKey("RegionAction")]
        [Column(name: "NsiRegionActionEntity_Id")]
        public Guid? RegionActionId { get; set; }
        public virtual NsiRegionEntity RegionAction { get; set; }

        /// <summary>
        /// Регион принятия
        /// </summary>
        [ForeignKey("AcceptedRegion")]
        [Column(name: "NsiRegionAcceptedEntity_Id")]
        public Guid? AcceptedRegionId { get; set; }
        public virtual NsiRegionEntity AcceptedRegion { get; set; }

        /// <summary>
        /// Место принятия
        /// </summary>
        [ForeignKey("InitRegion")]
        [Column(name: "NsiInitRegionEntity_Id")]
        public Guid? InitRegionId { get; set; }
        public virtual NsiInitRegionEntity InitRegion { get; set; }

        /// <summary>
        /// Сфера правоотношений
        /// </summary>
        [ForeignKey("Classifier")]
        [Column(name: "NsiClassifierEntity_Id")]
        public Guid? ClassifierId { get; set; }
        public virtual NsiClassifierEntity Classifier { get; set; }

        /// <summary>
        /// Юридическая сила акта
        /// </summary>
        [ForeignKey("LawForce")]
        [Column(name: "NsiLawForceEntity_Id")]
        public Guid? LawForceId { get; set; }
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

        //[Column("DocumentBodyEntity_Id")]
        //public Guid? DocumentBodyId { get; set; }
        //public virtual DocumentBodyEntity Body { get; set; }

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
