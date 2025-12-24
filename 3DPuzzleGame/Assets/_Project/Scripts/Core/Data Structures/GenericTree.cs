using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

// ===============================================================
// Generic Tree
// ===============================================================

namespace Vault.DataStrucures
{
    public class Tree<T>
    {
        public TreeNode<T> Root { get; private set; }
        
        public Tree(T rootData)
        {
            Root = new TreeNode<T>(rootData);
        }
        
        // Depth-First Search (DFS) - Preorder
        //The Preorder Traversal visits the nodes in the following order: Root, Left, Right.
        //Erst wird diw Wurzel durchlaufen, dann der linke Teilbaum, dann der rechte Teilbaum
        public void TraversePreOrder(TreeNode<T> node, Action<TreeNode<T>> action)
        {
            if (node == null) return;
            
            action(node);
            foreach (var child in node.Children)
            {
                TraversePreOrder(child.Data, action);//child.Data is of type TreeNode<T>
            }
        }
        
        // Depth-First Search (DFS) - Postorder
        //The Postorder Traversal visits the nodes in the following order: Left, Right, Root
        public void TraversePostOrder(TreeNode<T> node, Action<TreeNode<T>> action)
        {
            if (node == null) return;
            
            foreach (var child in node.Children)
            {
                TraversePostOrder(child.Data, action);//child.Data is of type TreeNode<T>
            }
            action(node);
        }
        
        // Breadth-First Search (BFS) - Level Order
        public void TraverseLevelOrder(Action<TreeNode<T>> action)
        {
            if (Root == null) return;
            
            TQueue<TreeNode<T>> queue = new TQueue<TreeNode<T>>();
            queue.Enqueue(Root);
            
            while (queue.Count > 0)
            {
                TreeNode<T> current = queue.Dequeue();
                action(current);
                
                foreach (var child in current.Children)
                {
                    queue.Enqueue(child.Data);
                }
            }
        }
        
        // Find node by data
        public TreeNode<T> Find(T data)
        {
            return FindRecursive(Root, data);
        }
        
        private TreeNode<T> FindRecursive(TreeNode<T> node, T data)
        {
            if (node == null) return null;
            if (EqualityComparer<T>.Default.Equals(node.Data, data))
                return node;
            
            foreach (var child in node.Children)
            {
                var found = FindRecursive(child.Data, data);
                if (found != null) return found;
            }
            
            return null;
        }
        
        // Get all leaves
        public List<TreeNode<T>> GetLeaves()
        {
            List<TreeNode<T>> leaves = new List<TreeNode<T>>();
            GetLeavesRecursive(Root, leaves);
            return leaves;
        }
        
        private void GetLeavesRecursive(TreeNode<T> node, List<TreeNode<T>> leaves)
        {
            if (node == null) return;
            
            if (node.IsLeaf)
            {
                leaves.Add(node);
            }
            else
            {
                foreach (var child in node.Children)
                {
                    GetLeavesRecursive(child.Data, leaves);
                }
            }
        }
        
        // Get tree height
        public int GetHeight()
        {
            return GetHeightRecursive(Root);
        }
        
        private int GetHeightRecursive(TreeNode<T> node)
        {
            if (node == null || node.IsLeaf) return 0;
            
            int maxHeight = 0;
            foreach (var child in node.Children)
            {
                int height = GetHeightRecursive(child.Data);
                maxHeight = Math.Max(maxHeight, height);
            }
            
            return maxHeight + 1;
        }
        
        // Count total nodes
        public int CountNodes()
        {
            return CountNodesRecursive(Root);
        }
        
        private int CountNodesRecursive(TreeNode<T> node)
        {
            if (node == null) return 0;
            
            int count = 1;
            foreach (var child in node.Children)
            {
                count += CountNodesRecursive(child.Data);
            }
            
            return count;
        }
        
        // Print tree structure
        public void PrintTree()
        {
            PrintTreeRecursive(Root, "", true);
        }
        
        private void PrintTreeRecursive(TreeNode<T> node, string indent, bool last)
        {
            if (node == null) return;
            
            Console.Write(indent);
            if (last)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "│ ";
            }
            
            Console.WriteLine(node.Data);
            
            for (int i = 0; i < node.Children.Count; i++)
            {
                PrintTreeRecursive(node.Children[i], indent, i == node.Children.Count - 1);
            }
        }
    }
}