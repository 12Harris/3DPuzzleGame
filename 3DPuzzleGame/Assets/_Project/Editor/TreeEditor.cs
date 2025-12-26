using System;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vault.DataStrucures;

public class TreEditorWindow : EditorWindow
{
    private const string UxmlPath = "Assets/_Project/Editor//TreeEditor.uxml";
    private const string UssPath  = "Assets/_Project/Editor//TreeEditor.uss";
    private static Texture2D treeNodeIcon;
    private BinarySearchTree<int> tree = new BinarySearchTree<int>();
    private BinarySearchTree<Vec2> displayTree;

    [MenuItem("Tools/Tree Editor")]
    public static void ShowWindow()
    {
        TreEditorWindow wnd = GetWindow<TreEditorWindow>();
        wnd.titleContent = new GUIContent("Tree Editor Window");
    }

    public void CreateGUI()
    {
        //Create Test Tree
        tree.Insert(7);
        tree.Insert(4);
        tree.Insert(10);
        tree.Insert(9);
        tree.Insert(11);
        tree.Insert(3);
        tree.Insert(8);
        tree.Insert(13);

        /*tree.Insert(5);
        tree.Insert(4);
        tree.Insert(8);
        tree.Insert(3);
        tree.Insert(6);
        tree.Insert(9);
        tree.Insert(7);*/

        /*tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(4);
        tree.Insert(5);*/

        tree.LevelOrderTraversal(null);
        
        tree.PrintTree();

        //Assign leaf indices to leaf nodes
        var leafNodes = tree.GetNodesAtLevel(tree.Root, tree.Levels);
        int leafIndex = 0;
        Debug.Log("leaf nodes count: " + leafNodes.Count);
        foreach(var leafNode in leafNodes)
        {
            Debug.Log("leaf node: " + leafNode.Data);
            leafNode.LeafIndex = leafIndex;
            leafIndex++;
        }

        displayTree = tree.GenerateNodeDisplayTree();
        displayTree.LevelOrderTraversal(null);  
        displayTree.PrintTree();
        Debug.Log("display tree count: " + displayTree.CountNodes() + ", levels: " + displayTree.Levels);

        // Load UXML
        VisualTreeAsset visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
        visualTree.CloneTree(rootVisualElement);

        // Load USS
        StyleSheet styleSheet =
            AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath);
        rootVisualElement.styleSheets.Add(styleSheet);

        // Query elements
        Button myButton = rootVisualElement.Q<Button>("my-button");
        Label statusLabel = rootVisualElement.Q<Label>("status-label");

        //Toolbar
        var toolbar = new Toolbar();
        toolbar.Add(new ToolbarButton(() => Debug.Log("Toolbar Action")) { text = "Run" });
        rootVisualElement.Add(toolbar);

        //treenode icon
        treeNodeIcon = AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets/_Project/Editor/Icons/treenode.png");
        /*var button = rootVisualElement.Q<Button>("treeNode");
        /.style.backgroundImage = new StyleBackground(treeNodeIcon);*/

        // Bind logic
        myButton.clicked += () =>
        {
            statusLabel.text = "Button clicked!";
            Debug.Log("Button clicked");
        };

        CreateTreeUI();
    }

    public void CreateTreeUI()
    {
        var container = rootVisualElement.Q<VisualElement>("tree-container");
        for(int i = 0; i <= displayTree.Levels; i++)
        {
            var treeLevel = CreateTreeLevel(i);
            container.Add(treeLevel);
        }
        rootVisualElement.Add(container);
    }

    public VisualElement CreateTreeLevel(int level)
    {
        var treeLevel = new VisualElement();
        treeLevel.name ="tree-level";
        treeLevel.AddToClassList("tree-level");

        var nodes =  displayTree.GetNodesAtLevel(displayTree.Root, level);
        var nodesInfo = tree.GetNodesAtLevel(tree.Root, level);

        for(int i = 0; i < nodes.Count; i++)
        {
            var img = new Image
            {
                image = treeNodeIcon,

                scaleMode = ScaleMode.ScaleToFit
            };

            img.style.position = Position.Absolute;
            img.style.left = nodes[i].Data.X;
            img.style.top = nodes[i].Data.Y;
            treeLevel.Add(img);

            //add label with node info
            var label = new Label(nodesInfo[i].Data.ToString());
            label.style.fontSize = 20;
            label.style.color = Color.black;
            label.style.position = Position.Absolute;
            label.style.left = nodes[i].Data.X;
            label.style.top = nodes[i].Data.Y;
            
            treeLevel.Add(label);
        }

        return treeLevel;
    }
}