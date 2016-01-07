namespace Common.Web.MVC.Serialization
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class JsonReader
    {
        private TextReader _reader;

        public JsonReader(TextReader reader)
        {
            this._reader = reader;
        }

        private string GetCharacters(int count)
        {
            string str = string.Empty;
            for (int i = 0; i < count; i++)
            {
                char ch = (char) this._reader.Read();
                if (ch == '\0')
                {
                    return null;
                }
                str = str + ch;
            }
            return str;
        }

        private char GetNextCharacter()
        {
            return (char) this._reader.Read();
        }

        private char GetNextSignificantCharacter()
        {
            char c = (char) this._reader.Read();
            while ((c != '\0') && char.IsWhiteSpace(c))
            {
                c = (char) this._reader.Read();
            }
            return c;
        }

        private char PeekNextSignificantCharacter()
        {
            char c = (char) this._reader.Peek();
            while ((c != '\0') && char.IsWhiteSpace(c))
            {
                this._reader.Read();
                c = (char) this._reader.Peek();
            }
            return c;
        }

        private ArrayList ReadArray()
        {
            ArrayList list = new ArrayList();
            this._reader.Read();
            while (true)
            {
                char ch = this.PeekNextSignificantCharacter();
                switch (ch)
                {
                    case '\0':
                        throw new FormatException("Unterminated array literal.");

                    case ']':
                        this._reader.Read();
                        return list;
                }
                if (list.Count != 0)
                {
                    if (ch != ',')
                    {
                        throw new FormatException("Invalid array literal.");
                    }
                    this._reader.Read();
                }
                object obj2 = this.ReadValue();
                list.Add(obj2);
            }
        }

        private bool ReadBoolean()
        {
            string str = this.ReadName(false);
            if (str != null)
            {
                if (str.Equals("true", StringComparison.Ordinal))
                {
                    return true;
                }
                if (str.Equals("false", StringComparison.Ordinal))
                {
                    return false;
                }
            }
            throw new FormatException("Invalid boolean literal.");
        }

        private string ReadName(bool allowQuotes)
        {
            char c = this.PeekNextSignificantCharacter();
            switch (c)
            {
                case '"':
                case '\'':
                    if (!allowQuotes)
                    {
                        return null;
                    }
                    return this.ReadString();
            }
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                c = (char) this._reader.Peek();
                if ((c != '_') && !char.IsLetterOrDigit(c))
                {
                    return builder.ToString();
                }
                this._reader.Read();
                builder.Append(c);
            }
        }

        private void ReadNull()
        {
            string str = this.ReadName(false);
            if ((str == null) || !str.Equals("null", StringComparison.Ordinal))
            {
                throw new FormatException("Invalid null literal.");
            }
        }

        private object ReadNumber()
        {
            char ch = (char) this._reader.Read();
            StringBuilder builder = new StringBuilder();
            bool flag = ch == '.';
            builder.Append(ch);
            while (true)
            {
                ch = this.PeekNextSignificantCharacter();
                if (!char.IsDigit(ch) && (ch != '.'))
                {
                    break;
                }
                this._reader.Read();
                builder.Append(ch);
                flag |= ch == '.';
            }
            string s = builder.ToString();
            if (flag)
            {
                float num;
                if (float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
                {
                    return num;
                }
            }
            else
            {
                try
                {
                    int num2;
                    if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out num2))
                    {
                        return num2;
                    }
                }
                catch
                {
                    long num3;
                    if (long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out num3))
                    {
                        return num3;
                    }
                }
            }
            throw new FormatException("Invalid numeric literal.");
        }

        private Hashtable ReadObject()
        {
            Hashtable hashtable = new Hashtable();
            this._reader.Read();
            while (true)
            {
                char ch = this.PeekNextSignificantCharacter();
                switch (ch)
                {
                    case '\0':
                        throw new FormatException("Unterminated object literal.");

                    case '}':
                        this._reader.Read();
                        return hashtable;
                }
                if (hashtable.Count != 0)
                {
                    if (ch != ',')
                    {
                        throw new FormatException("Invalid object literal.");
                    }
                    this._reader.Read();
                }
                string str = this.ReadName(true);
                if (this.PeekNextSignificantCharacter() != ':')
                {
                    throw new FormatException("Unexpected name/value pair syntax in object literal.");
                }
                this._reader.Read();
                object obj2 = this.ReadValue();
                hashtable[str] = obj2;
            }
        }

        private string ReadString()
        {
            bool flag;
            return this.ReadString(out flag);
        }

        private string ReadString(out bool hasLeadingSlash)
        {
            StringBuilder builder = new StringBuilder();
            char ch = (char) this._reader.Read();
            bool flag = false;
            bool flag2 = true;
            hasLeadingSlash = false;
            while (true)
            {
                char nextCharacter = this.GetNextCharacter();
                if (nextCharacter == '\0')
                {
                    throw new FormatException("Unterminated string literal.");
                }
                if (flag2)
                {
                    if (nextCharacter == '\\')
                    {
                        hasLeadingSlash = true;
                    }
                    flag2 = false;
                }
                if (flag)
                {
                    if (nextCharacter == 'u')
                    {
                        string characters = this.GetCharacters(4);
                        if (characters == null)
                        {
                            throw new FormatException("Unterminated string literal.");
                        }
                        nextCharacter = (char) int.Parse(characters, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                    }
                    builder.Append(nextCharacter);
                    flag = false;
                }
                else if (nextCharacter == '\\')
                {
                    flag = true;
                }
                else
                {
                    if (nextCharacter == ch)
                    {
                        return builder.ToString();
                    }
                    builder.Append(nextCharacter);
                }
            }
        }

        public object ReadValue()
        {
            object obj2 = null;
            bool flag = false;
            char c = this.PeekNextSignificantCharacter();
            switch (c)
            {
                case '[':
                    obj2 = this.ReadArray();
                    break;

                case '{':
                    obj2 = this.ReadObject();
                    break;

                case '\'':
                case '"':
                {
                    bool flag2;
                    long num;
                    string str = this.ReadString(out flag2);
                    if ((flag2 && str.StartsWith("@")) && (str.EndsWith("@") && long.TryParse(str.Substring(1, str.Length - 2), NumberStyles.Integer, CultureInfo.InvariantCulture, out num)))
                    {
                        obj2 = new DateTime((num * 0x2710L) + Common.Web.MVC.Serialization.JsonWriter.MinDateTimeTicks, DateTimeKind.Utc);
                    }
                    if (obj2 == null)
                    {
                        obj2 = str;
                    }
                    break;
                }
                default:
                    if ((char.IsDigit(c) || (c == '-')) || (c == '.'))
                    {
                        obj2 = this.ReadNumber();
                    }
                    else if ((c == 't') || (c == 'f'))
                    {
                        obj2 = this.ReadBoolean();
                    }
                    else if (c == 'n')
                    {
                        this.ReadNull();
                        flag = true;
                    }
                    break;
            }
            if ((obj2 == null) && !flag)
            {
                throw new FormatException("Invalid JSON text.");
            }
            return obj2;
        }
    }
}
