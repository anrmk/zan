using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

using Core.Data.Dto;
using Core.Data.Entities.Documents;
using Core.Data.Entities.Nsi;
using Core.Enums;
using Core.Services.Managers;

using Microsoft.Extensions.Configuration;

namespace Core.Services.Business {
    public interface ISyncBusinessService {
        Task<SyncResultDto> Sync(SyncCommandEnum nsiType);
    }

    public class SyncBusinessService: ISyncBusinessService {
        private readonly IConfiguration _configuration;

        private readonly IDocumentManager _documentManager;
        private readonly IDocumentBodyManager _documentBodyManager;

        private readonly INsiLanguageManager _nsiLanguagesManager;
        private readonly INsiDocumentStatusManager _nsiDocumentStatusesManager;
        private readonly INsiDocumentTypeManager _nsiDocumentTypeManager;
        private readonly INsiRegionManager _nsiRegionManager;
        private readonly INsiDevAgencyManager _nsiDevAgencyManager;
        private readonly INsiInitRegionManager _nsiInitRegionManager;
        private readonly INsiDocumentSectionManager _nsiDocSectionManager;
        private readonly INsiSourceManager _nsiSourceManager;
        private readonly INsiRegAgencyManager _nsiRegAgencyManager;
        private readonly INsiClassifierManager _nsiClassifierManager;
        private readonly INsiDepartmentManager _nsiDepartmentManager;
        private readonly INsiDocumentTitlePrefixManager _nsiDocumentTitlePrefixManager;
        private readonly INsiLawForceManager _nsiLawForceManager;
        private readonly INsiGrifTypeManager _nsiGrifTypeManager;

        public SyncBusinessService(IConfiguration configuration,
            IDocumentManager documentManager,
            IDocumentBodyManager documentBodyManager,
            INsiLanguageManager nsiLanguagesManager,
            INsiDocumentStatusManager nsiDocumentStatusesManager,
            INsiDocumentTypeManager nsiDocumentTypeManager,
            INsiRegionManager nsiRegionManager,
            INsiDevAgencyManager nsiDevAgencyManager,
            INsiInitRegionManager nsiInitRegionManager,
            INsiDocumentSectionManager nsiDocSectionManager,
            INsiSourceManager nsiSourceManager,
            INsiRegAgencyManager nsiRegAgencyManager,
            INsiClassifierManager nsiClassifierManager,
            INsiDepartmentManager nsiDepartmentManager,
            INsiDocumentTitlePrefixManager nsiDocumentTitlePrefixManager,
            INsiLawForceManager nsiLawForceManager,
            INsiGrifTypeManager nsiGrifTypeManager) {
            _configuration = configuration;
            _documentManager = documentManager;
            _documentBodyManager = documentBodyManager;

            _nsiLanguagesManager = nsiLanguagesManager;
            _nsiDocumentStatusesManager = nsiDocumentStatusesManager;
            _nsiDocumentTypeManager = nsiDocumentTypeManager;
            _nsiRegionManager = nsiRegionManager;
            _nsiDevAgencyManager = nsiDevAgencyManager;
            _nsiInitRegionManager = nsiInitRegionManager;
            _nsiDocSectionManager = nsiDocSectionManager;
            _nsiSourceManager = nsiSourceManager;
            _nsiRegAgencyManager = nsiRegAgencyManager;
            _nsiClassifierManager = nsiClassifierManager;
            _nsiDepartmentManager = nsiDepartmentManager;
            _nsiDocumentTitlePrefixManager = nsiDocumentTitlePrefixManager;
            _nsiLawForceManager = nsiLawForceManager;
            _nsiGrifTypeManager = nsiGrifTypeManager;
        }

