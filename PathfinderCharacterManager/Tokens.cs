using System.Collections;
using System.Collections.Generic;

namespace PathfinderCharacterManager
{
    public class Token { }
    public class TokenModifer<T>
    {
        public TokenModifer(T value, string source)
        {
            this.value = value;
            this.source = source;
        }
        public T value { get; }
        public string source { get; }
    }
    public class TokenValue<T> : IEnumerable<TokenModifer<T>>
    {
        private readonly IEnumerable<TokenModifer<T>> _mods;
        public TokenValue(IEnumerable<TokenModifer<T>> mods)
        {
            _mods = mods;
        }
        public IEnumerator<TokenModifer<T>> GetEnumerator()
        {
            return _mods.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_mods).GetEnumerator();
        }
    }
    public class TokenQuery : QueryEvent
    {
        public TokenQuery(Token token)
        {
            this.token = token;
        }
        public Token token { get; }
    }
}
