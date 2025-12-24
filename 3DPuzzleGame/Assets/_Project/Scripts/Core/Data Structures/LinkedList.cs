using System;
using System.Collections;
using System.Collections.Generic;


namespace Vault.DataStrucures
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class LinkedList<T> : IEnumerable<Node<T>>
    {
        private Node<T> head;

        public Node<T> Head => head;

        private Node<T> _iterator;

        private int _count = 0;

        public int Count{get {if(_count == 0)Traverse(null); return _count; }}

        //Indexer
        //Indexer declaration
        public T this[int index]
        {
            get
            {
                if(index < Count)
                {
                    Traverse(null,index+1);
                    return _iterator.Data;
                }
                return default;
            }
            set
            {
                if(index < Count)
                {
                    Traverse(null,index+1);
                    _iterator.Data = value;
                }
            }
        }

        public LinkedList()
        {
            head = null;
        }

        public void Prepend(T data)
        {
            Node<T> newNode = new Node<T>(data);
            newNode.Next = head;
            head = newNode;
        }

        public void Append(T data)
        {
            Node<T> newNode = new Node<T>(data);

            if (head == null)
            {
                head = newNode;
                return;
            }

            Node<T> current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = newNode;
        }

        public bool Remove(T data)
        {
            if (head == null)
                return false;

            if (head.Data.Equals(data))
            {
                head = head.Next;
                return true;
            }

            Node<T> current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Equals(data))
                {
                    current.Next = current.Next.Next;
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        //This method allows us to perform any action on each node's data, such as printing it or performing calculations.
        public void Traverse(Action<T> action, int limit=-1)
        {
            _iterator = head;
            bool firstTrarversal = _count == 0;
            while (_iterator != null && (limit ==-1 || _count < limit))
            { 
                action(_iterator.Data);
                _iterator =_iterator.Next;
                if(firstTrarversal)_count++;
            }
        }

        public Node<T> Reverse(Node<T> head)
        {
            Node<T> previous = null;
            Node<T> current = head;
            Node<T> next = null;

            while (current != null)
            {
                next = current.Next;        
                current.Next = previous;
                previous = current;
                current = next;
            }

            //head = previous;
            return previous;
        }

        LinkedList<T> CopyList(Node<T> head)
        {
            if (head == null) return null;

            LinkedList<T> copy = new LinkedList<T>();

            Node<T> newHead = new Node<T>(head.Data);//copy head
            Node<T> currentOld = head.Next;//original old
            //Node<T> currentNew = newHead;//copy new

            copy.Append(currentOld.Data);

            while (currentOld != null)
            {
                currentOld = currentOld.Next;
                copy.Append(currentOld.Data);
            }

            return copy;
        }


        public T FindMiddle()
        {
            if (head == null)
                throw new InvalidOperationException("The list is empty.");

            Node<T> slow = head;

            Node<T> fast = head;

            while (fast != null && fast.Next != null)
            {
                slow = slow.Next;
                fast = fast.Next.Next;
            }

            return slow.Data;
        }

        public void RemoveDuplicates()
        {
            if (head == null)
                return;

            HashSet<T> uniqueValues = new HashSet<T>();
            Node<T> current = head;
            Node<T> previous = null;

            while (current != null)
            {
                if (uniqueValues.Contains(current.Data))
                {
                    previous.Next = current.Next;
                }
                else
                {
                    uniqueValues.Add(current.Data);
                    previous = current;
                }

                current = current.Next;

            }
        }

        public bool IsPalindrome()
        {
            if (head == null || head.Next == null)
                return true;

            // Find the middle of the list
            Node<T> slow = head;
            Node<T> fast = head;

            /*1 2 3 4 Null
             *S:1, F:1
             *1) S:2, F:3
             *S = 2*/

            /*1 2 3 4 5 Null
             *S:1, F:1
             *1) S:2, F:3
             *1) S:3, F:5
             *S = 3*/

            while (fast.Next != null && fast.Next.Next != null)
            {
                slow = slow.Next;
                fast = fast.Next.Next;
            }

            // Reverse the second half of the list
            Node<T> secondHalf = Reverse(slow.Next);
            Node<T> firstHalf = head;

            // Compare the two halves
            while (secondHalf != null)
            {
                if (!firstHalf.Data.Equals(secondHalf.Data))
                    return false;
                firstHalf = firstHalf.Next;
                secondHalf = secondHalf.Next;

            }
            return true;
        }
        public static LinkedList<T> MergeSortedLists<T>(LinkedList<T> list1, LinkedList<T> list2) where T : IComparable<T>
        {
            LinkedList<T> result = new LinkedList<T>();


            Node<T> current1 = list1.Head;
            Node<T> current2 = list2.Head;

            while (current1 != null && current2 != null)
            {
                if (current1.Data.CompareTo(current2.Data) <= 0)
                {
                    result.Append(current1.Data);
                    current1 = current1.Next;
                }
                else
                {
                    result.Append(current2.Data);
                    current2 = current2.Next;
                }
            }

            while (current1 != null)
            {
                result.Append(current1.Data);
                current1 = current1.Next;
            }

            while (current2 != null)
            {
                result.Append(current2.Data);
                current2 = current2.Next;
            }

            return result;
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {   
            Node<T> current = head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}