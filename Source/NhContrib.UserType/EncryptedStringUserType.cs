using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Nuaguil.Security.EntLib;

namespace Nuaguil.NhContrib.UserType
{
    public class EncryptedStringUserType : IUserType
    {
        public new bool Equals(object obj1,object obj2)
        {
            return obj1.Equals(obj2);
        }

        #region IUserType Members

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public bool IsMutable
        {
            get { return false; } 
        }

        public object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner)
        {
            string cipherText = (string)NHibernateUtil.AnsiString.NullSafeGet(rs, names[0]);
            return CryptoProvider.Instance.Decrypt(cipherText);
        }

        public void NullSafeSet(System.Data.IDbCommand cmd, object value, int index)
        {
            
            if(value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
                return;
            }

            string cipherText = CryptoProvider.Instance.Encrypt(value.ToString());
            NHibernateUtil.String.NullSafeSet(cmd, cipherText, index); 
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public Type ReturnedType
        {
            get { return typeof (string); }
        }

        public SqlType[] SqlTypes
        {
            get
            {
                return new SqlType[] { new SqlType(DbType.String) };
            }
        }

        #endregion
    }
}
