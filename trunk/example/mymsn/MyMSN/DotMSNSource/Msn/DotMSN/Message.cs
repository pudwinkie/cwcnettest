namespace DotMSN
{
    using System;
    using System.Drawing;
    using System.Text.RegularExpressions;

    public class Message
    {
        // Methods
        public Message(string body, string header)
        {
            this.Decorations = MSNTextDecorations.None;
            this.Text = body;
            this.Header = header;
        }

        internal void ParseHeader()
        {
            string text1;
            string text2;
            Match match1 = Regex.Match(this.Header, @"FN=(?<Font>\S+);", RegexOptions.Multiline);
            if (match1.Success)
            {
                this.Font = HttpUtility.UrlDecode(match1.Groups["Font"].ToString());
            }
            match1 = Regex.Match(this.Header, @"EF=(?<Decoration>\S*);", RegexOptions.Multiline);
            if (match1.Success)
            {
                this.Decorations = MSNTextDecorations.None;
                text1 = match1.Groups["Decoration"].ToString();
                if (text1.IndexOf('I') >= 0)
                {
                    this.Decorations |= MSNTextDecorations.Italic;
                }
                if (text1.IndexOf('B') >= 0)
                {
                    this.Decorations |= MSNTextDecorations.Bold;
                }
                if (text1.IndexOf('U') >= 0)
                {
                    this.Decorations |= MSNTextDecorations.Underline;
                }
                if (text1.IndexOf('S') >= 0)
                {
                    this.Decorations |= MSNTextDecorations.Strike;
                }
            }
            match1 = Regex.Match(this.Header, @"CO=(?<Color>\S+);", RegexOptions.Multiline);
            if (match1.Success)
            {
                text2 = match1.Groups["Color"].ToString();
                try
                {
                    if (text2.Length < 6)
                    {
                        goto Label_0157;
                    }
                    this.Color = Color.FromArgb(int.Parse(text2.Substring(4, 2), NumberStyles.HexNumber), int.Parse(text2.Substring(2, 2), NumberStyles.HexNumber), int.Parse(text2.Substring(0, 2), NumberStyles.HexNumber));
                }
                catch (Exception)
                {
                }
            }
        Label_0157:
            match1 = Regex.Match(this.Header, @"CS=(?<Charset>\d+);", RegexOptions.Multiline);
            if (!match1.Success)
            {
                return;
            }
            try
            {
                this.Charset = ((MSNCharset) int.Parse(match1.Groups["Charset"].ToString()));
            }
            catch (Exception)
            {
            }
        }


        // Fields
        public MSNCharset Charset;
        public Color Color;
        public MSNTextDecorations Decorations;
        public string Font;
        public string Header;
        public string Text;
    }}

