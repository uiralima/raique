using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Raique.Database.Contracts
{
    public interface ISession : IDisposable
    {
        void Invalidate();
        ISession SetQuery(string query);
        ISession SetType(System.Data.CommandType type);
        ISession AddParameter(string name, object value);
        ISession ClearParameters();
        DbDataReader ExecuteReader(bool startTransaction);
        DbDataReader ExecuteReader();
        Task<DbDataReader> ExecuteReaderAsync(bool startTransaction);
        Task<DbDataReader> ExecuteReaderAsync();
        int ExecuteNonQuery(bool startTransaction);
        Task<int> ExecuteNonQueryAsync(bool startTransaction);
        Task<int> ExecuteNonQueryAsync();
        int ExecuteNonQuery();
    }
}
