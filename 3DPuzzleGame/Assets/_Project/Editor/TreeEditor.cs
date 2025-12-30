using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Mono.Cecil.Cil;
using NUnit.Framework.Constraints;
using PlasticGui.WorkspaceWindow;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Vault.DataStrucures;


public sealed class GraphNodeWrapper : VisualElementWrapper
{
    public int Level{get;set;} = 0;
    public int LevelIndex{get;set;} = 0;
    public GraphNodeWrapper(VisualElement visualElement) : base(visualElement)
    {
        
    }

    public override int CompareTo(VisualElementWrapper other)
    {
        return -1;
    }

}

public sealed class HierarchyWrapper : VisualElementWrapper
{
    public HierarchyWrapper(VisualElement visualElement) : base(visualElement){}

    public override  void ToggleEnable(bool isLeaf = false)
    {
        base.ToggleEnable(isLeaf);

        if(!isLeaf)
        {
            var labelText = VisualElement.Q<Label>("vsubtree-label").text;

             if(!Enabled)
                labelText += " ...";
        
            else
                labelText = labelText.Remove(labelText.Length-4,4);
            VisualElement.Q<Label>("vsubtree-label").text = labelText;
        }
    }

    public override void Enable(bool enable)
    {

        base.Enable(enable);

        VisualElement.Q<VisualElement>("vsubtree-label").style.display = DisplayStyle.Flex;
        VisualElement.Q<VisualElement>("vertical-line").style.display = Enabled ? DisplayStyle.Flex : DisplayStyle.None;

        if(VisualElement.Q<VisualElement>("parent-connection") !=  null)
            VisualElement.Q<VisualElement>("parent-connection").style.display = DisplayStyle.Flex;

    }

    public override void Hide()
    {
        VisualElement.Q<VisualElement>("vsubtree-label").style.display = DisplayStyle.None;
        VisualElement.Q<VisualElement>("vertical-line").style.display = DisplayStyle.None;
        VisualElement.Q<VisualElement>("parent-connection").style.display = DisplayStyle.None;

    }

    
    public override int CompareTo(VisualElementWrapper other)
    {
        if(other is not HierarchyWrapper)
            return -2;

        if(Convert.ToInt32(VisualElement.name) < Convert.ToInt32(other.VisualElement.name))
            return -1;
    
        if(Convert.ToInt32(VisualElement.name) > Convert.ToInt32(other.VisualElement.name))
            return 1;
        
        return 0;
    }
}

public abstract class VisualElementWrapper : IComparable<VisualElementWrapper>
{
    private VisualElement _visualElement;
    public VisualElement VisualElement => _visualElement;

    private bool _enable = true;

    public bool Enabled => _enable;

    public VisualElementWrapper(VisualElement visualElement)
    {
        _visualElement = visualElement;
    }


    public virtual void ToggleEnable(bool isLeaf = false)
    {
        Enable(!_enable);
    }

    public virtual void Enable(bool enable)
    {
        _enable = enable;
    }

    public virtual void Hide(){}

    //Comparison
    public abstract int CompareTo(VisualElementWrapper other);


    public int GetDepth()
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

    private BinarySearchTree<VisualElementWrapper> _hierachy = new BinarySearchTree<VisualElementWrapper>();
    
    private BinaryTreeNode<VisualElementWrapper> _selectedNode = null;

    private List<GraphNodeWrapper> _graphNodes = new List<GraphNodeWrapper>();

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
        tree.Insert(12);

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

        ReparentHierarchyNodes(_hierachy.Root);

