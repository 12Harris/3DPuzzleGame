using UnityEngine;
using System;
using System.Collections.Generic;

namespace Vault.DataStrucures
{

    public class BinaryTree<T>
    {

        public class TreeNode
        {
            public T Data { get; set; }
            public TreeNode Left;
            public TreeNode Right;

            public int Level { get; set; }

            //The level index of this node
            public int Index { get; set; }

            //The maximum level, i.e. the level of a leaf node
            public static int MaxLevel { get; set; } = 0;

            // static int LeafSiblingDistance => (int)(MaxLevel * Math.Pow(0.75, MaxLevel));

            public static int LeafSiblingDistance => 8;

            public int DisplayOffset { get; set; } = 0;

            public TreeNode(T data)
            {
                Data = data;
                Left = null;
                Right = null;
                Level = 0;
            }

            public override string ToString()
            {
                return Data.ToString();
            }
        }


        public TreeNode Root;

        public TreeNode Iter;


        public int CurrentLevel { get; set; } = 0;
        public int MaxLevels { get; set; } = 0;

        public int Count { get; set; } = 0;

        public bool CalculateDisplayOffset = true;

        public static int LeafSiblingOffset { get; set; } = 0;

        public BinaryTree()
        {
            Root = null;
        }

        private string _repr = "";
        public string Repr => _repr;

