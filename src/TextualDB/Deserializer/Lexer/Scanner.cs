using System;
using System.Collections.Generic;
using System.Text;

using TextualDB.Exceptions;

namespace TextualDB.Deserializer.Lexer
{
    public class Scanner
    {
        private List<Token> tokens;

        private SourceLocation location;
        private int pos;
        private string source;

        public List<Token> Scan(string file, string source)
        {
            tokens = new List<Token>();
            
            location = new SourceLocation(file, 1, 1);
            pos = 0;
            this.source = source;

            whiteSpace();
            while (peekChar() != -1)
            {
                if ((char)peekChar() == '"')
                    scanString(false);
                else if (char.IsLetterOrDigit((char)peekChar()))
                    scanIdentifier();
                else
                {
                    char c;
                    switch ((char)peekChar())
                    {
                        case '*':
                            add(TokenType.Asterisk, ((char)readChar()).ToString());
                            break;
                        case ':':
                            add(TokenType.Colon, ((char)readChar()).ToString());
                            break;
                        case ',':
                            add(TokenType.Comma, ((char)readChar()).ToString());
                            break;
                        case '=':
                            add(TokenType.Comparison, ((char)readChar()).ToString());
                            break;
                        case '>':
                        case '<':
                            c = (char)readChar();
                            if ((char)peekChar() == '=')
                                add(TokenType.Comparison, c + "" + (char)readChar());
                            else
                                add(TokenType.Comparison, c.ToString());
                            break;
                        case '!':
                            readChar();
                            if ((char)peekChar() == '=')
                                add(TokenType.Comparison, "!" + (char)readChar());
                            else
                                add(TokenType.Exclamation, "!");
                            break;
                        case '-':
                            add(TokenType.Hyphen, ((char)readChar()).ToString());
                            break;
                        case '|':
                            add(TokenType.Pipe, ((char)readChar()).ToString());
                            break;
                        case '{':
                            readChar();
                            add(TokenType.Placeholder, ((char)readChar()).ToString());
                            readChar();
                            break;
                        case '?':
                            add(TokenType.QuestionMark, ((char)readChar()).ToString());
                            break;
                        default:
                            throw new DeserializerException(location, "Unknown char {0} in lexer!", readChar());
                    }
                }
                whiteSpace();
            }
            return tokens;
        }

        private void scanString(bool isVerbatim)
        {
            var str = new StringBuilder();
            readChar();
            while ((char)peekChar() != '\"' && peekChar() != -1)
            {
                char ch = (char)readChar();
                if (ch == '\\' && !isVerbatim)
                    str.Append(scanEscapeCode((char)readChar()));
                else
                    str.Append(ch);
            }
            readChar();

            add(TokenType.String, str.ToString());
        }

        private char scanEscapeCode(char escape)
        {
            switch (escape)
            {
                case '\\':
                    return '\\';
                case '"':
                    return '\"';
                case '\'':
                    return '\'';
                case 'a':
                    return '\a';
                case 'b':
                    return '\b';
                case 'f':
                    return '\f';
                case 'n':
                    return '\n';
                case 'r':
                    return '\r';
                case 't':
                    return '\t';
                case 'v':
                    return '\v';
                case '#':
                    return '#';
                default:
                    throw new DeserializerException(location, "Unknown escape sequence {0}!", escape);
            }
        }

        private void scanIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            while ((char.IsLetterOrDigit((char)peekChar()) || (char)peekChar() == '_') && peekChar() != -1)
                sb.Append((char)readChar());
            string val = sb.ToString();
            try
            {
                add(TokenType.Number, Convert.ToDouble(val).ToString());
            }
            catch
            {
                add(TokenType.Identifier, val);
            }
        }

        private void whiteSpace()
        {
            while (char.IsWhiteSpace((char)peekChar()))
                readChar();
        }

        private int peekChar(int n = 0)
        {
            return pos + n < source.Length ? source[pos + n] : -1;
        }
        private int readChar()
        {
            if (peekChar() == '\n')
                location = new SourceLocation(location.File, location.Line + 1, 1);
            else
                location = new SourceLocation(location.File, location.Line, location.Position + 1);
            return pos < source.Length ? source[pos++] : -1;
        }

        private void add(TokenType tokenType, string val)
        {
            tokens.Add(new Token(location, tokenType, val));
        }
    }
}
