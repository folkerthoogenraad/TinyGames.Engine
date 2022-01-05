using System;
using System.Collections.Generic;
using System.Text;

namespace StudentBikeGame
{
    public class DataMapping<TKey, TValue>
    {
        private Dictionary<TKey, TValue> Values;

        public TValue DefaultValue { get; set; }

        public DataMapping(TValue defaultValue)
        {
            DefaultValue = defaultValue;
            Values = new Dictionary<TKey, TValue>();
        }

        public TValue Map(TKey key)
        {
            if(Values.ContainsKey(key)) return Values[key];

            return DefaultValue;
        }

        public void Register(TKey key, TValue value)
        {
            Values[key] = value;
        }
    }
}