        public void InOrderTraversal(TreeNode node, Action<T> action)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, action);
                //Debug.Log("Node: " + node.Data + " ");
                _repr += node.Data + " ";
                action(node.Data);
                InOrderTraversal(node.Right, action);
            }
        }


        public void PreOrderTraversal(TreeNode node, Action<T> action)
        {
            if (node != null)
            {
                _repr += node.Data + " ";
                action(node.Data);
                PreOrderTraversal(node.Left, action);
                PreOrderTraversal(node.Right, action);
            }
        }

        public void PostOrderTraversal(TreeNode node, Action<T> action)
        {
            if (node != null)
            {
                PostOrderTraversal(node.Left, action);
                PostOrderTraversal(node.Right, action);
                action(node.Data);
                _repr += node.Data + " ";
            }
        }


        public List<TreeNode> LevelOrderTraversal(Action<T> action, int maxLevel = -1)
        {
            if (Root == null) return null;

            TQueue<TreeNode> queue = new TQueue<TreeNode>();
            queue.Enqueue(Root);

            int counter = 0;
            List<TreeNode> result = new List<TreeNode>();

            TreeNode node = Root;

            while (queue.Count > 0 && counter < (int)Math.Pow(2, node.Level))
            {
                node = queue.Dequeue();

                //extract all nodes at maxLevel
                if (node.Level == maxLevel)
                {
                    counter++;
                    if (counter <= (int)Math.Pow(2, node.Level))
                    {
                        result.Add(node);
                    }
                }

                if(action != null)
                    action(node.Data); //AddToList(node.Data)

                if (node.Left != null)
                    queue.Enqueue(node.Left);
                if (node.Right != null)
                    queue.Enqueue(node.Right);
            }

            Debug.Log("levels list count: " + result.Count);
            return result;

        }

        public void Append(T data)
        {
            Count++;
            var sum = 0;
            for (int i = 0; i <= CurrentLevel; i++)
                sum += (int)Math.Pow(2, i);


            //Calculate level of next new Tree Node
            if (Count > sum)
            {
                Debug.Log("incrementing current level: " + CurrentLevel);
                CurrentLevel++;
                TreeNode.MaxLevel++;
            }


            //Create new Tree Node
            var node = new TreeNode(data);
            node.Level = CurrentLevel;
            node.Index = Count - (int)Math.Pow(2, node.Level);

            if (Root == null)
            {
                Debug.Log("Root is null, adding root");
                Root = node;
            }
            else
            {
                //Traverse the tree until we find a parent for the new node
                var parentList = LevelOrderTraversal(null, CurrentLevel - 1);
                string temp = "";
                foreach (var parent in parentList)
                {
                    temp += "parent: " + parent.Data + "(level: " + parent.Level + "), ";
                }
                Debug.Log(temp);

                TreeNode nextAvailableParent = null;
                foreach (var parent in parentList)
                {
                    if (parent.Left == null || parent.Right == null)
                    {
                        nextAvailableParent = parent;
                        break;
                    }
                }
                if (nextAvailableParent.Left == null)
                {
                    nextAvailableParent.Left = node;
                }
                else
                {
                    nextAvailableParent.Right = node;
                }
            }

        }


        public virtual void AppendRange(List<T> data)
        {
            foreach (var item in data)
            {
                //Root = InsertRec(Root, item);
                Append(item);
            }
        }

        public virtual void Insert(T data)
        {
            Root = InsertRec(Root, data);
        }

        //Insert Range of values
        public virtual void InsertRange(List<T> data)
        {
            foreach (var item in data)
            {
                //Root = InsertRec(Root, item);
                Insert(item);
            }
        }

        private TreeNode InsertRec(TreeNode root, T data)
        {
            if (root == null)
            {
                Count++;
                root = new TreeNode(data);
                root.Level = CurrentLevel;
                Debug.Log("TREENODE LEVEL: " + root.Level);
                root.Index = Count - (int)Math.Pow(2, root.Level);
                //Update tree level
   
                var sum = 0;
                for (int i = 0; i <= CurrentLevel; i++)
                {
                    sum += (int)Math.Pow(2, i);
                }

                if (Count > sum)
                {
                    CurrentLevel++;
                    TreeNode.MaxLevel++;
                }

                return root;
            }

            if (new System.Random().Next(2) == 0)
                root.Left = InsertRec(root.Left, data);
            else
                root.Right = InsertRec(root.Right, data);

            return root;
        }

        public int Height()
        {
            return HeightRec(Root);
        }

        private int HeightRec(TreeNode node)
        {
            if (node == null)
                return 0;

            int leftHeight = HeightRec(node.Left);
            int rightHeight = HeightRec(node.Right);

            return Math.Max(leftHeight, rightHeight) + 1;
        }

        public bool IsBalanced()
        {
            return IsBalancedRec(Root) != -1;
        }

        private int IsBalancedRec(TreeNode node)
        {
            if (node == null)
                return 0;

            int leftHeight = IsBalancedRec(node.Left);
            if (leftHeight == -1)
                return -1;

            int rightHeight = IsBalancedRec(node.Right);
            if (rightHeight == -1)
                return -1;

            if (Math.Abs(leftHeight - rightHeight) > 1)
                return -1;

            return Math.Max(leftHeight, rightHeight) + 1;
        }

        //calculates the count of given subtree
        public int GetCount(TreeNode node)
        {
            if (node != null)
            {
                return 1 + GetCount(node.Left) + GetCount(node.Right);
            }
            return 0;
        }


        //Display tree using level order traversal
        public void Display()
        {
            Debug.Log("max level: " + TreeNode.MaxLevel);
            GetDisplayOffset(ref Root);
   
            if (Root == null)
                Debug.Log("root is null???");
            Debug.Log("Root display offset: " + Root.DisplayOffset);

            if (Root == null) return;

            TQueue<TreeNode> queue = new TQueue<TreeNode>();
            queue.Enqueue(Root);

            var currentLevel = 0;
            var displayOffsetSum = 0;
            var prevDisplayOffset = 0;

            string display = "=>";

            while (queue.Count > 0)
            {
                TreeNode node = queue.Dequeue();

                if (node.Index < (int)Math.Pow(2, node.Level))
                {
                    Debug.Log("oki");
                    for (int i = 0; i < node.DisplayOffset - prevDisplayOffset; i++)
                    {
                        display += "-";
                    }
                    display += node.Data;
                    //display += node.DisplayOffset;
                    //displayOffsetSum += node.DisplayOffset;
                    prevDisplayOffset = node.DisplayOffset;

                    if (node.Index == (int)Math.Pow(2, node.Level) - 1)
                    {
                        Debug.Log(display);
                        prevDisplayOffset = 0;
                        display = "=>";
                    }

                }

                if (node.Left != null)
                    queue.Enqueue(node.Left);
                if (node.Right != null)
                    queue.Enqueue(node.Right);
            }

        }

        public void GetDisplayOffset(ref TreeNode node)
        {
            if (node != null)
            {
                GetDisplayOffsetRec(ref node);
                Debug.Log("Node displayoffset: " + node.DisplayOffset + "(" + node.Data + ")");
                CalculateDisplayOffset = false;
            }
        }

        private void GetDisplayOffsetRec(ref TreeNode root)
        {

            if (root.Level < TreeNode.MaxLevel)
            {
                Debug.Log(root.Data + " has level: " + root.Level);

                GetDisplayOffsetRec(ref root.Left);
                GetDisplayOffsetRec(ref root.Right);

                root.DisplayOffset= root.Left.DisplayOffset + (root.Right.DisplayOffset - root.Left.DisplayOffset) / 2;
            }
            else//we reached the leaf level
            {
                if (root.Index == 0)
                    LeafSiblingOffset = 0;
                

                //root.DisplayOffset = LeafSiblingOffset + root.Data.ToString().Length - 1;
                root.DisplayOffset = LeafSiblingOffset;

                //if (root.Data.ToString().Length % 2 == 0)
                    //root.DisplayOffset += 1;
                Debug.Log("leaf display offset: " + root.DisplayOffset);

                if ((root.Index % 2) == 0)
                {
                    Debug.Log("root index % 2 == 0 " + LeafSiblingOffset);
                    LeafSiblingOffset += root.Data.ToString().Length + 3;
                }
                else
                    LeafSiblingOffset += root.Data.ToString().Length + 5;                
                //return root.DisplayOffset;
            }
        }
    }
}
