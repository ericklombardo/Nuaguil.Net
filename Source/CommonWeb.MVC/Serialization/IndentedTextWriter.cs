namespace Common.Web.MVC.Serialization
{
    using System;
    using System.IO;
    using System.Text;
    using System.Globalization;

    internal sealed class IndentedTextWriter : TextWriter
    {
        private int _indentLevel;
        private bool _minimize;
        private bool _tabsPending;
        private string _tabString;
        private TextWriter _writer;

        public IndentedTextWriter(TextWriter writer, bool minimize) : base(CultureInfo.InvariantCulture)
        {
            this._writer = writer;
            this._minimize = minimize;
            if (this._minimize)
            {
                this.NewLine = "\r";
            }
            this._tabString = "    ";
            this._indentLevel = 0;
            this._tabsPending = false;
        }

        public override void Close()
        {
            this._writer.Close();
        }

        public override void Flush()
        {
            this._writer.Flush();
        }

        private void OutputTabs()
        {
            if (this._tabsPending)
            {
                if (!this._minimize)
                {
                    for (int i = 0; i < this._indentLevel; i++)
                    {
                        this._writer.Write(this._tabString);
                    }
                }
                this._tabsPending = false;
            }
        }

        public override void Write(bool value)
        {
            this.OutputTabs();
            this._writer.Write(value);
        }

        public override void Write(char value)
        {
            this.OutputTabs();
            this._writer.Write(value);
        }

        public override void Write(string s)
        {
            this.OutputTabs();
            this._writer.Write(s);
        }

        public override void Write(char[] buffer)
        {
            this.OutputTabs();
            this._writer.Write(buffer);
        }

        public override void Write(double value)
        {
            this.OutputTabs();
            this._writer.Write(value);
        }

        public override void Write(int value)
        {
            this.OutputTabs();
            this._writer.Write(value);
        }

        public override void Write(long value)
        {
            this.OutputTabs();
            this._writer.Write(value);
        }

        public override void Write(object value)
        {
            this.OutputTabs();
            this._writer.Write(value);
        }

        public override void Write(float value)
        {
            this.OutputTabs();
            this._writer.Write(value);
        }

        public override void Write(string format, params object[] arg)
        {
            this.OutputTabs();
            this._writer.Write(format, arg);
        }

        public override void Write(string format, object arg0)
        {
            this.OutputTabs();
            this._writer.Write(format, arg0);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            this.OutputTabs();
            this._writer.Write(buffer, index, count);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            this.OutputTabs();
            this._writer.Write(format, arg0, arg1);
        }

        public override void WriteLine()
        {
            this.OutputTabs();
            this._writer.WriteLine();
            this._tabsPending = true;
        }

        public override void WriteLine(bool value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(char value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(double value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(int value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(char[] buffer)
        {
            this.OutputTabs();
            this._writer.WriteLine(buffer);
            this._tabsPending = true;
        }

        public override void WriteLine(long value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(object value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(float value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(string s)
        {
            this.OutputTabs();
            this._writer.WriteLine(s);
            this._tabsPending = true;
        }

        public override void WriteLine(uint value)
        {
            this.OutputTabs();
            this._writer.WriteLine(value);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.OutputTabs();
            this._writer.WriteLine(format, arg);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, object arg0)
        {
            this.OutputTabs();
            this._writer.WriteLine(format, arg0);
            this._tabsPending = true;
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            this.OutputTabs();
            this._writer.WriteLine(format, arg0, arg1);
            this._tabsPending = true;
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            this.OutputTabs();
            this._writer.WriteLine(buffer, index, count);
            this._tabsPending = true;
        }

        public void WriteLineNoTabs(string s)
        {
            this._writer.WriteLine(s);
        }

        public void WriteNewLine()
        {
            if (!this._minimize)
            {
                this.WriteLine();
            }
        }

        public void WriteSignificantNewLine()
        {
            this.WriteLine();
        }

        public void WriteTrimmed(string text)
        {
            if (!this._minimize)
            {
                this.Write(text);
            }
            else
            {
                this.Write(text.Trim());
            }
        }

        public override System.Text.Encoding Encoding
        {
            get
            {
                return this._writer.Encoding;
            }
        }

        public int Indent
        {
            get
            {
                return this._indentLevel;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                this._indentLevel = value;
            }
        }

        public override string NewLine
        {
            get
            {
                return this._writer.NewLine;
            }
            set
            {
                this._writer.NewLine = value;
            }
        }
    }
}
