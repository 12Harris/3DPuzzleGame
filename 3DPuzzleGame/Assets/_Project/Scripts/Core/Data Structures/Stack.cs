using System;
using System.Collections.Generic;

namespace Vault.DataStrucures
{
    //Stacks operate under the LIFO principal
    class TStack<T>
    {

        public int Count => _stack.Count;

        // Create a new stack of integers
        private Stack<T> _stack = new Stack<T>();


        //push to stack
        public void Push(T item)
        {
            _stack.Push(item);
        }


        //return top of stack
        public T Pop()
        {
            return _stack.Pop();
        }


        //Return top of stack without returning it
        public T Peek()
        {
            return _stack.Peek();
        }

        public void Clear()
        {
            _stack.Clear();

        }

        public bool Contains(T value)
        {
            return _stack.Contains(value);
        }
    } 
}