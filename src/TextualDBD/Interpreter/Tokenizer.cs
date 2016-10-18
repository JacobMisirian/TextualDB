using System;
using System.Collections.Generic;
using System.Text;

namespace TextualDBD.Interpreter
{
    public class Tokenizer
    {
        private List<Token> result;
        private int position;
        private string code;

        public List<Token> Scan(string line)
        {
            result = new List<Token>();
            position = 0;
            code = line;

            while (peekChar() != -1)
            {
                whiteSpace();
                if (char.IsLetterOrDigit((char)peekChar()))
                    scanIdentifier();
                else
                {
                    switch ((char)peekChar())
                    {
                        case '"':
                            scanString();
                            break;
                        case '*':
                            result.Add(Token.Create(TokenType.Identifier, ((char)readChar()).ToString()));
                            break;
                        case '|':
                            readChar();
                            if ((char)peekChar() == '|')
                                readChar();
                            result.Add(Token.Create(TokenType.Operation, "||"));
                            break;
                        case '&':
                            readChar();
                            if ((char)peekChar() == '&')
                                readChar();
                            result.Add(Token.Create(TokenType.Operation, "&&"));
                            break;
                        case '=':
                            readChar();
                            if ((char)peekChar() == '=')
                                readChar();
                            result.Add(Token.Create(TokenType.Comparison, "=="));
                            break;
                        case '>':
                        case '<':
                        case '!':
                            char op = (char)readChar();
                            if ((char)peekChar() == '=')
                            {
                                readChar();
                                result.Add(Token.Create(TokenType.Comparison, op + "" + (char)readChar()));
                            }
                            else
                                result.Add(Token.Create(TokenType.Comparison, op.ToString()));
                            break;
                        default:
                            Console.WriteLine("Unknown char {0}, ASCII value {1}!", (char)peekChar(), readChar());
                            break;
                    }
                }
            }
            return result;
        }

        private void whiteSpace()
        {
            while (char.IsWhiteSpace((char)peekChar()))
                readChar();
        }

        private void scanIdentifier()
        {
            StringBuilder sb = new StringBuilder();

            while (char.IsLetterOrDigit((char)peekChar()) && peekChar() != -1)
                sb.Append((char)readChar());

            result.Add(Token.Create(TokenType.Identifier, sb.ToString()));
        }

        private void scanString()
        {
            StringBuilder sb = new StringBuilder();

            readChar(); // "
            while ((char)peekChar() != '"' && peekChar() != -1)
                sb.Append((char)readChar());
            readChar(); // "

            result.Add(Token.Create(TokenType.String, sb.ToString()));
        }

        private int peekChar(int n = 0)
        {
            return position + n < code.Length ? code[position + n] : -1;
        }
        private int readChar()
        {
            return position < code.Length ? code[position++] : -1;
        }
    }
}

