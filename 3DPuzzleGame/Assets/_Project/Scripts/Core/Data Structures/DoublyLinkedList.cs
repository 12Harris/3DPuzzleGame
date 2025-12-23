using System;

namespace Vault.DataStrucures
{
    public class DoublyLinkedListNode<T>
    {
        public T Data { get; set; }
        public DoublyLinkedListNode<T> Previous { get; set; }
        public DoublyLinkedListNode<T> Next { get; set; }

        public DoublyLinkedListNode(T data)
        {
            Data = data;
            Previous = null;
            Next = null;
        }
    }

    public class DoublyLinkedList<T>
    {
        private DoublyLinkedListNode<T> head;
        private DoublyLinkedListNode<T> tail;

        public DoublyLinkedList()
        {
            head = null;
            tail = null;

        }

        public void Append(T data)
        {
            DoublyLinkedListNode<T> newNode = new DoublyLinkedListNode<T>(data);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Previous = tail;
                tail = newNode;
            }
        }

        public void Prepend(T data)
        {
            DoublyLinkedListNode<T> newNode = new DoublyLinkedListNode<T>(data);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                newNode.Next = head;
                head.Previous = newNode;
                head = newNode;
            }
        }


        public bool Remove(T data)
        {
            DoublyLinkedListNode<T> current = head;

            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    if (current.Previous != null)
                        current.Previous.Next = current.Next;
                    else
                        head = current.Next;

                    if (current.Next != null)
                        current.Next.Previous = current.Previous;
                    else
                        tail = current.Previous;
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        public void TraverseForward(Action<T> action)
        {
            DoublyLinkedListNode<T> current = head;
            while (current != null)
            {
                action(current.Data);
                current = current.Next;
            }
        }

        public void TraverseBackward(Action<T> action)
        {
            DoublyLinkedListNode<T> current = tail;
            while (current != null)
            {
                action(current.Data);
                current = current.Previous;
            }
        }

    }
}