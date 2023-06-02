using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace BoxFactory.Models;

public class BoxList : ICollection<BoxBatch>
{
    private class Node 
    {
        public BoxBatch theBox;

        public Node? Next;
        
        public Node(BoxBatch box)
        {
            theBox = box;
        }
    }

    Node? head;
    int _count = 0;
    public int Count { get => _count; }

    public BoxList(ICollection<BoxBatch> collection) => throw new NotImplementedException();
    public BoxList() { }

    public void Add(BoxBatch box)
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
    public bool Remove(BoxBatch box)
    {
        if (head == null)
        {
            return false;
        }

        if (head.theBox == box)
        {
            head = head.Next;
            _count--;
            return true;
        }

        Node prev = head;
        Node? current = prev.Next;

        while (current != null)
        {
            if (current.theBox == box)
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
    public BoxBatch? GetBox() { return head?.theBox; }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public void CopyTo(BoxBatch[] array, int arrayIndex) => throw new NotImplementedException();
    bool ICollection<BoxBatch>.Contains(BoxBatch item)
    {     
        Node? current = head;
        while (current != null)
        {
            if (current.theBox == item)
            {
                return true;
            }
            current = current.Next;
        }
        return false;
    }
    public void Dispose(){ }

    public IEnumerator GetEnumerator() { return this.GetEnumerator(); }
    IEnumerator<BoxBatch> IEnumerable<BoxBatch>.GetEnumerator()
    {
        
        Node? current = head;
        while (current != null)
        {
            yield return current.theBox;
            current = current.Next;

        }
        //return current.theBox;
    }
    public class Enumerator : IEnumerator
    {
        private Node? node = BoxList.head;
        private int Cursor;
        public Enumerator(int[] intarr)
        {
            this.intArr = intarr;
            Cursor = -1;
        }
    }


}
