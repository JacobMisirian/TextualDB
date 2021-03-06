﻿using System;
using System.Collections.Generic;
using System.Text;

using TextualDB.CommandLine.Exceptions;

namespace TextualDB.CommandLine.Lexer
{
    public class Scanner
    {
        private string code;
        private int position;
        private List<Token> result;

        private SourceLocation location;
        private int row = 1, column = 1;

        public Scanner(string source)
        {
            code = source;
            position = 0;
            result = new List<Token>();
            location = new SourceLocation(code, row, column);
        }

        public List<Token> Scan()
        {
            eatWhiteSpace();
            while (peekChar() != -1)
            {
                if (char.IsLetterOrDigit((char)peekChar()))
                    result.Add(scanIdentifier());
                else
                {
                    switch ((char)peekChar())
                    {
                        case '"':
                            result.Add(scanString());
                            break;
                        case ',':
                            result.Add(new Token(location, TokenType.Comma, readCharAsString()));
                            break;
                        case '*':
                            result.Add(new Token(location, TokenType.Identifier, readCharAsString()));
                            break;
                        case ';':
                            result.Add(new Token(location, TokenType.Semicolon, readCharAsString()));
                            break;
                        case '=':
                            result.Add(new Token(location, TokenType.Comparison, readCharAsString()));
                            break;
                        case '<':
                            if ((char)peekChar(1) == '=')
                            {
                                result.Add(new Token(location, TokenType.Comparison, "<="));
                                position += 2;
                            }
                            else
                                result.Add(new Token(location, TokenType.Comparison, readCharAsString()));
                            break;
                        case '>':
                            if ((char)peekChar(1) == '=')
                            {
                                result.Add(new Token(location, TokenType.Comparison, ">="));
                                position += 2;
                            }
                            else
                                result.Add(new Token(location, TokenType.Comparison, readCharAsString()));
                            break;
                        case '!':
                            if ((char)peekChar(1) == '=')
                            {
                                result.Add(new Token(location, TokenType.Comparison, "!="));
                                position += 2;
                            }
                            else
                                throw new UnknownCharacterException(location, (char)readChar());
                            break;
                        default:
                            char c = (char)readChar();
                            throw new UnknownCharacterException(location, c);
                    }
                }
                eatWhiteSpace();
            }
            return result;
        }

        private void eatWhiteSpace()
        {
            while (char.IsWhiteSpace((char)peekChar()) && peekChar() != -1)
                readChar();
        }

        private Token scanIdentifier()
        {
            StringBuilder sb = new StringBuilder();
            while ((char.IsLetterOrDigit((char)peekChar()) || (char)peekChar() == '_') && peekChar() != -1)
                sb.Append((char)readChar());

            string id = sb.ToString();
            try
            {
                return new Token(location, TokenType.Number, Convert.ToDouble(id).ToString());
            }
            catch
            {
                if (id.ToUpper() == "CONTAINS")
                    return new Token(location, TokenType.Comparison, id);
                else if (id.ToUpper() == "AND" || id.ToUpper() == "OR")
                    return new Token(location, TokenType.Comparison, id);
                return new Token(location, TokenType.Identifier, id);
            }
        }

        private Token scanString()
        {
            readChar(); // "
            StringBuilder sb = new StringBuilder();
            while ((char)peekChar() != '\"' && peekChar() != -1)
            {
                char ch = (char)readChar();
                if (ch == '\\')
                    sb.Append(scanEscapeCode((char)readChar()));
                else
                    sb.Append(ch);
            }
            readChar(); // "

            return new Token(location, TokenType.String, sb.ToString());
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
                    throw new UnknownEscapeCodeException(location, escape.ToString());
            }
        }

        private int peekChar(int n = 0)
        {
            return position + n < code.Length ? code[position + n] : -1;
        }

        private int readChar()
        {
            if (peekChar() == -1)
                return -1;
            char c = (char)peekChar();

            if (c == '\n')
            {
                row++;
                column = 1;
            }
            else
                column++;
            location = new SourceLocation(code, row, column);
            position++;
            return c;
        }

        private string readCharAsString()
        {
            return ((char)readChar()).ToString();
        }
    }
}
