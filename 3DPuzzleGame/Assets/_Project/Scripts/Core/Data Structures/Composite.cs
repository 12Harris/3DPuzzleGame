using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

namespace Treshhold.DataStrucures
{

    public interface IComponent<T>
    {
        void Add(IComponent<T> c);
        IComponent<T> Remove(T s);
        IComponent<T> Find(T s);
        string Display(int depth);
        T Name { get; set; }
        IComponent<T> Start();

        IComponent<T> Parent { get; set; }

        static int Count { get; set; } = 0;
        int ID { get; set; }
        int Level { get; set; }
        void AssignLevels(int level);

        void Update(in float dt);

        public static bool operator >(IComponent<T> lhs, IComponent<T> rhs)
        {

            return lhs.ID > rhs.ID;
        }

        public static bool operator <(IComponent<T> lhs, IComponent<T> rhs)
        {

            return lhs.ID < rhs.ID;
        }
    }

    [Serializable]
    // The Component (atomic)
    public class Component<T> : IComponent<T>
    {
        public T Name { get; set; }

        public int ID { get; set; } = 0;

        public static int Count { get; set; } = 1;

        public int Level { get; set; } = 0;

        public IComponent<T> Parent { get; set; } = null;

        public Component(T name)
        {
            Name = name;
        }
        public void Add(IComponent<T> c)
        {
            Debug.Log("Cannot add to an item");
        }
        public IComponent<T> Remove(T s)
        {
            Debug.Log("Cannot remove directly");
            return this;
        }
        public string Display(int depth)
        {
            return new String('-', depth) + Name + "\n";
        }
        public IComponent<T> Find(T s)
        {
            if (s.Equals(Name))
                return this;
            else
                return null;
        }

        public IComponent<T> Start()
        {
            return this;
        }

        public virtual void Update(in float dt)
        {

        }

        public void GenerateID(int id)
        {
            ID = id;
            Debug.Log("assigned id : " + ID);
        }

        public void AssignLevels(int level)
        {
            Level = level;
        }
    }

    [Serializable]
    // The Composite
    public class Composite<T> : IComponent<T>
    {
        public List<IComponent<T>> list;
 
        private static int ComponentCount = 0;
        public int ID { get; set; } = 0;

        public static int Count { get; set; } = 0;

        public static int CurrentID { get; set; } = 0;

        public int Level { get; set; } = 0;

        public T Name { get; set; }
        public IComponent<T> Parent { get; set; } = null; 


        public IComponent<T> this[int index]
		{
			// Getter
			get
			{
                return list[index];
			}
			// Setter
			set
			{
				list[index] = value;
			}
    	}

        public Composite()
        {

            Parent = null;
            list = new List<IComponent<T>>();
            Count = 0;
            Debug.Log("created composite");
            Name = default;
        }

        //ToDo: 2 add methods: 1 for adding composite,
        //another for adding atomic Component
        public void Add(IComponent<T> c)
        {
            c.Parent = this;
            list.Add(c);
            Count++;
        }

        IComponent<T> holder = null;
        // Finds the item from a particular point in the structure
        // and returns the composite from which it was removed
        // If not found, return the point as given
        public IComponent<T> Remove(T s)
        {
            holder = this;
            IComponent<T> p = holder.Find(s);
            if (holder != null)
            {
                Debug.Log("found holder, removing item");
                (holder as Composite<T>).list.Remove(p);
                Count--;
                return holder;
            }
            else
            {
                Debug.Log("Couldnt find holder, NOT removing item");
                return this;
            }
        }

        public void Clear()
        {
            while (Count > 0)
            {
                Debug.Log("Count updated: " + Count);

                Debug.Log("list 0: " + list[0].Name);
                Remove(list[0].Name);
            }
            Debug.Log("Count after clearing:  " + Count);
        }

        // Recursively looks for an item
        // Returns its reference or else null
        public IComponent<T> Find(T s)
        {
            holder = this;
            IComponent<T> found = null;
            foreach (IComponent<T> c in list)
            {
                found = c.Find(s);
                if (found != null)
                    break;
            }
            return found;
        }

        public IComponent<T> Start()
        {
            IComponent<T> start = null;

            start = list[0].Start();

            return start;
        }

        public virtual void Update(in float dt)
        {

        }

        /*public void GenerateID(int id)
        {
            ID = id;
            
            int i = id;

            Debug.Log("assigned id = " + ID);

            foreach (IComponent <T> component in list) {
                
                i++;

                if(component is Composite<T>)
                    (component as Composite<T>).GenerateID(i);
                else
                    (component as Component<T>).GenerateID(i);
                
            }

            Count = i;
            
        }*/


        public int GenerateID(int id)
        {
            ID = id;

            int l = id;

            Debug.Log("assigned id : " + ID);

            foreach (IComponent<T> component in list)
            {

                if (component is Composite<T>)
                {
                    l += (component as Composite<T>).GenerateID(l + 1) + 1;
                    //Count += l;

                }
                else
                {
                    l++;
                    (component as Component<T>).GenerateID(l);
                    //Count++;
                }

            }
            //return list.Count;
            return l - id;

        }


        public void AssignLevels(int level)
        {
            Level = level;

            Debug.Log("assigned level = " + level);
            
            level = 1;
            foreach (IComponent<T> component in list)
            {
                //level += 1;
                component.AssignLevels(level);
            }

        }

        // Displays items in a format indicating their level in the composite structure
        public string Display(int depth)
        {
            StringBuilder s = new StringBuilder(new String('-', depth));
            s.Append("Set " + Name + " length :" + list.Count + "\n");
            foreach (IComponent<T> component in list)
            {
                s.Append(component.Display(depth + 2));
            }
            return s.ToString();
        }

        public static void AddToArray(List<IComponent<T>> ls, IComponent<T> component)
        {
            ls.Add(component);

            if (component is Composite<T>)
            {
                foreach (IComponent<T> child in (component as Composite<T>).list)
                {
                    CurrentID++;
                    AddToArray(ls, child);
                }
            }
        }

        public static List<IComponent<T>> ToArray(Composite<T> root)
        {
            List<IComponent<T>> ls = new List<IComponent<T>>();

            ls.Add(root);

            CurrentID = 1;

            foreach (IComponent<T> component in root.list)
            {

                AddToArray(ls, component);
            }

            return ls;
        }
    }
}