        public async Task<SyncResultDto> Sync(SyncCommandEnum nsiType) {
            var sw = Stopwatch.StartNew();
            var result = new SyncResultDto(Enum.GetName(typeof(SyncCommandEnum), nsiType));

            switch(nsiType) {
                case SyncCommandEnum.Language:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Languages]", SyncNsiLanguage);
                    break;
                case SyncCommandEnum.DocumentType:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocTypes]", SyncNsiDocumentType);
                    break;
                case SyncCommandEnum.DocumentStatus:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocStatuses]", SyncNsiDocumentStatus);
                    break;
                case SyncCommandEnum.Region:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Regions]", SyncNsiRegion);
                    break;
                case SyncCommandEnum.DevAgency:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DevAgencies]", SyncNsiDevAgency);
                    break;
                case SyncCommandEnum.InitRegion:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_InitRegions]", SyncNsiInitRegion);
                    break;
                case SyncCommandEnum.DocSection:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocSections]", SyncNsiDocSection);
                    break;
                case SyncCommandEnum.Source:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Sources]", SyncNsiSource);
                    break;
                case SyncCommandEnum.RegAgency:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_RegAgencies]", SyncNsiRegAgency);
                    break;
                case SyncCommandEnum.Classifier:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Classifier]", SyncNsiClassifier);
                    break;
                case SyncCommandEnum.Department:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Departments]", SyncNsiDepartment);
                    break;
                case SyncCommandEnum.DocTitlePrefix:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocTitlePrefixes]", SyncNsiDocTitlePrefix);
                    break;
                case SyncCommandEnum.LawForce:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_LawForces]", SyncNsiLawForce);
                    break;
                case SyncCommandEnum.GrifType:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_GrifTypes]", SyncNsiGrifType);
                    break;
                case SyncCommandEnum.Document:
                    result.Count = await RunRemoteConnection("SELECT * FROM [dyn_Documents]", SyncDocuments);
                    break;
                case SyncCommandEnum.DocumentBody:
                    result.Count = await RunRemoteConnection("SELECT TOP(100) DOC.* , BODY.Id as BodyId, Body.Body as Content FROM[dyn_Documents] as DOC INNER JOIN[dyn_DocumentBodies] as BODY ON DOC.[Id] = BODY.[DocumentId] WHERE DOC.[Price] IS NULL", SyncDocumentBodies);
                    break;
                default:
                    break;
            }
            sw.Stop();
            result.LeadTime = sw.Elapsed;

            return result;
        }

        public int SyncDocumentBodies(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var documentId = (Guid)reader["DocumentId"];

                    var item = _documentBodyManager.Find(id).Result ?? new DocumentBodyEntity();

                    item.Id = id;
                    item.DocumentId = documentId;
                    item.Body = reader["Body"] as string;

                    item = _documentBodyManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncDocumentBodies: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncDocuments: " + e.Message);
            }

            return count;
        }

        public int SyncDocuments(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _documentManager.Find(id).Result ?? new DocumentEntity();

                    #region DOCUMENT
                    item.Id = id;
                    item.Ngr = reader["Ngr"] as string;
                    item.GosNumber = reader["GosNumber"] != DBNull.Value ? (int)reader["GosNumber"] : (int?)null;
                    item.AcceptNumber = reader["AcceptNumber"] as string;
                    item.RegNumber = reader["RegNumber"] as string;
                    item.Title = reader["Title"] as string;
                    item.Info = reader["Info"] as string;

                    item.StatusId = reader["StatusId"] != DBNull.Value ? (int)reader["StatusId"] : (int?)null;
                    item.SectionId = reader["SectionId"] != DBNull.Value ? (Guid)reader["SectionId"] : (Guid?)null;
                    item.LanguageId = reader["LanguageId"] != DBNull.Value ? (int)reader["LanguageId"] : (int?)null;
                    item.DevAgencyId = reader["DevAgencyId"] != DBNull.Value ? (Guid)reader["DevAgencyId"] : (Guid?)null;
                    item.RegionActionId = reader["RegionActionId"] != DBNull.Value ? (Guid)reader["RegionActionId"] : (Guid?)null;
                    item.RegAgencyId = reader["RegAgencyId"] != DBNull.Value ? (Guid)reader["RegAgencyId"] : (Guid?)null;
                    item.AcceptedRegionId = reader["AcceptedRegionId"] != DBNull.Value ? (Guid)reader["AcceptedRegionId"] : (Guid?)null;
                    item.InitRegionId = reader["InitRegionId"] != DBNull.Value ? (Guid)reader["InitRegionId"] : (Guid?)null;
                    item.ClassifierId = reader["ClassifierId"] != DBNull.Value ? (Guid)reader["ClassifierId"] : (Guid?)null;
                    item.LawForceId = reader["LawForfceId"] != DBNull.Value ? (Guid)reader["LawForfceId"] : (Guid?)null;
                    item.AcceptedDate = reader["AcceptedDate"] != DBNull.Value ? (DateTime)reader["AcceptedDate"] : (DateTime?)null;
                    item.EditionDate = (DateTime)reader["EditionDate"];
                    item.EntryDate = reader["EntryDate"] != DBNull.Value ? (DateTime)reader["EntryDate"] : (DateTime?)null;
                    item.RegJustDate = reader["RegJustDate"] != DBNull.Value ? (DateTime)reader["RegJustDate"] : (DateTime?)null;
                    item.RegSystDate = reader["RegSystDate"] != DBNull.Value ? (DateTime)reader["RegSystDate"] : (DateTime?)null;
                    item.PublishedDate = reader["PublishedDate"] != DBNull.Value ? (DateTime)reader["PublishedDate"] : (DateTime?)null;
                    item.PrintDepartment = reader["PrintDepartment"] as string;
                    item.IssetOtherEditions = reader["IssetOtherEditions"] != DBNull.Value ? (bool)reader["IssetOtherEditions"] : false;
                    item.IsArchive = reader["IsArchive"] != DBNull.Value ? (bool)reader["IsArchive"] : false;
                    #endregion

                    item = _documentManager.CreateOrUpdate(item).Result;

                    var bodyItem = _documentBodyManager.FindByDocumentIdAsync(id).Result;
                    bodyItem.Id = (Guid)reader["BodyId"];
                    bodyItem.DocumentId = id;
                    bodyItem.Body = reader["Content"] as string;

                    bodyItem = _documentBodyManager.CreateOrUpdate(bodyItem).Result;

                    if(bodyItem == null) {
                        Console.Error.WriteLine($"SyncDocumentBodis: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncDocuments: " + e.Message);
            }

            return count;
        }

        #region NSI
        /// <summary>
        /// Гриф
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public int SyncNsiGrifType(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiGrifTypeManager.Find(id).Result ?? new NsiGrifTypeEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;

                    item = _nsiGrifTypeManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiGrifType: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiGrifType: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Юридическая сила акта
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public int SyncNsiLawForce(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiLawForceManager.Find(id).Result ?? new NsiLawForceEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;

                    item = _nsiLawForceManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiLawForce: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiLawForce: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Кроме актов
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiDocTitlePrefix(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiDocumentTitlePrefixManager.Find(id).Result ?? new NsiDocumentTitlePrefixEntity();

                    item.Id = id;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.CodeRu = reader["CodeRu"] as string;
                    item.CodeKk = reader["CodeKk"] as string;
                    item.CodeEn = reader["CodeEn"] as string;

                    item = _nsiDocumentTitlePrefixManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiDocTitlePrefix: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiDocTitlePrefix: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Ведомства
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiDepartment(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiDepartmentManager.Find(id).Result ?? new NsiDepartmentEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : (Guid?)null;

                    item = _nsiDepartmentManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiDepartment: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiDepartment: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Сфера правоотношений
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiClassifier(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiClassifierManager.Find(id).Result ?? new NsiClassifierEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : (Guid?)null;

                    item = _nsiClassifierManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiClassifier: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiClassifier: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Орган госрегистрации
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiRegAgency(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiRegAgencyManager.Find(id).Result ?? new NsiRegAgencyEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : (Guid?)null;

                    item = _nsiRegAgencyManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiRegAgency: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiRegAgency: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Источник публикации
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiSource(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiSourceManager.Find(id).Result ?? new NsiSourceEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : (Guid?)null;

                    item = _nsiSourceManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiSource: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiSource: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Раздел законодательства
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiDocSection(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiDocSectionManager.Find(id).Result ?? new NsiDocumentSectionEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;

                    item = _nsiDocSectionManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiDocSection: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiDocSection: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Место принятия
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiInitRegion(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiInitRegionManager.Find(id).Result ?? new NsiInitRegionEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : (Guid?)null;

                    item = _nsiInitRegionManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiInitRegion: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiInitRegion: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Орган разработчик
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiDevAgency(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiDevAgencyManager.Find(id).Result ?? new NsiDevAgencyEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : (Guid?)null;

                    item = _nsiDevAgencyManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiDevAgency: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiDevAgency: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Регион действия
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiRegion(SqlDataReader reader) {
            var count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiRegionManager.Find(id).Result ?? new NsiRegionEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.ComentRu = reader["ComentRu"] as string;
                    item.ComentKk = reader["ComentKk"] as string;
                    item.ComentEn = reader["ComentEn"] as string;
                    item.ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : (Guid?)null;

                    item = _nsiRegionManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiLanguage: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiRegion: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Справочник типов документа (форма акта)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiDocumentType(SqlDataReader reader) {
            int count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (Guid)reader["Id"];
                    var item = _nsiDocumentTypeManager.Find(id).Result ?? new NsiDocumentTypeEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;

                    item = _nsiDocumentTypeManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiDocumentType: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiDocumentType: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Справочник статусов документа
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiDocumentStatus(SqlDataReader reader) {
            int count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (int)reader["Id"];
                    var item = _nsiDocumentStatusesManager.Find(id).Result ?? new NsiDocumentStatusEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.CodeBd7 = reader["CodeBd7"] as string;

                    item = _nsiDocumentStatusesManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiDocumentStatus: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiDocumentStatus: " + e.Message);
            }

            return count;
        }

        /// <summary>
        /// Справочник языков
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected int SyncNsiLanguage(SqlDataReader reader) {
            int count = 0;
            try {
                while(reader.Read()) {
                    count++;
                    var id = (int)reader["Id"];
                    var item = _nsiLanguagesManager.Find(id).Result ?? new NsiLanguageEntity();

                    item.Id = id;
                    item.Code = reader["Code"] as string;
                    item.NameRu = reader["NameRu"] as string;
                    item.NameKk = reader["NameKk"] as string;
                    item.NameEn = reader["NameEn"] as string;
                    item.Sort = (int)reader["Sort"];
                    item.CodeImport = reader["CodeImport"] as string;

                    item = _nsiLanguagesManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiLanguage: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncNsiLanguage: " + e.Message);
            }

            return count;
        }
        #endregion

        private async Task<int> RunRemoteConnection(string query, Func<SqlDataReader, int> myMethodName) {
            using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("RemoteConnection"))) {
                // var query = $"SELECT COUNT(*) FROM {tableName}";
                using(var command = new SqlCommand(query, connection)) {
                    await connection.OpenAsync();
                    using(var reader = await command.ExecuteReaderAsync()) {
                        return myMethodName(reader);
                    }
                }
            }
            //var count = await GetCountAsync(query);
            //if(count > 0) {
            //    var limit = 1000;
            //    var pages = count / limit;
            //    for(var i = 0; i < pages; i++) {
            //        var offset = i * limit;
            //        using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("RemoteConnection"))) {
            //            var cmd = new SqlCommand($"SELECT * FROM {query} ORDER BY Id OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY; ");
            //            await connection.OpenAsync();

            //            using(var command = new SqlCommand(query, connection)) {
            //                await connection.OpenAsync();
            //                using(var reader = await command.ExecuteReaderAsync()) {
            //                    return myMethodName(reader);
            //                }
            //            }
            //        }
            //    }
            //}
            //return 0;
        }

        private async Task<int> GetCountAsync(string tableName) {
            using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("RemoteConnection"))) {
                var query = $"SELECT COUNT(*) FROM {tableName}";
                using(var command = new SqlCommand(query, connection)) {
                    await connection.OpenAsync();
                    using(var reader = await command.ExecuteReaderAsync()) {
                        return (int)reader[0];
                    }
                }
            }
        }
    }
}
