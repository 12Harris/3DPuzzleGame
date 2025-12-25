using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TreEditorWindow : EditorWindow
{
    private const string UxmlPath = "Assets/_Project/Editor//TreeEditor.uxml";
    private const string UssPath  = "Assets/_Project/Editor//TreeEditor.uss";
    private static Texture2D treeNodeIcon;

    [MenuItem("Tools/Tree Editor")]
    public static void ShowWindow()
    {
        TreEditorWindow wnd = GetWindow<TreEditorWindow>();
        wnd.titleContent = new GUIContent("Tree Editor Window");

    }

    public void CreateGUI()
    {
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

        //treenode
        treeNodeIcon = AssetDatabase.LoadAssetAtPath<Texture2D> ("Assets/_Project/Editor/Icons/treenode.png");
        var button = rootVisualElement.Q<Button>("treeNode");
        if(button == null)
            Debug.Log("Button is null!");

        //if(treeNodeIcon != null)
        button.style.backgroundImage = new StyleBackground(treeNodeIcon);

        // Bind logic
        myButton.clicked += () =>
        {
            statusLabel.text = "Button clicked!";
            Debug.Log("Button clicked");
        };
    }
}