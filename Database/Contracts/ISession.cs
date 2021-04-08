using System;
using System.Data.Common;

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
        int ExecuteNonQuery(bool startTransaction);
        DbDataReader ExecuteReader();
        int ExecuteNonQuery();
    }
}
