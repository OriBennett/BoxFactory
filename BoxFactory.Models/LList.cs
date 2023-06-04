using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

namespace BoxFactory.Models;

public class LList<T> : ICollection<T>
{
    // Node in the linked list
    public class Node 
    {
        // Value in the node, can be null
        public T? theBox;

        // Pointer to next node in the list, can be null
        public Node? Next;
        
        // Constructor
        public Node(T box)
        {
            theBox = box;
        }
    }
// Variables:
    // First node in the list, might be null 
    Node? head;
    // Counter for the number of nodes in the lists
    int _count = 0;
//Public methods:
    // Property for getting _count
    public int Count { get => _count; }

    public LList(ICollection<T> collection) => throw new NotImplementedException();
    // Constructor
    public LList() { }
    // Adding new element to the list.
    // Assigning head with new node, linking it to old head. 
    public void Add(T box)
    {
        Node toInsert = new Node(box);
        toInsert.Next = head;
        head = toInsert;

        _count++;
    }
    // Clearing all nodes, assigning 0 to _count
    public void Clear()
    {
        head = null;
        _count = 0;
    }

    // Removing an item from the list.
    // Searching for node with equal data, and removing it if found.
    // Return true if found.
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

    // Removing Next node of the prevNode from the list.
    // Used when the node to remove is known, instead of traversing the whole list to find it again.
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

// Implementations of ICollection interface
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

    public void Dispose() {}

    // Class for iteration over the list,
    public class BoxListEnumerator<T> : IEnumerator<T>
    {
        // The list itself,
        private LList<T>? _list;
        // Previous node in the list, saved to support deletition.
        private LList<T>.Node? prevNode;
        // Current node in the list,
        private LList<T>.Node? curNode;
        // Current object.
        private T? currBox;

        // Helper function to init all data structures to the begining of the list.
        private void initHead()
        {
            LList<T>.Node firstNode = new LList<T>.Node(default(T));
            firstNode.Next = _list.head;
            curNode = firstNode;
            prevNode = null;
        }
        // Constructor.
        public BoxListEnumerator(LList<T> list)
        {
            _list = list;
            initHead();
        }

        // One iteration forward.
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

        // Reseting iterator to the begining of the list.
        public void Reset()
        {
            initHead();
        }

        // Deleting the node this iterator is currently pointing at from the linked list,
        // Moving the iterator to the next node.
        // Return true is deletition succeeded.
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

        // Destructor
        void IDisposable.Dispose() {}

        // Current object pointed to, valid only after first MoveNext.
        public T Current
        {
            get { return currBox; }
        }

        // Current object pointed to, valid only after first MoveNext.
        object IEnumerator.Current
        {
            get { return Current; }
        }
    }

    // Interfaces for cretion of iterator from list
    public BoxListEnumerator<T> GetEnumerator()
    { return new BoxListEnumerator<T>(this); }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    { return new BoxListEnumerator<T>(this); }

    IEnumerator IEnumerable.GetEnumerator()
    { return new BoxListEnumerator<T>(this); }
}


