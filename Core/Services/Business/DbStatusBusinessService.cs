﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Core.Data.Dto.DbStatus;
using Microsoft.Extensions.Configuration;

namespace Core.Services.Business {
    public interface IDbStatusBusinessService {
        Task<List<DbStatusDocumentDto>> GetDbDocumentStatistics();
    }
    public class DbStatusBusinessService: IDbStatusBusinessService {
        private readonly IConfiguration _configuration;
        public DbStatusBusinessService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<List<DbStatusDocumentDto>> GetDbDocumentStatistics() {
            using(SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))) {
                var query = $"SELECT LANG.[NameEn] as [Name], COUNT(DOC.[NsiLanguageEntity_Id]) AS [Value] " +
                           "FROM [Documents] DOC " +
                           "INNER JOIN[nsi.Languages] LANG ON DOC.NsiLanguageEntity_Id = LANG.Id " +
                           "GROUP BY DOC.NsiLanguageEntity_Id, LANG.[NameEn] " +
                           "UNION ALL SELECT 'Total' [Name], COUNT(*) FROM [Documents] " +
                           "UNION ALL SELECT 'Archive' [Name], COUNT(*) FROM [Documents] WHERE IsArchive = 1";
                using(var command = new SqlCommand(query, connection)) {
                    await connection.OpenAsync();
                    using(var reader = await command.ExecuteReaderAsync()) {
                        List<DbStatusDocumentDto> list = new List<DbStatusDocumentDto>();
                        while(reader.Read()) {
                            // count++;
                            var name = reader["Name"] as string;
                            var value = (int)reader["Value"];
                            list.Add(new DbStatusDocumentDto() {
                                Name = name,
                                Value = value
                            });
                        }
                        return list;
                    }
                }
            }
        }
    }
}
