using System.Collections.Generic;

namespace Vault.DataStrucures
{
    public class CustomSet<T>
    {
        private HashSet<T> items;

        public CustomSet()
        {
            items = new HashSet<T>();
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public CustomSet<T> Union(CustomSet<T> other)
        {
            CustomSet<T> result = new CustomSet<T>();
            result.items.UnionWith(this.items);
            result.items.UnionWith(other.items);
            return result;
        }

        public CustomSet<T> Intersect(CustomSet<T> other)
        {
            CustomSet<T> result = new CustomSet<T>();
            result.items.UnionWith(this.items);
            result.items.IntersectWith(other.items);
            return result;
        }

        public CustomSet<T> Except(CustomSet<T> other)
        {
            CustomSet<T> result = new CustomSet<T>();
            result.items.UnionWith(this.items);
            result.items.ExceptWith(other.items);
            return result;

        }

        public override string ToString()
        {
            return "{" + string.Join(", ", items) + "}";
        }
    }
}