namespace Common.Web.MVC.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public sealed class JsonWriter
    {
        private Stack<Scope> _scopes;
        private Common.Web.MVC.Serialization.IndentedTextWriter _writer;
        internal static readonly DateTime MinDate;
        internal static readonly long MinDateTimeTicks;

        static JsonWriter()
        {
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0);
            MinDateTimeTicks = time.Ticks;
            MinDate = new DateTime(100, 1, 1, 0, 0, 0);
        }

        public JsonWriter(TextWriter writer) : this(writer, true)
        {
        }

        public JsonWriter(TextWriter writer, bool minimizeWhitespace)
        {
            this._writer = new Common.Web.MVC.Serialization.IndentedTextWriter(writer, minimizeWhitespace);
            this._scopes = new Stack<Scope>();
        }

        public void EndScope()
        {
            if (this._scopes.Count == 0)
            {
                throw new InvalidOperationException("No active scope to end.");
            }
            this._writer.WriteLine();
            this._writer.Indent--;
            if (this._scopes.Pop().Type == ScopeType.Array)
            {
                this._writer.Write("]");
            }
            else
            {
                this._writer.Write("}");
            }
        }

        private static string QuoteJScriptString(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            StringBuilder builder = null;
            int startIndex = 0;
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if ((((ch == '\r') || (ch == '\t')) || ((ch == '"') || (ch == '\''))) || (((ch == '\\') || (ch == '\r')) || ((ch < ' ') || (ch > '\x007f'))))
                {
                    if (builder == null)
                    {
                        builder = new StringBuilder(s.Length + 6);
                    }
                    if (count > 0)
                    {
                        builder.Append(s, startIndex, count);
                    }
                    startIndex = i + 1;
                    count = 0;
                }
                switch (ch)
                {
                    case '\'':
                    {
                        builder.Append(@"\'");
                        continue;
                    }
                    case '\\':
                    {
                        builder.Append(@"\\");
                        continue;
                    }
                    case '\t':
                    {
                        builder.Append(@"\t");
                        continue;
                    }
                    case '\n':
                    {
                        builder.Append(@"\n");
                        continue;
                    }
                    case '\r':
                    {
                        builder.Append(@"\r");
                        continue;
                    }
                    case '"':
                    {
                        builder.Append("\\\"");
                        continue;
                    }
                }
                if ((ch < ' ') || (ch > '\x007f'))
                {
                    builder.AppendFormat(CultureInfo.InvariantCulture, @"\u{0:x4}", new object[] { (int) ch });
                }
                else
                {
                    count++;
                }
            }
            string str = s;
            if (builder == null)
            {
                return str;
            }
            if (count > 0)
            {
                builder.Append(s, startIndex, count);
            }
            return builder.ToString();
        }

        public void StartArrayScope()
        {
            this.StartScope(ScopeType.Array);
        }

        public void StartObjectScope()
        {
            this.StartScope(ScopeType.Object);
        }

        private void StartScope(ScopeType type)
        {
            if (this._scopes.Count != 0)
            {
                Scope scope = this._scopes.Peek();
                if ((scope.Type == ScopeType.Array) && (scope.ObjectCount != 0))
                {
                    this._writer.WriteTrimmed(", ");
                }
                scope.ObjectCount++;
            }
            Scope item = new Scope(type);
            this._scopes.Push(item);
            if (type == ScopeType.Array)
            {
                this._writer.Write("[");
            }
            else
            {
                this._writer.Write("{");
            }
            this._writer.Indent++;
            this._writer.WriteLine();
        }

        private void WriteCore(string text, bool quotes)
        {
            if (this._scopes.Count != 0)
            {
                Scope scope = this._scopes.Peek();
                if (scope.Type == ScopeType.Array)
                {
                    if (scope.ObjectCount != 0)
                    {
                        this._writer.WriteTrimmed(", ");
                    }
                    scope.ObjectCount++;
                }
            }
            if (quotes)
            {
                this._writer.Write('"');
            }
            this._writer.Write(text);
            if (quotes)
            {
                this._writer.Write('"');
            }
        }

        public void WriteName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (this._scopes.Count == 0)
            {
                throw new InvalidOperationException("No active scope to write into.");
            }
            if (this._scopes.Peek().Type != ScopeType.Object)
            {
                throw new InvalidOperationException("Names can only be written into Object scopes.");
            }
            Scope scope = this._scopes.Peek();
            if (scope.Type == ScopeType.Object)
            {
                if (scope.ObjectCount != 0)
                {
                    this._writer.WriteTrimmed(", ");
                }
                scope.ObjectCount++;
            }
            this._writer.Write(name);
            this._writer.WriteTrimmed(": ");
        }

        public void WriteValue(bool value)
        {
            this.WriteCore(value ? "true" : "false", false);
        }

        public void WriteValue(ICollection items)
        {
            if ((items == null) || (items.Count == 0))
            {
                this.WriteCore("[]", false);
            }
            else
            {
                this.StartArrayScope();
                foreach (object obj2 in items)
                {
                    this.WriteValue(obj2);
                }
                this.EndScope();
            }
        }

        public void WriteValue(IDictionary record)
        {
            if ((record == null) || (record.Count == 0))
            {
                this.WriteCore("{}", false);
            }
            else
            {
                this.StartObjectScope();
                foreach (DictionaryEntry entry in record)
                {
                    string key = entry.Key as string;
                    if (string.IsNullOrEmpty(key))
                    {
                        throw new ArgumentException("Key of unsupported type contained in Hashtable.");
                    }
                    this.WriteName(key);
                    this.WriteValue(entry.Value);
                }
                this.EndScope();
            }
        }

        public void WriteValue(DateTime dateTime)
        {
            if (dateTime < MinDate)
            {
                throw new ArgumentOutOfRangeException("dateTime");
            }
            this.WriteCore(@"\@" + (((dateTime.Ticks - MinDateTimeTicks) / 0x2710L)).ToString(CultureInfo.InvariantCulture) + "@", true);
        }

        public void WriteValue(double value)
        {
            this.WriteCore(value.ToString(CultureInfo.InvariantCulture), false);
        }

        public void WriteValue(int value)
        {
            this.WriteCore(value.ToString(CultureInfo.InvariantCulture), false);
        }

        public void WriteValue(object o)
        {
            if (o == null)
            {
                this.WriteCore("null", false);
            }
            else if (o is bool)
            {
                this.WriteValue((bool) o);
            }
            else if (o is int)
            {
                this.WriteValue((int) o);
            }
            else if (o is float)
            {
                this.WriteValue((float) o);
            }
            else if (o is double)
            {
                this.WriteValue((double) o);
            }
            else if (o is DateTime)
            {
                this.WriteValue((DateTime) o);
            }
            else if (o is string)
            {
                this.WriteValue((string) o);
            }
            else if (o is IDictionary)
            {
                this.WriteValue((IDictionary) o);
            }
            else if (o is ICollection)
            {
                this.WriteValue((ICollection) o);
            }
            else
            {
                this.StartObjectScope();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(o))
                {
                    this.WriteName(descriptor.Name);
                    this.WriteValue(descriptor.GetValue(o));
                }
                this.EndScope();
            }
        }

        public void WriteValue(float value)
        {
            this.WriteCore(value.ToString(CultureInfo.InvariantCulture), false);
        }

        public void WriteValue(string s)
        {
            if (s == null)
            {
                this.WriteCore("null", false);
            }
            else
            {
                this.WriteCore(QuoteJScriptString(s), true);
            }
        }

        private sealed class Scope
        {
            private int _objectCount;
            private Common.Web.MVC.Serialization.JsonWriter.ScopeType _type;

            public Scope(Common.Web.MVC.Serialization.JsonWriter.ScopeType type)
            {
                this._type = type;
            }

            public int ObjectCount
            {
                get
                {
                    return this._objectCount;
                }
                set
                {
                    this._objectCount = value;
                }
            }

            public Common.Web.MVC.Serialization.JsonWriter.ScopeType Type
            {
                get
                {
                    return this._type;
                }
            }
        }

        private enum ScopeType
        {
            Array,
            Object
        }
    }
}
