using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace Improx.Utility
{
  [Serializable]
  public class ObservableList<T> : List<T>
  {
    public event Action<int> OnChanged = delegate { };
    public event Action OnUpdated = delegate { };

      public ObservableList()
      {

      }

        public ObservableList(IEnumerable<T> oldList) : base(oldList)
      {
          
      }

    public new void Add(T item)
    {
      base.Add(item);
      OnUpdated();
    }
    public new void Remove(T item)
    {
      base.Remove(item);
      OnUpdated();
    }
    public new void AddRange(IEnumerable<T> collection)
    {
      base.AddRange(collection);
      OnUpdated();
    }
    public new void RemoveRange(int index, int count)
    {
      base.RemoveRange(index, count);
      OnUpdated();
    }
    public new void Clear()
    {
      base.Clear();
      OnUpdated();
    }
    public new void Insert(int index, T item)
    {
      base.Insert(index, item);
      OnUpdated();
    }
    public new void InsertRange(int index, IEnumerable<T> collection)
    {
      base.InsertRange(index, collection);
      OnUpdated();
    }
    public new void RemoveAll(Predicate<T> match)
    {
      base.RemoveAll(match);
      OnUpdated();
    }


    public new T this[int index]
    {
      get
      {
        return base[index];
      }
      set
      {
        base[index] = value;
        OnChanged(index);
      }
    }


  }
}
