using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace BoxFactory.Models;


public class LList<T> : ICollection<T>
{
    public class Node 
    {
        public T? theBox;

        public Node? Next;
        
        public Node(T box)
        {
            theBox = box;
        }
    }

    Node? head;
    int _count = 0;
    public int Count { get => _count; set => _count = value; }

    public LList(ICollection<T> collection) => throw new NotImplementedException();
    public LList() { }

    public void Add(T box)
    {
        Node toInsert = new Node(box);
        if (head == null)
        {
            head = toInsert;
        }
        else
        {
            toInsert.Next = head;
            head = toInsert;
        }
        _count++;
    }
    public void Clear()
    {
        head = null;
        _count = 0;
    }
    public bool Remove(T box)
    {
        if (head == null)
        {
            return false;
        }

        if (object.Equals(head.theBox, box))
        {
            head = head.Next;
            _count--;
            return true;
        }

        Node prev = head;
        Node? current = prev.Next;

        while (current != null)
        {
            if (object.Equals(current.theBox, box))
            {
                prev.Next = current.Next;
                _count--;
                return true;
            }

            prev = current;
            current = current.Next;
        }

        return false;
    }

    public bool RemoveNext(Node prevNode)
    {
        if (prevNode.Next == null)
        {
            return false;
        }

        prevNode.Next = prevNode.Next.Next;
        _count--;
        return true;
    }
    public T GetFirst() { return head.theBox; }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
    bool ICollection<T>.Contains(T item)
    {     
        Node? current = head;
        while (current != null)
        {
            if (object.Equals(current.theBox, item))
            {
                return true;
            }
            current = current.Next;
        }
        return false;
    }
    public void Dispose(){ }

    public class BoxListEnumerator : IEnumerator<T>
    {
        private LList<T>? _list;
        private LList<T>.Node? prevNode;
        private LList<T>.Node? curNode;
        private T? currBox;

        private void initHead()
        {
            LList<T>.Node firstNode = new LList<T>.Node(default(T));
            firstNode.Next = _list.head;
            curNode = firstNode;
            prevNode = null;
        }

        public BoxListEnumerator(LList<T> list)
        {
            _list = list;
            initHead();
        }

        public bool MoveNext()
        {
            //Avoids going beyond the end of the collection.
            if (curNode == null)
            {
                return false;
            }

            prevNode = curNode;
            curNode = curNode.Next;

            if (curNode == null)
            {
                return false;
            }

            currBox = curNode.theBox;
            return true;
        }

        public void Reset()
        {
            initHead();
        }

        public bool Erase()
        {
            if (curNode == null || prevNode == null)
            {
                return false;
            }
            
            if (object.Equals(currBox, _list.head.theBox))
            {
                _list?.Remove(currBox);
                initHead();
                return true;
            }

            _list?.RemoveNext(prevNode);
            curNode = prevNode.Next;
            return true;
        }

        void IDisposable.Dispose()
        {
        }

        void DeleteUnderlineObject()
        {
        }

        public T Current
        {
            get { return currBox; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    public BoxListEnumerator GetEnumerator()
    { return new BoxListEnumerator(this); }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return new BoxListEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new BoxListEnumerator(this);
    }
}


