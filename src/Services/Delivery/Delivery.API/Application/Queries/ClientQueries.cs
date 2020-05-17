using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delivery.API.Application.Queries
{
    public class ClientQueries : IClientQueries
    {
        readonly string _connectionString = string.Empty;

        #region SelectClientsQuery
        const string SelectClientsQuery =
            @"SELECT Id,
                FirstName,
                LastName,
                Phone
              FROM Delivery.Clients";
        #endregion

        public ClientQueries(string connectionString)
        {
            _connectionString = string.IsNullOrWhiteSpace(connectionString) ? throw new ArgumentNullException(nameof(connectionString)) : connectionString;
        }

        public async Task<ClientViewModel> GetClientByIdAsync(Guid clientId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = $"{SelectClientsQuery} WHERE Id = @ClientId;";
                var clientDynamic = await connection.QueryFirstOrDefaultAsync(queryString, new { clientId });

                if (clientDynamic == null)
                    throw new KeyNotFoundException($"Client with specified Id not found: {clientId}");

                return MapToClientViewModel(clientDynamic);
            }
        }

        ClientViewModel MapToClientViewModel(dynamic queryResult)
        {
            return new ClientViewModel()
            {
                Id = queryResult.Id,
                Phone = queryResult.Phone,
                FirstName = queryResult.FirstName,
                LastName = queryResult.LastName
            };
        }
    }
}
