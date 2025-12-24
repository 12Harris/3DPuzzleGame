using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor.Rendering;

namespace Vault.DataStrucures
{
    //Queues operate under the FIFO principal

    //Example

    /* P1
     * A B C D
     * P2
     * E F G H
     * P3
     * I J K L
     * P4
     * M N O P  
     */

     /**
     *This class holds a collection of queues with associated priority
     */
    class TPriorityQueue<T>
    {

        private SortedDictionary<int, TQueue<T>> _priorityMap = new SortedDictionary<int, TQueue<T>>();

        //returns sum of all elements of all queues in the priority map
        public int Count => _priorityMap.Values.Sum(q => q.Count);

        //returns count of all queues in the priority map
        public int QueueCount => _priorityMap.Values.Count;


        //add to front of queue with given priority
        public void Enqueue(T item, int priority)
        {
            if (!_priorityMap.ContainsKey(priority))
            {
                _priorityMap[priority] = new TQueue<T>();
            }
            _priorityMap[priority].Enqueue(item);
        }
        
        //add multiple values to front of queue with given priority
        public void Enqueue(int priority, params T[] values)
        {
            foreach(var value in values)
                Enqueue(value, priority);
        }


        //returns and removes front element of queue with highest piority
        public T Dequeue()
        {
            if (_priorityMap.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            T item = _priorityMap[0].Dequeue();

            //Reassign the priorities to the number of queues-1
            //and remove the last queue(queue with lowes priority)
            if (_priorityMap[0].Count == 0)
            {
                _priorityMap.Remove(0);

                for(int i = 0; i < QueueCount-1; i++)
                {
                    _priorityMap[i] = _priorityMap[i+1];
                }
                _priorityMap.Remove(QueueCount-1);
            }

            return item;

        }

        //only returns front element of queue with highest piority
        public T Peek()
        {
            if (_priorityMap.Count == 0)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            return _priorityMap[0].Peek();
        }

        //Clears all queues
        public void Clear()
        {
            _priorityMap.Clear();
        }

        public bool Contains(T value)
        {
            foreach (KeyValuePair<int, TQueue<T>> kvp in _priorityMap)
            {
                if (kvp.Value.Contains(value))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString ()
        {
            string result = "";
            int count;
            foreach (KeyValuePair<int, TQueue<T>> kvp in _priorityMap)
            {
                result += "Priority: " + kvp.Key +"{\n";
                result += "\tValues: ";
                count = 0;
                foreach(var v in kvp.Value)
                {
                    count++;
                    result += count < kvp.Value.Count ? v + ", " : v + "\n}\n\n";
                }
            }
            return result;
        }
    } 
}