        RegisterClickEvents(_hierachy.Root);

    }

    public void RegisterClickEvents(BinaryTreeNode<VisualElementWrapper> node)
    {
        
        node.Data.VisualElement.Q<VisualElement>("vsubtree-label").RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Mouse clicked " + node.Data.VisualElement);
            OnClickVisualSubTree(node);
        });
        if(node.Left != null)
            RegisterClickEvents(node.Left);
        if(node.Right != null)
            RegisterClickEvents(node.Right);
    }

    private void OnClickVisualSubTree(BinaryTreeNode<VisualElementWrapper> node)
    {   
        _selectedNode = node;
        node.Data.ToggleEnable(node.IsLeaf);
        EnableDisableVisualSubTrees(_hierachy.Root);
    }

    private void EnableDisableVisualSubTrees(BinaryTreeNode<VisualElementWrapper> node)
    { 
        
        EnableDisableVisualSubTreesRec(node);  

    }

    private void EnableDisableVisualSubTreesRec( BinaryTreeNode<VisualElementWrapper> node)
    {
        if(node == null)
            return;

        if(node.Data.Enabled)
        {
            if(node.Left != null)
            {
                node.Left.Data.Enable(node.Left.Data.Enabled);
                EnableDisableVisualSubTreesRec( node.Left);
                
                
            }
            if(node.Right != null)
            {
                node.Right.Data.Enable(node.Right.Data.Enabled);
                EnableDisableVisualSubTreesRec( node.Right);
            }
        }
        else
        {
            Debug.Log("hiding subtrees!");
            HideVisualSubTreeRec(node.Left);
            HideVisualSubTreeRec(node.Right);
        }
    }

    public void HideVisualSubTreeRec( BinaryTreeNode<VisualElementWrapper> node)
    {
        if(node != null)
        {
            node.Data.Hide();
            HideVisualSubTreeRec(node.Left);
            HideVisualSubTreeRec(node.Right);
        }
    }

    public HierarchyWrapper NewVisualSubTree(BinaryTreeNode<int> node)
    {
        VisualElement subTree = new VisualElement();
        subTree.name = node.Data.ToString();//for comparison
        subTree.AddToClassList("tree-hierarchy-container");

        var label = new Label
        {
            name = "vsubtree-label",
            text = node.Data.ToString(),
            
        };

        label.style.position = Position.Absolute;
        label.style.left = node.Level*20+5;
        _hierarchyYOffset+=25;
        label.style.top = _hierarchyYOffset-5;

        subTree.Add(label);

        var lineHeight = 0;
        //Draw vertical lines

        lineHeight = tree.CountNodes(node)*25-25;
        if(node.Right != null)
            lineHeight -= (tree.CountNodes(node.Right)-1)*25;
        else if(node.Left != null)
            lineHeight -= (tree.CountNodes(node.Left)-1)*25;

        var line = new VisualElement();
        line.name = "vertical-line";
        line.style.position = Position.Absolute;
        line.style.height = lineHeight;
        line.style.backgroundColor = Color.black;
        line.style.top = _hierarchyYOffset;
        line.style.left = node.Level*20;
        line.style.width = 2;

        subTree.Add(line);

        if(node.Parent != null)//draw connection to parent node(each child node has single connection to parent node)
        {
            var line2 = new VisualElement();
            line2.name = "parent-connection";
            line2.style.position = Position.Absolute;
            line2.style.height = 2;
            line2.style.backgroundColor = Color.black;
            line2.style.top = _hierarchyYOffset;
            line2.style.left = node.Parent.Level*20;
            line2.style.width = 20;

            subTree.Add(line2);
        }

        //VisualElementWrapper wrapper = new VisualElementWrapper(subTree);
        HierarchyWrapper wrapper = new HierarchyWrapper(subTree);

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

            if(nodes[i].Left != null)
                treeLevel.Add(DrawConnection(nodes[i], nodes[i].Left));

            if(nodes[i].Right!= null)
                treeLevel.Add(DrawConnection(nodes[i], nodes[i].Right));
            
            treeLevel.Add(label);
            //Wrap this visual element to be able reference it later
            _graphNodes.Add(new GraphNodeWrapper(img));
            _graphNodes[_graphNodes.Count-1].Level = nodes[i].Level;
            _graphNodes[_graphNodes.Count-1].LevelIndex = nodes[i].LevelIndex;
            img.RegisterCallback<ContextClickEvent>(_ =>
            {
                Debug.Log("Mouse right clicked " +  _graphNodes[_graphNodes.Count-1].VisualElement);
                _graphNodes[_graphNodes.Count-1].ToggleEnable();
            });
        }
        
        return treeLevel;
    }
    
    //Draws vertical or horizontal lines
    private VisualElement DrawLine(Vec2 startPos, Vec2 endPos)
    {
        VisualElement line = new VisualElement();
        var lineSize = endPos-startPos;
        Debug.Log("LINESIZE.Y: " + lineSize.Y);
        Debug.Log("LINESIZE.X: " + lineSize.X);
        line.style.position = Position.Absolute;
        line.style.height = lineSize.Y <= 0 ? 5 : lineSize.Y;
        line.style.width = lineSize.X <= 0 ? 5 : lineSize.X;
        line.style.backgroundColor = Color.black;
        line.style.top = startPos.Y;
        line.style.left = startPos.X;
        return line;
    }

    public VisualElement DrawConnection(BinaryTreeNode<Vec2> node, BinaryTreeNode<Vec2> childnode)
    {
        VisualElement linesContainer = new VisualElement();
        linesContainer.Add(DrawLine(node.Data+new Vec2(32,64), node.Data+new Vec2(32,0) + new Vec2(0,90)));
        
        var yOffset = childnode.Data.Y - node.Data.Y;
        var v1 = node.Data+new Vec2(32,0) + new Vec2(0,90);
        var v2 = new Vec2(childnode.Data.X+32,v1.Y);

        if(childnode == node.Right)
            linesContainer.Add(DrawLine(v1, v2));
        if(childnode == node.Left)
            linesContainer.Add(DrawLine(v2, v1 + new Vec2(5,0)));

        linesContainer.Add(DrawLine(v2, childnode.Data + new Vec2(32,0)));

        return linesContainer;
    }

    public void Insert(VisualElementWrapper node)
    {
        _hierachy.Insert(node);
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