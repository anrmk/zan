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

        public SyncBusinessService(IConfiguration configuration,
            INsiLanguageManager nsiLanguagesManager,
            INsiDocumentStatusManager nsiDocumentStatusesManager,
            INsiDocumentTypeManager nsiDocumentTypeManager,
            INsiRegionManager nsiRegionManager) {
            _configuration = configuration;

            _nsiLanguagesManager = nsiLanguagesManager;
            _nsiDocumentStatusesManager = nsiDocumentStatusesManager;
            _nsiDocumentTypeManager = nsiDocumentTypeManager;
            _nsiRegionManager = nsiRegionManager;
        }

        public async Task<SyncResultDto> Sync(NsiEnum nsiType) {
            var sw = Stopwatch.StartNew();
            var result = new SyncResultDto(Enum.GetName(typeof(NsiEnum), nsiType));

            switch(nsiType) {
                case NsiEnum.Language:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Languages]", SyncNsiLanguage);
                    break;
                case NsiEnum.Region:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_Regions]", SyncNsiRegion);
                    break;
                case NsiEnum.DocumentStatus:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocStatuses]", SyncNsiDocumentStatus);
                    break;
                case NsiEnum.DocumentType:
                    result.Count = await RunRemoteConnection("SELECT * FROM [nsi_DocTypes]", SyncNsiDocumentType);
                    break;
                default:
                    break;
            }
            sw.Stop();
            result.LeadTime = sw.Elapsed;

            return result;
        }

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
                            ParentId = (Guid)reader["ParentId"],
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
