using System;
using System.Threading;
using Mono.Cecil.Cil;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vault.DataStrucures;

public class VisualElementWrapper : IComparable<VisualElementWrapper>
{
    private VisualElement _visualElement;
    public VisualElement VisualElement => _visualElement;
    public VisualElementWrapper(VisualElement visualElement)
    {
        _visualElement = visualElement;
    }

    //Comparison
    public int CompareTo(VisualElementWrapper other)
    {
        if(Convert.ToInt32(_visualElement.name) < Convert.ToInt32(other._visualElement.name))
            return -1;
    
        if(Convert.ToInt32(_visualElement.name) > Convert.ToInt32(other._visualElement.name))
            return 1;
        
        return 0;
    }

    int GetDepth()
    {
        int depth = 0;
        VisualElement ve = _visualElement;
        while (ve.parent != null)
        {
            depth++;
            ve = ve.parent;
        }
        return depth;
    }
}

public class TreEditorWindow : EditorWindow
{
    private const string UxmlPath = "Assets/_Project/Editor//TreeEditor.uxml";
    private const string UssPath  = "Assets/_Project/Editor//TreeEditor.uss";
    private static Texture2D treeNodeIcon;
    private BinarySearchTree<int> tree = new BinarySearchTree<int>();
    private BinarySearchTree<Vec2> displayTree;

    private BinarySearchTree<VisualElementWrapper> _testTree = new BinarySearchTree<VisualElementWrapper>();

    private float _hierarchyYOffset = 0;

    [MenuItem("Tools/Tree Editor")]
    public static void ShowWindow()
    {
        TreEditorWindow wnd = GetWindow<TreEditorWindow>();
        wnd.titleContent = new GUIContent("Tree Editor Window");
        wnd.StartUpdateLoop();
    }

    public void CreateGUI()
    {
        //Create Test Tree
        tree.Insert(7);
        tree.Insert(5);
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

        tree.PreorderTraversal(Insert, NewVisualSubTree);

        ReparentHierarchyNodes(_testTree.Root);

    }

    public VisualElementWrapper NewVisualSubTree(BinaryTreeNode<int> node)
    {
        VisualElement subTree = new VisualElement();
        subTree.name = node.Data.ToString();//for comparison
        subTree.AddToClassList("tree-hierarchy-container");

        var label = new Label
        {
            text = node.Data.ToString(),
            
        };

        label.style.position = Position.Absolute;
        label.style.left = node.Level*20;
        _hierarchyYOffset+=25;
        label.style.top = _hierarchyYOffset;

        subTree.Add(label);

        var lineHeight = 0;
        //Draw vertical lines

        lineHeight = tree.CountNodes(node)*25-25;
        if(node.Right != null)
            lineHeight -= (tree.CountNodes(node.Right)-1)*25;
        else if(node.Left != null)
            lineHeight -= (tree.CountNodes(node.Left)-1)*25;

        var line = new VisualElement();
        line.style.position = Position.Absolute;
        line.style.height = lineHeight;
        line.style.backgroundColor = Color.black;
        line.style.top = _hierarchyYOffset;
        line.style.left = node.Level*20;
        line.style.width = 2;

        subTree.Add(line);

        if(node.Parent != null)
        {
            var line2 = new VisualElement();
            line2.style.position = Position.Absolute;
            line2.style.height = 2;
            line2.style.backgroundColor = Color.black;
            line2.style.top = _hierarchyYOffset;
            line2.style.left = node.Parent.Level*20;
            line2.style.width = 20;

            subTree.Add(line2);
        }

        VisualElementWrapper wrapper = new VisualElementWrapper(subTree);

        return wrapper;
    }

    public void ReparentHierarchyNodes(BinaryTreeNode<VisualElementWrapper> node)
    {
        ReparentHierarchyNodesRec(node);
        rootVisualElement.Add(node.Data.VisualElement);
    }

    public void ReparentHierarchyNodesRec(BinaryTreeNode<VisualElementWrapper> node)
    {
        if(node.Left != null)
        {
            node.Data.VisualElement.Add(node.Left.Data.VisualElement);
            ReparentHierarchyNodes(node.Left);
        }
        if(node.Right != null)
        {
            node.Data.VisualElement.Add(node.Right.Data.VisualElement);
            ReparentHierarchyNodes(node.Right);
        }
    }

    public void CreateTreeHierarchyGUI()
    {
        CreateTreeHierarchyGUIRec(tree.Root, "");
    }

    private void CreateTreeHierarchyGUIRec(BinaryTreeNode<int> node, string indent)
    {
        var treeLevel = new VisualElement();
        treeLevel.name ="tree-level";
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
            img.style.width = 64;
            img.style.height = 64;
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

    public void Insert(VisualElementWrapper node)
    {
        _testTree.Insert(node);
    }

    void StartUpdateLoop()
    {
        rootVisualElement.schedule
            .Execute(UpdateUI)
            .Every(100); // milliseconds
    }


    void UpdateUI()
    {
       
    }

}