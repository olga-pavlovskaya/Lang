using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class TokenManager
    {
        public void Reset()
        {
            Tokens = new List<Token>();
            AllTokens = new Dictionary<int, int>();
        }
        public List<Token> Tokens { get; set; }
        public Dictionary<Int32, Int32> AllTokens { get; set; }
        public void AddToken(Token t)
        {
            if (Tokens == null) Tokens = new List<Token>();
            if (Tokens.Count(tok => tok.Type == t.Type && tok.Value == t.Value) == 0)
                Tokens.Add(t);
        }
        public void AddToAll(int i, Token t)
        {
            int idx = Tokens.FindIndex(tok => tok.Type == t.Type && tok.Value == t.Value);
            if (idx != -1)
                AllTokens.Add(i, idx);
        }
        public Token GetToken(int i, ref int idx)
        {
            if (AllTokens == null) return null;
            if (idx == -1)
                idx = AllTokens.FirstOrDefault(t =>
                {
                    int len = Tokens[t.Value].Value.Length;
                    return (t.Key <= i && t.Key + len > i);
                }).Value;
            if (idx != -1)
                return Tokens[idx];
            return null;
        }
        public List<int> GetAllSelections(Token t)
        {
            int idx = Tokens.FindIndex(tok => tok.Type == t.Type && tok.Value == t.Value);
            return AllTokens.Where(tok => tok.Value == idx).Select(tok => tok.Key).ToList();
        }
    }
}
