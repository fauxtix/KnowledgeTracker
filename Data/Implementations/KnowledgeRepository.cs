using Dapper;
using KnowledgeTracker.Data.Interfaces;
using KnowledgeTracker.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace KnowledgeTracker.Data.Implementations
{
    public class KnowledgeRepository : IKnowledgeRepository
    {
        private readonly string _connectionString;

        public KnowledgeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<KnowledgeEntry>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<KnowledgeEntry>(
                "GetAllKnowledgeEntries", commandType: CommandType.StoredProcedure);
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QuerySingleOrDefaultAsync<KnowledgeEntry>(
                "sp_GetKnowledgeEntryById", new { Id = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(KnowledgeEntry entry)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.ExecuteScalarAsync<int>(
                "AddKnowledgeEntry",
                new
                {
                    entry.Title,
                    entry.Description,
                    entry.DateResolved,
                    entry.ProjectName,
                    entry.ResolutionSteps,
                    entry.Technologies,
                    entry.Tags,
                    entry.Comments
                },
                commandType: CommandType.StoredProcedure);
            return id;
        }

        public async Task UpdateAsync(KnowledgeEntry entry)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UpdateKnowledgeEntry",
                new
                {
                    entry.Id,
                    entry.Title,
                    entry.Description,
                    entry.DateResolved,
                    entry.ProjectName,
                    entry.ResolutionSteps,
                    entry.Technologies,
                    entry.Tags,
                    entry.Comments
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "DeleteKnowledgeEntry",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<KnowledgeEntry>> SearchAsync(string searchTerm)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<KnowledgeEntry>(
                "SearchKnowledgeEntries",
                new { SearchTerm = searchTerm },
                commandType: CommandType.StoredProcedure
            );
            return result ?? Enumerable.Empty<KnowledgeEntry>();
        }
    }
}
