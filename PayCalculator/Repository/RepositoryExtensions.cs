using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;

namespace PayCalculator.Repository
{
    public static class RepositoryExtensions
    {
        public static void OpenIfNecessary(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open &&
                connection.State != ConnectionState.Connecting)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Calls a stored procedure in the database.
        /// Neatens up Dapper's interface. Surely someone else must do this as a wrapper around Dapper?
        /// </summary>
        /// <typeparam name="T">The model type matching the database results</typeparam>
        /// <param name="connection"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="storedProcedureParameters"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection connection, string storedProcedureName,
            object storedProcedureParameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            connection.OpenIfNecessary();

            var command = new CommandDefinition(storedProcedureName, storedProcedureParameters, transaction,
                commandTimeout, CommandType.StoredProcedure);
            
            var result = await connection.QueryAsync<T>(command);
            return result;

        }

        public static async Task ExecuteAsync(this IDbConnection connection, string storedProcedureName,
            object storedProcedureParameters = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            connection.OpenIfNecessary();

            var command = new CommandDefinition(storedProcedureName, storedProcedureParameters, transaction,
                commandTimeout, CommandType.StoredProcedure);

            await connection.ExecuteAsync(command);
        }
    }
}