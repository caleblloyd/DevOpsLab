using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DevOpsLab.Server.Models.Interfaces;

namespace DevOpsLab.Server.Models.Collections
{
    public class RankedList<T> : IList<T> where T : IRanked
    {
        protected readonly SortedList<double, T> Sl;

        public RankedList()
        {
            Sl = new SortedList<double, T>();
        }

        public IEnumerator<T> GetEnumerator() => Sl.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item) => Insert(Count, item);

        public void Clear() => Sl.Clear();

        public bool Contains(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return Sl.Values.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) => Sl.Values.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return Sl.Remove(item.Rank ?? throw new ArgumentNullException(nameof(item.Rank)));
        }

        public int Count => Sl.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return Sl.IndexOfValue(item);
        }

        public void Insert(int index, T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (!item.Rank.HasValue)
            {
                if (Count == 0)
                {
                    item.Rank = 0;
                }
                else if (index == 0)
                {
                    item.Rank = this[0].Rank - 1;
                }
                else if (index == Count)
                {
                    item.Rank = this[Count - 1].Rank + 1;
                }
                else
                {
                    var lower = this[index - 1].Rank;
                    var upper = this[index].Rank;
                    item.Rank = ((lower ?? 0) + (upper ?? 0)) / 2;
                }
            }

            Debug.Assert(item.Rank != null, "item.Rank != null");
            Debug.Assert(Sl.Keys.Contains(item.Rank.Value) == false, "Sl.Keys.Contains(item.Rank.Value) == false");
            Sl.Add(item.Rank.Value, item);
        }

        public void RemoveAt(int index) => Sl.RemoveAt(index);

        public T this[int index]
        {
            get => Sl.ElementAt(index).Value;
            set => Sl[Sl.ElementAt(index).Key] = value;
        }
    }
}
