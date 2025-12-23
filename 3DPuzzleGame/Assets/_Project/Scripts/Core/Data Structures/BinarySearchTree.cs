using UnityEngine;
using System;

namespace Vault.DataStrucures
{
    public class BinarySearchTree<T> : BinaryTree<T> where T: IComparable<T>
    {
        public BinarySearchTree() : base()
        {
          
        }

        public override void Insert(T data)
        {
            Debug.Log("insert in bst");
            Root = InsertRec(Root, data);
        }

        private TreeNode InsertRec(TreeNode root, T data)
        {
            if (root == null)
            {
                root = new TreeNode(data);
                root.Level = CurrentLevel;
                Debug.Log("TREENODE LEVEL: " + root.Level);

                //Update tree level
                Count++;

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
                root.Index = Count - (int)Math.Pow(2, root.Level) + 1;

                return root;
            }

            if (data.CompareTo(root.Data) < 0)
                root.Left = InsertRec(root.Left, data);
            else if (data.CompareTo(root.Data) > 0)
                root.Right = InsertRec(root.Right, data);

            return root;
        }

        public bool Search(T data)
        {
            return SearchRec(Root, data);
        }

        private bool SearchRec(TreeNode root, T data)
        {
            if (root == null)
                return false;

            if (data.CompareTo(root.Data) == 0)
                return true;

            if (data.CompareTo(root.Data) < 0)
                return SearchRec(root.Left, data);

            return SearchRec(root.Right, data);

        }
    }
}