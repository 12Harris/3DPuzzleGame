
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using Unity.Multiplayer.Center.Common;
using Unity.VisualScripting;
using UnityEngine;
namespace Vault.DataStrucures
{

// ===============================================================
// Vec2 structure
// ===============================================================
public class Vec2 : IComparable<Vec2>
{
    public float X;
    public float Y;

    public Vec2(float x, float y)
    {
        X = x;
        Y = y;
    }

    // Common vectors
    public static Vec2 Zero => new Vec2(0f, 0f);
    public static Vec2 One => new Vec2(1f, 1f);
    public static Vec2 Up => new Vec2(0f, 1f);
    public static Vec2 Right => new Vec2(1f, 0f);

    // Magnitude
    public float Length => MathF.Sqrt(X * X + Y * Y);
    public float LengthSquared => X * X + Y * Y;

    // Normalize
    public Vec2 Normalized()
    {
        float length = Length;
        return length > 0f ? this / length : Zero;
    }

    // Dot product
    public static float Dot(Vec2 a, Vec2 b)
        => a.X * b.X + a.Y * b.Y;

    // Distance
    public static float Distance(Vec2 a, Vec2 b)
        => (a - b).Length;


    //Comparison
    public int CompareTo(Vec2 other)
    {
        if(X < other.X)
            return -1;
        if(X > other.X)
            return 1;
        return 0;
    }

    // Operators
    public static Vec2 operator +(Vec2 a, Vec2 b)
        => new Vec2(a.X + b.X, a.Y + b.Y);

    public static Vec2 operator -(Vec2 a, Vec2 b)
        => new Vec2(a.X - b.X, a.Y - b.Y);

    public static Vec2 operator *(Vec2 v, float scalar)
        => new Vec2(v.X * scalar, v.Y * scalar);

    public static Vec2 operator /(Vec2 v, float scalar)
        => new Vec2(v.X / scalar, v.Y / scalar);

    public override string ToString()
        => $"({X}, {Y})";
}
    // ===============================================================
    // Binary Tree Node
    // ===============================================================

    [System.Serializable]
    public class BinaryTreeNode<T>
    {
        [SerializeField]
        private T _data;
        public T Data { get => _data; set => _data=value; }


        [SerializeField] private BinaryTreeNode<T> _left;

        [SerializeField] private BinaryTreeNode<T> _right;

        
        public BinaryTreeNode<T> Left { get=>_left; set=>_left=value; }
        public BinaryTreeNode<T> Right { get=>_right; set=>_right=value; }
        public int Level {get;set;} = 0;
        public int LevelIndex{get;private set;}
        public BinaryTreeNode<T> Parent{get;set;} = null;

        public void GetLevelIndex()
        {
            if(Parent == null)
            {
                LevelIndex = 0;
            }
            else
            {
                if(this == Parent.Left)
                    LevelIndex = Parent.LevelIndex*2;
                else
                    LevelIndex = Parent.LevelIndex*2+1;
            }
        }
        
        public BinaryTreeNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
        
        public bool IsLeaf => Left == null && Right == null;

        [HideInInspector]
        public int LeafIndex = -1;
    }

    // ===============================================================
    // Binary Search Tree
    // ===============================================================
    [System.Serializable]
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        public BinaryTreeNode<T> Root { get; private set; }

        private int _levels = 0;
        public int Levels => _levels;

        private int _currentLeaf = 0;
        
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
            {
                Debug.Log(data +  " < "  + node.Data);
                node.Left = InsertRecursive(node.Left, data);
                node.Left.Parent = node;
                node.Left.GetLevelIndex();
            }
            else if (comparison > 0)
            {
                Debug.Log(data +  " > "  + node.Data);
                node.Right = InsertRecursive(node.Right, data);
                node.Right.Parent = node;
                node.Right.GetLevelIndex();
            }
            
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
        public void PreorderTraversal<T2>(Action<T2> action, Func<BinaryTreeNode<T>,T2> func)
        {
            PreorderRecursive(Root, action, func);
        }
        

