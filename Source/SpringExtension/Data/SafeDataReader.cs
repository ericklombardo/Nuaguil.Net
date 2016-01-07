using System;
using System.Data;

namespace Nuaguil.SpringExt.Data
{
    public class SafeDataReader : IDataReader
    {
        private readonly IDataReader _dataReader;
        private bool _disposedValue;

        protected IDataReader DataReader
        {
            get
            {
                return _dataReader;
            }
        }

        public int Depth
        {
            get
            {
                return _dataReader.Depth;
            }
        }

        public int FieldCount
        {
            get
            {
                return _dataReader.FieldCount;
            }
        }

        public bool IsClosed
        {
            get
            {
                return _dataReader.IsClosed;
            }
        }

        public object this[string name]
        {
            get
            {
                object obj = _dataReader[name];
                if (DBNull.Value.Equals(obj))
                    return null;
                return obj;
            }
        }

        public virtual object this[int i]
        {
            get
            {
                if (_dataReader.IsDBNull(i))
                    return null;
                return _dataReader[i];
            }
        }

        public int RecordsAffected
        {
            get
            {
                return _dataReader.RecordsAffected;
            }
        }

        public SafeDataReader(IDataReader dataReader)
        {
            _dataReader = dataReader;
        }

        ~SafeDataReader()
        {
            Dispose(false);
        }

        public string GetString(string name)
        {
            return GetString(_dataReader.GetOrdinal(name));
        }

        public virtual string GetString(int i)
        {
            if (_dataReader.IsDBNull(i))
                return string.Empty;
            return _dataReader.GetString(i);
        }

        public object GetValue(string name)
        {
            return GetValue(_dataReader.GetOrdinal(name));
        }

        public virtual object GetValue(int i)
        {
            if (_dataReader.IsDBNull(i))
                return null;
            return _dataReader.GetValue(i);
        }

        public int GetInt32(string name)
        {
            return GetInt32(_dataReader.GetOrdinal(name));
        }

        public virtual int GetInt32(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            return _dataReader.GetInt32(i);
        }

        public double GetDouble(string name)
        {
            return GetDouble(_dataReader.GetOrdinal(name));
        }

        public virtual double GetDouble(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0.0;
            return _dataReader.GetDouble(i);
        }

        public Guid GetGuid(string name)
        {
            return GetGuid(_dataReader.GetOrdinal(name));
        }

        public virtual Guid GetGuid(int i)
        {
            if (_dataReader.IsDBNull(i))
                return Guid.Empty;
            return _dataReader.GetGuid(i);
        }

        public bool Read()
        {
            return _dataReader.Read();
        }

        public bool NextResult()
        {
            return _dataReader.NextResult();
        }

        public void Close()
        {
            _dataReader.Close();
        }

        public bool GetBoolean(string name)
        {
            return GetBoolean(_dataReader.GetOrdinal(name));
        }

        public virtual bool GetBoolean(int i)
        {
            if (_dataReader.IsDBNull(i))
                return false;
            return _dataReader.GetBoolean(i);
        }

        public byte GetByte(string name)
        {
            return GetByte(_dataReader.GetOrdinal(name));
        }

        public virtual byte GetByte(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            return _dataReader.GetByte(i);
        }

        public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            return GetBytes(_dataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferOffset, int length)
        {
            if (_dataReader.IsDBNull(i))
                return 0L;
            return _dataReader.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
        }

        public char GetChar(string name)
        {
            return GetChar(_dataReader.GetOrdinal(name));
        }

        public virtual char GetChar(int i)
        {
            if (_dataReader.IsDBNull(i))
                return char.MinValue;
            char[] buffer = new char[1];
            _dataReader.GetChars(i, 0L, buffer, 0, 1);
            return buffer[0];
        }

        public long GetChars(string name, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            return GetChars(_dataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        public virtual long GetChars(int i, long fieldOffset, char[] buffer, int bufferOffset, int length)
        {
            if (_dataReader.IsDBNull(i))
                return 0L;
            return _dataReader.GetChars(i, fieldOffset, buffer, bufferOffset, length);
        }

        public IDataReader GetData(string name)
        {
            return GetData(_dataReader.GetOrdinal(name));
        }

        public virtual IDataReader GetData(int i)
        {
            return _dataReader.GetData(i);
        }

        public string GetDataTypeName(string name)
        {
            return GetDataTypeName(_dataReader.GetOrdinal(name));
        }

        public virtual string GetDataTypeName(int i)
        {
            return _dataReader.GetDataTypeName(i);
        }

        public virtual DateTime GetDateTime(string name)
        {
            return GetDateTime(_dataReader.GetOrdinal(name));
        }

        public virtual DateTime GetDateTime(int i)
        {
            if (_dataReader.IsDBNull(i))
                return DateTime.MinValue;
            return _dataReader.GetDateTime(i);
        }

        public Decimal GetDecimal(string name)
        {
            return GetDecimal(_dataReader.GetOrdinal(name));
        }

        public virtual Decimal GetDecimal(int i)
        {
            if (_dataReader.IsDBNull(i))
                return new Decimal(0);
            return _dataReader.GetDecimal(i);
        }

        public Type GetFieldType(string name)
        {
            return GetFieldType(_dataReader.GetOrdinal(name));
        }

        public virtual Type GetFieldType(int i)
        {
            return _dataReader.GetFieldType(i);
        }

        public float GetFloat(string name)
        {
            return GetFloat(_dataReader.GetOrdinal(name));
        }

        public virtual float GetFloat(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0.0f;
            return _dataReader.GetFloat(i);
        }

        public short GetInt16(string name)
        {
            return GetInt16(_dataReader.GetOrdinal(name));
        }

        public virtual short GetInt16(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0;
            return _dataReader.GetInt16(i);
        }

        public long GetInt64(string name)
        {
            return GetInt64(_dataReader.GetOrdinal(name));
        }

        public virtual long GetInt64(int i)
        {
            if (_dataReader.IsDBNull(i))
                return 0L;
            return _dataReader.GetInt64(i);
        }

        public virtual string GetName(int i)
        {
            return _dataReader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return _dataReader.GetOrdinal(name);
        }

        public DataTable GetSchemaTable()
        {
            return _dataReader.GetSchemaTable();
        }

        public int GetValues(object[] values)
        {
            return _dataReader.GetValues(values);
        }

        public virtual bool IsDBNull(int i)
        {
            return _dataReader.IsDBNull(i);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue && disposing)
                _dataReader.Dispose();
            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
