using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TinyGames.Engine.Util
{
    public class RingList<T> : IEnumerable<T>
    {
        private T[] _data;
        private int _writeIndex;
        private int _length;

        public IEnumerable<T> Data => GetEnumerable();
        public int Count => _length;

        public RingList(int length)
        {
            Debug.Assert(length > 0);
            
            _data = new T[length];

            _length = 0;
            _writeIndex = 0;
        }

        public void Add(T data)
        {
            _data[_writeIndex] = data;

            if (_length < _data.Length) _length++;

            _writeIndex++;
            _writeIndex %= _data.Length;
        }



        public IEnumerable<T> GetEnumerable()
        {
            for(int i = 0; i < _length; i++)
            {
                yield return _data[GetReadIndex(i)];
            }
        }

        private int GetReadIndex(int item)
        {
            return (item + _writeIndex) % _length;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerable().GetEnumerator();
        }
    }
}
