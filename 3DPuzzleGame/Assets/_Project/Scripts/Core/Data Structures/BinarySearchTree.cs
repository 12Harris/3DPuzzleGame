
using System;
using System.Collections.Generic;
using System.Linq;

namespace Vault.DataStrucures
{

    // ===============================================================
    // Binary Tree Node
    // ===============================================================
    public class BinaryTreeNode<T>
    {
        public T Data { get; set; }
        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }
        
        public BinaryTreeNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
        
        public bool IsLeaf => Left == null && Right == null;
    }

    // ===============================================================
    // Binary Search Tree
    // ===============================================================
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        public BinaryTreeNode<T> Root { get; private set; }
        
        public void Insert(T data)
        {
            Root = InsertRecursive(Root, data);
        }
        
        private BinaryTreeNode<T> InsertRecursive(BinaryTreeNode<T> node, T data)
        {
            if (node == null)
                return new BinaryTreeNode<T>(data);
            
            int comparison = data.CompareTo(node.Data);
            
            if (comparison < 0)
                node.Left = InsertRecursive(node.Left, data);
            else if (comparison > 0)
                node.Right = InsertRecursive(node.Right, data);
            
            return node;
        }
        
        public bool Search(T data)
        {
            return SearchRecursive(Root, data);
        }
        
        private bool SearchRecursive(BinaryTreeNode<T> node, T data)
        {
            if (node == null) return false;
            
            int comparison = data.CompareTo(node.Data);
            
            if (comparison == 0) return true;
            if (comparison < 0) return SearchRecursive(node.Left, data);
            return SearchRecursive(node.Right, data);
        }
        
        public void Delete(T data)
        {
            Root = DeleteRecursive(Root, data);
        }
        
        private BinaryTreeNode<T> DeleteRecursive(BinaryTreeNode<T> node, T data)
        {
            if (node == null) return null;
            
            int comparison = data.CompareTo(node.Data);
            
            if (comparison < 0)
            {
                node.Left = DeleteRecursive(node.Left, data);
            }
            else if (comparison > 0)
            {
                node.Right = DeleteRecursive(node.Right, data);
            }
            else
            {
                // Node with only one child or no child
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;
                
                // Node with two children
                node.Data = FindMin(node.Right).Data;
                node.Right = DeleteRecursive(node.Right, node.Data);
            }
            
            return node;
        }
        
        private BinaryTreeNode<T> FindMin(BinaryTreeNode<T> node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }
        
        // Inorder Traversal (Left, Root, Right) - Returns sorted order
        public void InorderTraversal(Action<T> action)
        {
            InorderRecursive(Root, action);
        }
        
        private void InorderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            
            InorderRecursive(node.Left, action);
            action(node.Data);
            InorderRecursive(node.Right, action);
        }
        
        // Preorder Traversal (Root, Left, Right)
        public void PreorderTraversal(Action<T> action)
        {
            PreorderRecursive(Root, action);
        }
        
        private void PreorderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            
            action(node.Data);
            PreorderRecursive(node.Left, action);
            PreorderRecursive(node.Right, action);
        }
        
        // Postorder Traversal (Left, Right, Root)
        public void PostorderTraversal(Action<T> action)
        {
            PostorderRecursive(Root, action);
        }
        
        private void PostorderRecursive(BinaryTreeNode<T> node, Action<T> action)
        {
            if (node == null) return;
            
            PostorderRecursive(node.Left, action);
            PostorderRecursive(node.Right, action);
            action(node.Data);
        }
        
        // Level Order Traversal (BFS)
        public void LevelOrderTraversal(Action<T> action)
        {
            if (Root == null) return;
            
            Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(Root);
            
            while (queue.Count > 0)
            {
                BinaryTreeNode<T> current = queue.Dequeue();
                action(current.Data);
                
                if (current.Left != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
        }
        
        public int GetHeight()
        {
            return GetHeightRecursive(Root);
        }
        
        private int GetHeightRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return -1;
            
            int leftHeight = GetHeightRecursive(node.Left);
            int rightHeight = GetHeightRecursive(node.Right);
            
            return Math.Max(leftHeight, rightHeight) + 1;
        }
    }
}