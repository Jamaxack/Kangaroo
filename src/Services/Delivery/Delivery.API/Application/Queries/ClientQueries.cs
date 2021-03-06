﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Delivery.API.Application.Queries.ViewModels;
using Microsoft.Data.SqlClient;

namespace Delivery.API.Application.Queries
{
    public class ClientQueries : IClientQueries
    {
        #region SelectClientsQuery

        private const string SelectClientsQuery =
            @"SELECT Id,
                FirstName,
                LastName,
                Phone
              FROM Delivery.Clients";

        #endregion

        private readonly string _connectionString = string.Empty;

        public ClientQueries(string connectionString)
        {
            _connectionString = string.IsNullOrWhiteSpace(connectionString)
                ? throw new ArgumentNullException(nameof(connectionString))
                : connectionString;
        }

        public async Task<ClientViewModel> GetClientByIdAsync(Guid clientId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var queryString = $"{SelectClientsQuery} WHERE Id = @ClientId;";
                var clientDynamic = await connection.QueryFirstOrDefaultAsync(queryString, new {clientId});

                if (clientDynamic == null)
                    throw new KeyNotFoundException($"Client with specified Id not found: {clientId}");

                return MapToClientViewModel(clientDynamic);
            }
        }

        public async Task<List<ClientViewModel>> GetClientsAsync(int pageSize, int pageIndex)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var offset = pageSize * pageIndex;
                var pagedQuery =
                    $"{SelectClientsQuery} {@"Order BY FirstName, LastName OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY"}";
                var queryResult = await connection.QueryAsync(pagedQuery, new {offset, pageSize});

                var clients = new List<ClientViewModel>();
                foreach (var client in queryResult)
                    clients.Add(MapToClientViewModel(client));

                return clients;
            }
        }

        private ClientViewModel MapToClientViewModel(dynamic queryResult)
        {
            return new ClientViewModel
            {
                Id = queryResult.Id,
                Phone = queryResult.Phone,
                FirstName = queryResult.FirstName,
                LastName = queryResult.LastName
            };
        }
    }
}