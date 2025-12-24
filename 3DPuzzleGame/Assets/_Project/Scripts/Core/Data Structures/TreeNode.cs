using UnityEngine;
using System;
using System.Linq;

// ===============================================================
// Generic Tree Node
// ===============================================================
namespace Vault.DataStrucures
{
    public class TreeNode<T>
    {
        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public LinkedList<TreeNode<T>> Children { get; set; }
        
        public TreeNode(T data)
        {
            Data = data;
            Children = new LinkedList<TreeNode<T>>();
            Parent = null;
        }
        
        public void AddChild(TreeNode<T> child)
        {
            child.Parent = this;
            Children.Append(child);
        }
        
        public void RemoveChild(TreeNode<T> child)
        {
            child.Parent = null;
            Children.Remove(child);
        }
        
        public bool IsLeaf => Children.Count == 0;
        public bool IsRoot => Parent == null;
        public int Level
        {
            get
            {
                int level = 0;
                TreeNode<T> current = this;
                while (current.Parent != null)
                {
                    level++;
                    current = current.Parent;
                }
                return level;
            }
        }
    }
}