using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(BinaryTreeNodeComponent))]
public class BinaryTreeNodeComponentEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // Root
        var root = new VisualElement();

        // Load UXML
        var visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/_Project/Editor/BinaryTreeNode.uxml");

        visualTree.CloneTree(root);

         // Load USS
        var styleSheet =
            AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/_Project/Editor/BinaryTreeNode.uss");

        // Bind serialized object
        root.Bind(serializedObject);

        return root;
    }
}