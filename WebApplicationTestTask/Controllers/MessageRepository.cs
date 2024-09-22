using Microsoft.AspNetCore.Mvc;
using Dapper;
using Npgsql;
using WebApplicationTestTask.Models;

namespace WebApplicationTestTask.Controllers
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to);
    }

    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(IConfiguration configuration, ILogger<MessageRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public async Task AddMessageAsync(Message message)
        {
            var sql = "INSERT INTO messages (text, timestamp, \"order\") VALUES (@Text, @Timestamp, @Order)";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    _logger.LogInformation("Executing SQL:{sql}", sql);
                    await connection.ExecuteAsync(sql, message);
                }
                catch 
                {
                    _logger.LogError("Error when executing SQL:{sql}", sql);
                }
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to)
        {
            var sql = "SELECT * FROM messages WHERE timestamp BETWEEN @From AND @To ORDER BY timestamp";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    _logger.LogInformation("Executing SQL:{sql}", sql);
                    return await connection.QueryAsync<Message>(sql, new { From = from, To = to });
                }
                catch
                {
                    _logger.LogError("Error when executing SQL:{sql}", sql);
                    return Enumerable.Empty<Message>();
                }
            }
        }
    }
}
