using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WF.SDK.Common
{
  [Serializable]
  public abstract class Tuple
  {
    [XmlIgnore]
    public abstract int Count { get; }

    [XmlIgnore]
    public object this[int item]
    {
      get
      {
        if (0 <= item && item < this.Count)
        {
          var name = "Item" + item.ToString();
          return ObjectHelper.GetMember<object>(this, name);
        }
        throw new ArgumentOutOfRangeException("Argument out of range " + this.ToString());
      }
    }

    public override string ToString()
    {
      return string.Format("Tuple<{0}>", this.Count.ToString());
    }

    public IEnumerator<object> GetEnumerator()
    {
      return new TupleEnumerator(this);
    }

    //System.Collections.IEnumerator GetEnumerator()
    //{
    //  return new TupleEnumerator(this);
    //}

    //public void Add(object o) { return; }
  }

  //Same as Couple
  [Serializable]
  public class Tuple<T1, T2> : Tuple
  {
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }

    public Tuple() { }

    public Tuple(T1 item1 = default(T1), T2 item2 = default(T2))
    {
      this.Item1 = item1;
      this.Item2 = item2;
    }

    [XmlIgnore]
    public override int Count { get { return 2; } }
  }


  [Serializable]
  public class Tuple<T1, T2, T3> : Tuple
  {
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }
    public T3 Item3 { get; set; }

    public Tuple() { }

    public Tuple(T1 item1 = default(T1), T2 item2 = default(T2), T3 item3 = default(T3))
    {
      this.Item1 = item1;
      this.Item2 = item2;
      this.Item3 = item3;
    }

    [XmlIgnore]
    public override int Count { get { return 3; } }
  }


  [Serializable]
  public class Tuple<T1, T2, T3, T4> : Tuple
  {
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }
    public T3 Item3 { get; set; }
    public T4 Item4 { get; set; }

    public Tuple() { }

    public Tuple(T1 item1 = default(T1), T2 item2 = default(T2), T3 item3 = default(T3), T4 item4 = default(T4))
    {
      this.Item1 = item1;
      this.Item2 = item2;
      this.Item3 = item3;
      this.Item4 = item4;
    }

    [XmlIgnore]
    public override int Count { get { return 4; } }
  }

  [Serializable]
  public class Tuple<T1, T2, T3, T4, T5> : Tuple
  {
    public T1 Item1 { get; set; }
    public T2 Item2 { get; set; }
    public T3 Item3 { get; set; }
    public T4 Item4 { get; set; }
    public T5 Item5 { get; set; }

    public Tuple() { }

    public Tuple(T1 item1 = default(T1), T2 item2 = default(T2), T3 item3 = default(T3), T4 item4 = default(T4), T5 item5 = default(T5))
    {
      this.Item1 = item1;
      this.Item2 = item2;
      this.Item3 = item3;
      this.Item4 = item4;
      this.Item5 = item5;
    }

    [XmlIgnore]
    public override int Count { get { return 5; } }
  }

  public class TupleEnumerator : IEnumerator<object>
  {
    private Tuple instance = null;
    private int idx = -1;

    public TupleEnumerator(Tuple instance)
    {
      this.instance = instance;
    }

    public object Current
    {
      get { return this.instance[idx]; }
    }

    public void Dispose()
    {
      this.instance = null;
    }

    public bool MoveNext()
    {
      idx++;
      if (idx >= this.instance.Count) { return false; }
      return true;
    }

    public void Reset()
    {
      this.idx = -1;
    }
  }
}
