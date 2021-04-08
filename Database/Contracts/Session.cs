using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Raique.Database.Contracts
{
    public abstract class Session : ISession
    {
        private string _query = String.Empty;
        private List<DbParameter> _parameters = new List<DbParameter>();
        System.Data.CommandType _type = System.Data.CommandType.Text;
        private DbConnection _conn;
        private DbCommand _command;
        private DbTransaction _transaction;
        bool _isOk = true;

        protected Session(string connectionString)
        {
            _conn = GetConnection();
            _conn.ConnectionString = connectionString;
        }

        protected abstract DbConnection GetConnection();
        protected abstract DbCommand GetCommand();
        protected abstract DbParameter GetParameter(string name, object value);

        public virtual void Invalidate()
        {
            _isOk = false;
        }

        public virtual ISession SetQuery(string query)
        {
            this._command = GetCommand();
            this._command.Connection = _conn;
            this._command.CommandText = query;
            return this;
        }

        public virtual ISession SetProcedure(string procedureName)
        {
            this._command = GetCommand();
            this._command.Connection = _conn;
            this._command.CommandText = procedureName;
            this._type = System.Data.CommandType.StoredProcedure;
            return this;
        }

        public virtual ISession ClearParameters()
        {
            this._parameters.Clear();
            return this;
        }

        public ISession SetType(System.Data.CommandType type)
        {
            this._type = type;
            return this;
        }

        public virtual ISession AddParameter(string name, object value)
        {
            this._parameters.Add(GetParameter(name, value));
            return this;
        }

        public virtual DbDataReader ExecuteReader(bool startTransaction)
        {
            this._command.CommandType = this._type;
            this._command.Parameters.AddRange(this._parameters.ToArray());
            if (this._conn.State != System.Data.ConnectionState.Open)
            {
                this._conn.Open();
            }
            if (startTransaction)
            {
                this._transaction = this._conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            }
            if (null != this._transaction)
            {
                this._command.Transaction = this._transaction;
            }
            try
            {
                return this._command.ExecuteReader();
            }
            catch (Exception ex)
            {
                _isOk = false;
                throw ex;
            }
        }

        public virtual int ExecuteNonQuery(bool startTransaction)
        {
            this._command.CommandType = this._type;
            this._command.Parameters.AddRange(this._parameters.ToArray());
            if (this._conn.State != System.Data.ConnectionState.Open)
            {
                this._conn.Open();
            }
            if (startTransaction)
            {
                this._transaction = this._conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            }
            if (null != this._transaction)
            {
                this._command.Transaction = this._transaction;
            }
            try
            {
                return this._command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _isOk = false;
                throw ex;
            }
        }

        public virtual void Dispose()
        {
            if (null != this._transaction)
            {
                if (_isOk)
                {
                    this._transaction.Commit();
                }
                else
                {
                    this._transaction.Rollback();
                }
            }
            this._command?.Dispose();
            this._conn?.Close();
            this._conn?.Dispose();
        }

        public virtual DbDataReader ExecuteReader()
        {
            return ExecuteReader(true);
        }

        public virtual int ExecuteNonQuery()
        {
            return ExecuteNonQuery(true);
        }
    }
}
