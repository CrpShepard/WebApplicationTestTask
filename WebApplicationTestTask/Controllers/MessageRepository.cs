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

        public MessageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddMessageAsync(Message message)
        {
            var sql = "INSERT INTO messages (text, timestamp, order) VALUES (@Text, @Timestamp, @Order)";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, message);
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(DateTime from, DateTime to)
        {
            var sql = "SELECT * FROM messages WHERE timestamp BETWEEN @From AND @To ORDER BY timestamp";
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Message>(sql, new { From = from, To = to });
            }
        }
    }
}