        private void PreorderRecursive<T2>(BinaryTreeNode<T> node, Action<T2> action, Func<BinaryTreeNode<T>,T2> func)
        {
            if (node == null) return;
            action(func(node));
            PreorderRecursive(node.Left, action, func);
            PreorderRecursive(node.Right, action, func);
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

        // Postorder Traversal (Left, Right, Root)
        public void PostorderTraversal<T2>(Action<T2> action, Func<BinaryTreeNode<T>,T2> func)
        {
            PostorderRecursive(Root, action, func);
        }
        

        private void PostorderRecursive<T2>(BinaryTreeNode<T> node, Action<T2> action, Func<BinaryTreeNode<T>,T2> func)
        {
            if (node == null) return;
            
            PostorderRecursive(node.Left, action, func);
            PostorderRecursive(node.Right, action, func);
            action(func(node));
        }
        
        // Level Order Traversal (BFS)
        public void LevelOrderTraversal(Action<T> action)
        {
            if (Root == null) return;
            
            Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(Root);
            _levels = 0;
            var nodesOnLevel = 1;
            var countedNodesOnLevel = 0;
            var nullNodes = 0;
            var totalNodes = CountNodes();
            var totalNodesCounted = 0;
            while (queue.Count > 0)
            {
                BinaryTreeNode<T> current = queue.Dequeue();
                

                if(current != null)
                {
                    current.Level = _levels;
                    Debug.Log("current.level: " + current.Level + "(" + current.Data + ")");
                    if(action != null) action(current.Data);
                    countedNodesOnLevel++;
                    totalNodesCounted++;
                    Debug.Log("Current data = " + current.Data + ", countedNodes = " + countedNodesOnLevel + ", nodesonlevel = " + nodesOnLevel);
                    if(countedNodesOnLevel == nodesOnLevel && totalNodesCounted < totalNodes)//countedNodesOnLevel+nullNodes=4, nodesonlevel = 3
                    {
                        Debug.Log("Nodes On Level " + _levels + ": " + nodesOnLevel);
                        _levels++;
                        countedNodesOnLevel = 0;
                        nodesOnLevel = (int)Math.Pow(2,_levels);
                        nullNodes = 0;
                    }
    
                    if(totalNodesCounted < totalNodes)queue.Enqueue(current.Left);
                    if(totalNodesCounted < totalNodes)queue.Enqueue(current.Right);
                    
                    
                }
                else
                {
                    nodesOnLevel--;
                    nullNodes++;
                    if(totalNodesCounted < totalNodes)queue.Enqueue(null);
                    if(totalNodesCounted < totalNodes)queue.Enqueue(null);
                }   
            }
            Debug.Log("levels total: " + _levels + "node count: "+ totalNodes);
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


        // Count total nodes
        public int CountNodes()
        {
            return CountNodesRecursive(Root);
        }
        

        // Count total nodes of subTree
        public int CountNodes(BinaryTreeNode<T> node)
        {
            return CountNodesRecursive(node);
        }
        
        private int CountNodesRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return 0;
            
            int count = 1;

            count += CountNodesRecursive(node.Left);
            count += CountNodesRecursive(node.Right);
            
            return count;
        }


        public List<BinaryTreeNode<T>> GetNodesAtLevel(BinaryTreeNode<T> root,int targetLevel)
        {
            var result = new List<BinaryTreeNode<T>>();
            Collect(root, 0, targetLevel, result);
            return result;  
        }

        private void Collect(BinaryTreeNode<T> node,int currentLevel,int targetLevel, List<BinaryTreeNode<T>> result)
        {
            if (node == null)
                return;

            if (currentLevel == targetLevel)
            {
                result.Add(node);
                Debug.Log("adding node to collection!" + node.Data);
                return;
            }

            Collect(node.Left, currentLevel + 1, targetLevel, result);
            if(result.Count > 0) Debug.Log("checked left tree of level " + node.Level  + " node");
            Collect(node.Right, currentLevel + 1, targetLevel,  result);
            if(result.Count > 0) Debug.Log("checked right tree of level " + node.Level  + " node");

     
        }


        public BinarySearchTree<Vec2> GenerateNodeDisplayTree()
        {
            BinarySearchTree<Vec2> displayTree = new BinarySearchTree<Vec2>();
            _currentLeaf = 0;//important
            PreorderTraversal(displayTree.Insert, GetNodeDisplayPosition);
            Debug.Log("display tree has: " + displayTree.CountNodes() + " nodes with values: " + displayTree.Root.Data);
            return displayTree;

        }

        public float GetBottomNodeDisplayDistance()
        {
            return 500/(int)Math.Pow(2,_levels);
        }

        public float GetNodeVDisplayDistanceAbsolute(int level)
        {
            float distance = 0;
            for(int i = 1; i <= level; i++)
            {
                distance += (300+level*70)/(int)Math.Pow(2,i);//level 1:50, level 2: 75
            }
            return distance;
        }

        public Vec2 GetNodeDisplayPosition(BinaryTreeNode<T> node)
        {   
            Debug.Log("node level: " + node.Level);
            return GetNodeDisplayPositionRec(node) + new Vec2(300,0);
        }

        private Vec2 GetNodeDisplayPositionRec(BinaryTreeNode<T> node)
        {

            if(node.IsLeaf)
            {
                return  GetNodeDisplayPositionRec(node.Level, node.LevelIndex);
            }

            else
            {
                Vec2 leftPos = Vec2.Zero;
                Vec2 rightPos = Vec2.Zero;

                if(node.Left == null)
                    leftPos = GetNodeDisplayPositionRec(node.Level+1, node.LevelIndex*2);
                else    
                    leftPos = GetNodeDisplayPositionRec(node.Left);

                if(node.Right == null)
                    rightPos = GetNodeDisplayPositionRec(node.Level+1, node.LevelIndex*2+1);
                else    
                    rightPos = GetNodeDisplayPositionRec(node.Right);

                Vec2 middlePos = Vec2.Zero;

                if(leftPos.X > 0)
                    middlePos = leftPos + (rightPos-leftPos)/2;
                else
                {
                    if(rightPos.X < 0)
                        middlePos = leftPos - (leftPos-rightPos)/2;
                    else
                        middlePos = leftPos+rightPos;
                }
                
                Debug.Log("AAleftpos: " + leftPos.X);
                Debug.Log("AA rightpos " + rightPos.X);
                Debug.Log("AA middlepos x (no leaf): " + middlePos.X);
                return new Vec2(middlePos.X, GetNodeVDisplayDistanceAbsolute(node.Level));
            }
        }

        //Called only for leaf nodes
        private Vec2 GetNodeDisplayPositionRec(int level, int levelIndex)
        {
            if(level < _levels)
            {
                var leftPos = GetNodeDisplayPositionRec(level+1,levelIndex*2);
                var rightPos = GetNodeDisplayPositionRec(level+1, levelIndex*2+1);
                Vec2 middlePos = Vec2.Zero;

                if(leftPos.X > 0)
                    middlePos = leftPos + (rightPos-leftPos)/2;
                else
                {
                    if(rightPos.X < 0)
                        middlePos = leftPos - (leftPos-rightPos)/2;
                    else
                        middlePos = leftPos+rightPos;
                }
                

                Debug.Log("middlepos x: " + middlePos.X);
                return new Vec2(middlePos.X, GetNodeVDisplayDistanceAbsolute(level));
            }
            else
            {   
                var temp = (int)Math.Pow(2,level)/2;
                if(levelIndex < temp)//left side
                {
                    Debug.Log("node is to left");
                    return new Vec2((levelIndex - temp)*GetBottomNodeDisplayDistance(), GetNodeVDisplayDistanceAbsolute(level));
                }
                else
                {
                    Debug.Log("node is to right");
                    return new Vec2((levelIndex - temp+1)*GetBottomNodeDisplayDistance(), GetNodeVDisplayDistanceAbsolute(level));
                }
            }
        }       

        // Print tree structure
        public void PrintTree()
        {
            PrintTreeRecursive(Root, "", true);
        }
        
        private void PrintTreeRecursive(BinaryTreeNode<T> node, string indent, bool last)
        {
            if (node == null) return;

            string result = "";
            
            result+=indent;
            if (last)
            {
                result+= "└─";
                result+= "  ";
            }
            else
            {
                result+= "├─";
                result+= "│ ";
            }
            
            result += node.Data + "Level: " + node.Level +"\n";
            Debug.Log(result);
            
            
            PrintTreeRecursive(node.Left, indent, false);
            PrintTreeRecursive(node.Right, indent, true);
        }
    }
}