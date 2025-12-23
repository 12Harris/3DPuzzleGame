using System;
using System.Collections;
using System.Collections.Generic;

namespace Vault.DataStrucures
{
    //Queues operate under the FIFO principal
    class TQueue<T> :  IEnumerable<T>
    {

        public int Count => _queue.Count;

        private Queue<T> _queue = new Queue<T>();


        //add to front of queue
        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }


        //return front of queke
        public T Dequeue()
        {
            return _queue.Dequeue();
        }


        //Return front of queue without returning it
        public T Peek()
        {
            return _queue.Peek();
        }

        public void Clear()
        {
            _queue.Clear();

        }

        public bool Contains(T value)
        {
            return _queue.Contains(value);
        }

        public IEnumerator<T> GetEnumerator()
        {   
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    } 
}