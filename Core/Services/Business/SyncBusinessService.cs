using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Data.Dto;
using Core.Data.Entities.Nsi;
using Core.Enums;
using Core.Services.Managers;
using Microsoft.Extensions.Configuration;

namespace Core.Services.Business {
    public interface ISyncBusinessService {
        Task<SyncResultDto> Sync(NsiEnum nsiType);
    }

    public class SyncBusinessService: ISyncBusinessService {
        private readonly IConfiguration _configuration;

        private readonly INsiLanguageManager _nsiLanguagesManager;
        private readonly INsiDocumentStatusManager _nsiDocumentStatusesManager;
        private readonly INsiDocumentTypeManager _nsiDocumentTypeManager;
        private readonly INsiRegionManager _nsiRegionManager;
        private readonly INsiDevAgencyManager _nsiDevAgencyManager;
        private readonly INsiInitRegionManager _nsiInitRegionManager;
        private readonly INsiDocSectionManager _nsiDocSectionManager;
        private readonly INsiSourceManager _nsiSourceManager;
        private readonly INsiRegAgencyManager _nsiRegAgencyManager;
        private readonly INsiClassifierManager _nsiClassifierManager;
        private readonly INsiDepartmentManager _nsiDepartmentManager;
        private readonly INsiDocTitlePrefixManager _nsiDocTitlePrefixManager;

        public SyncBusinessService(IConfiguration configuration,
            INsiLanguageManager nsiLanguagesManager,
            INsiDocumentStatusManager nsiDocumentStatusesManager,
            INsiDocumentTypeManager nsiDocumentTypeManager,
            INsiRegionManager nsiRegionManager,
            INsiDevAgencyManager nsiDevAgencyManager,
            INsiInitRegionManager nsiInitRegionManager,
            INsiDocSectionManager nsiDocSectionManager,
            INsiSourceManager nsiSourceManager,
            INsiRegAgencyManager nsiRegAgencyManager,
            INsiClassifierManager nsiClassifierManager,
            INsiDepartmentManager nsiDepartmentManager,
            INsiDocTitlePrefixManager nsiDocTitlePrefixManager) {
            _configuration = configuration;

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
            _nsiDocTitlePrefixManager = nsiDocTitlePrefixManager;
        }

        public async Task<SyncResultDto> Sync(NsiEnum nsiType) {
            var sw = Stopwatch.StartNew();
            var result = new SyncResultDto(Enum.GetName(typeof(NsiEnum), nsiType));

            switch(nsiType) {
                case NsiEnum.Language:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Languages]", SyncNsiLanguage);
                    break;
                case NsiEnum.DocumentType:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocTypes]", SyncNsiDocumentType);
                    break;
                case NsiEnum.DocumentStatus:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocStatuses]", SyncNsiDocumentStatus);
                    break;
                case NsiEnum.Region:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Regions]", SyncNsiRegion);
                    break;
                case NsiEnum.DevAgency:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DevAgencies]", SyncNsiDevAgency);
                    break;
                case NsiEnum.InitRegion:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_InitRegions]", SyncNsiInitRegion);
                    break;
                case NsiEnum.DocSection:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocSections]", SyncNsiDocSection);
                    break;
                case NsiEnum.Source:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Sources]", SyncNsiSource);
                    break;
                case NsiEnum.RegAgency:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_RegAgencies]", SyncNsiRegAgency);
                    break;
                case NsiEnum.Classifier:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Classifier]", SyncNsiClassifier);
                    break;
                case NsiEnum.Department:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Departments]", SyncNsiDepartment);
                    break;
                case NsiEnum.DocTitlePrefix:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocTitlePrefixes]", SyncNsiDocTitlePrefix);
                    break;
                default:
                    break;
            }
            sw.Stop();
            result.LeadTime = sw.Elapsed;

            return result;
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
                    var item = _nsiDocTitlePrefixManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiDocTitlePrefixEntity {
                            Id = id,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,

                            CodeRu = reader["CodeRu"] as string,
                            CodeKk = reader["CodeKk"] as string,
                            CodeEn = reader["CodeEn"] as string
                        };
                    }
                    item = _nsiDocTitlePrefixManager.CreateOrUpdate(item).Result;
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
                    var item = _nsiDepartmentManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiDepartmentEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            ParentId = reader["ParentId"] != DBNull.Value ? reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : Guid.Empty : Guid.Empty
                        };
                    }
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
                    var item = _nsiClassifierManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiClassifierEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : Guid.Empty
                        };
                    }
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
                    var item = _nsiRegAgencyManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiRegAgencyEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : Guid.Empty
                        };
                    }
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
                    var item = _nsiSourceManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiSourceEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : Guid.Empty
                        };
                    }
                    item = _nsiSourceManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncSource: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncSource: " + e.Message);
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
                    var item = _nsiDocSectionManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiDocSectionEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string
                        };
                    }
                    item = _nsiDocSectionManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncDocSection: recordId {id}");
                    }
                }
            } catch(Exception e) {
                Console.WriteLine("SyncDocSection: " + e.Message);
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
                    var item = _nsiInitRegionManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiInitRegionEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : Guid.Empty,
                            //OldId = reader["OldId"] != DBNull.Value ? (int)reader["OldId"] : null
                        };
                    }
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
                    var item = _nsiDevAgencyManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiDevAgencyEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : Guid.Empty
                        };
                    }
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
                    var item = _nsiRegionManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiRegionEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            ComentRu = reader["ComentRu"] as string,
                            ComentKk = reader["ComentKk"] as string,
                            ComentEn = reader["ComentEn"] as string,
                            ParentId = reader["ParentId"] != DBNull.Value ? (Guid)reader["ParentId"] : Guid.Empty,
                        };
                    }
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
                    var item = _nsiDocumentTypeManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiDocumentTypeEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                        };
                    }
                    item = _nsiDocumentTypeManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiLanguage: recordId {id}");
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
                    var item = _nsiDocumentStatusesManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiDocumentStatusEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            CodeBd7 = reader["CodeBd7"] as string
                        };
                    }
                    item = _nsiDocumentStatusesManager.CreateOrUpdate(item).Result;
                    if(item == null) {
                        Console.Error.WriteLine($"SyncNsiLanguage: recordId {id}");
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

                    var item = _nsiLanguagesManager.Find(id).Result;
                    if(item == null) {
                        item = new NsiLanguageEntity {
                            Id = id,
                            Code = reader["Code"] as string,
                            NameRu = reader["NameRu"] as string,
                            NameKk = reader["NameKk"] as string,
                            NameEn = reader["NameEn"] as string,
                            Sort = (int)reader["Sort"],
                            CodeImport = reader["CodeImport"] as string
                        };
                    }
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

        private async Task<int> RunRemoteConnection(string query, Func<SqlDataReader, int> myMethodName) {
            using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("RemoteConnection"))) {
                using(var command = new SqlCommand(query, connection)) {
                    await connection.OpenAsync();
                    using(var reader = await command.ExecuteReaderAsync()) {
                        return myMethodName(reader);
                    }
                }
            }
        }
    }
}
