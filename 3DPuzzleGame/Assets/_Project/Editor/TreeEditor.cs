using System;
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


    [MenuItem("Tools/Tree Editor")]
    public static void ShowWindow()
    {
        TreEditorWindow wnd = GetWindow<TreEditorWindow>();
        wnd.titleContent = new GUIContent("Tree Editor Window");

    }

    public void CreateGUI()
    {
        //Create Test Tree
        tree.Insert(0);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(4);
        tree.Insert(5);

        tree.LevelOrderTraversal(null);
        
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
        for(int i = 0; i <= tree.Levels; i++)
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

        for(int i = 0; i < (int)Math.Pow(2,level); i++)
        {
            Debug.Log("I is: " + i);
            var img = new Image
            {
                image = treeNodeIcon,
                scaleMode = ScaleMode.ScaleToFit
            };
            treeLevel.Add(img);
        }
        return treeLevel;
    }
}