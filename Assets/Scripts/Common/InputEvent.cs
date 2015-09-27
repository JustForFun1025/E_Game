using System.Collections;
using System;

public class InputEvent : DictionaryBase
{
    public delegate void DEvent(string OperateType);
    public LinkList<DEvent> this[String key]
    {
        get
        {
            return ((LinkList<DEvent>)Dictionary[key]);
        }
        set
        {
            Dictionary[key] = value;
        }
    }

    public ICollection Keys
    {
        get
        {
            return (Dictionary.Keys);
        }
    }

    public ICollection Values
    {
        get
        {
            return (Dictionary.Values);
        }
    }

    public void Add(String key, DEvent value)
    {
        LinkList<DEvent> dEvent = new LinkList<DEvent>(value);
        if (this.Contains(key)) this[key].Add(dEvent);
        else    Dictionary.Add(key, dEvent);
    }

    public bool Contains(String key)
    {
        return (Dictionary.Contains(key));
    }

    public void Remove(String key)
    {
        Dictionary.Remove(key);
    }

    protected override void OnInsert(Object key, Object value)
    {
        if (key.GetType() != typeof(System.String))
            throw new ArgumentException("key must be of type String.", "key");
        if (value.GetType() != typeof(LinkList<DEvent>))
            throw new ArgumentException("value must be of type LinkList<Event>.", "value");
    }

    protected override void OnRemove(Object key, Object value)
    {
        if (key.GetType() != typeof(System.String))
            throw new ArgumentException("key must be of type String.", "key");
    }

    protected override void OnSet(Object key, Object oldValue, Object newValue)
    {
        if (key.GetType() != typeof(System.String))
            throw new ArgumentException("key must be of type String.", "key");
        if (newValue.GetType() != typeof(LinkList<DEvent>))
            throw new ArgumentException("newValue must be of type LinkList<Event>.", "newValue");
    }

    protected override void OnValidate(Object key, Object value)
    {
        if (key.GetType() != typeof(System.String))
            throw new ArgumentException("key must be of type String.", "key");
        if (value.GetType() != typeof(LinkList<DEvent>))
            throw new ArgumentException("value must be of type LinkList<Event>.", "value");
    }

}

public class LinkList<T>    //双向链表
{
    public T Value { private set; get; }
    public LinkList<T> Previous { private set; get; }
    public LinkList<T> Next { private set; get; }

    public LinkList(T value)
    {
        this.Value = value;
        this.Previous = null;
        this.Next = null;
    }
    public void Add(LinkList<T> newList)
    {
        LinkList<T> lastList = this;
        while (lastList.Next != null)
        {
            lastList = lastList.Next;
        }
        lastList.Next = newList;
        newList.Previous = lastList;
        newList.Next = null;
    }

    public void Delete()
    {
        if (this.Previous != null) this.Previous.Next = this.Next;
        if (this.Next != null) this.Next.Previous = this.Previous;
        this.Previous = null;
        this.Next = null;
        //TODO 显式释放内存？
    }
}