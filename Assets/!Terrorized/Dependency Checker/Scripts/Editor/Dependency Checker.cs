//  This script has been made by iterrorized & vi._.gnette on discord along with help from chatgpt

//  This script is NOT for re-use and is copyrighted

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.SceneManagement;

[InitializeOnLoad] // <-- Ensures this class initializes on Unity startup
public class DependencyChecker : EditorWindow
{
    private const string REQUIRED_UNITY_VERSION = "2022.3.22f1";

    private VisualElement root;
    private Label statusLabel;

    private readonly string[][] dependencies = new string[][]
    {
        new string[] { "VRChat SDK", "com.vrchat.avatars", "https://vrchat.com/home/download" },
        new string[] { "Poiyomi Shader", "com.poiyomi.toon", "https://github.com/poiyomi/PoiyomiToonShader" },
        new string[] { "VRCFury", "com.vrcfury.vrcfury", "https://vrcfury.com/" }
    };

    private static Vector2 windowSize = new Vector2(700, 500); // Window size

    // ✅ Automatically open on Unity project startup
    static DependencyChecker()
    {
        EditorApplication.update += OpenOnStartup;
    }

    private static void OpenOnStartup()
    {
        EditorApplication.update -= OpenOnStartup; // Ensures it only runs once per startup
        ShowWindow();
    }

    [MenuItem("Terrorized/Dependency Checker")]
    public static void ShowWindow()
    {
    // Check if window already exists
    DependencyChecker window = GetWindow<DependencyChecker>(true, "Dependency Checker", true);

    // Set position and size only if the window is newly created
    window.position = new Rect(100, 100, windowSize.x, windowSize.y);
    window.ShowUtility(); // pop-out persistent window
    window.Focus();       // brings window to front if already open
    }


    private void OnEnable()
    {
        CreateUI();
        CheckDependencies();
    }

    private void OnGUI()
    {
        // Prevent resizing
        this.minSize = windowSize;
        this.maxSize = windowSize;
    }

    private void OpenScene(string scenePath)
{
    if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath);
    }
}

    private void CreateUI()
    {
        root = rootVisualElement;
        root.Clear();

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/!Terrorized/Dependency Checker/Scripts/DependencyChecker.uss");
        if (styleSheet != null) root.styleSheets.Add(styleSheet);

        // ✅ Main Layout Container
        VisualElement container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;
        container.style.flexGrow = 1;

        // ✅ Sidebar
        VisualElement sidebar = new VisualElement();
        sidebar.style.width = 180;
        sidebar.style.minWidth = 180;
        sidebar.style.flexShrink = 0;
        sidebar.AddToClassList("sidebar");

        // ✅ Move Creator Logo to Top Left
        Image logo = new Image();
        logo.image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/!Terrorized/Dependency Checker/Resources/Icons/CreatorLogo.png");
        logo.AddToClassList("sidebar-logo");
        logo.style.width = 150;
        logo.style.height = 150;
        logo.style.alignSelf = Align.Center;
        logo.style.marginBottom = 20;
        sidebar.Add(logo);


        // ✅ Add Social Buttons (Icons on the Left, Spaced Out)
        AddSidebarButton(sidebar, "Discord.png", "Discord", "https://discord.gg/p9ecE3mTXx");
        AddSidebarButton(sidebar, "Payhip.png", "Payhip", "https://payhip.com/Levitella");
        AddSidebarButton(sidebar, "Instagram.png", "Instagram", "https://www.instagram.com/__.levitella.__");
        AddSidebarButton(sidebar, "Tiktok.png", "Tiktok", "https://www.tiktok.com/@levitella");


        VisualElement spacer = new VisualElement();
        spacer.style.flexGrow = 1;
        sidebar.Add(spacer);


        // ✅ Button to open a scene
        Button openSceneButton = new Button(() => OpenScene("Assets/Avatar.unity"))
        {
            text = "Open Avatar"
        };
        openSceneButton.style.marginBottom = 10;
        openSceneButton.style.width = 160;
        openSceneButton.style.alignSelf = Align.FlexStart;
        sidebar.Add(openSceneButton);


        Label sidebarTitle = new Label("Script by Terrorized");
        sidebarTitle.AddToClassList("sidebar-Credit");
        sidebarTitle.style.color = new Color(0.65f, 0.65f, 0.65f, 0.65f); // Darker Red
        sidebarTitle.style.paddingBottom = 8;
        sidebar.Add(sidebarTitle);
        sidebar.style.paddingLeft = 12;
        

        container.Add(sidebar);

        // ✅ Main Panel
        VisualElement mainPanel = new VisualElement();
        mainPanel.style.flexGrow = 1;
        mainPanel.style.backgroundColor = new Color(0f, 0f, 0f, 0.1f); // Darker Red
        mainPanel.style.borderTopLeftRadius = 15;   // Rounds top-left corner
        mainPanel.style.borderTopRightRadius = 15;  // Rounds top-right corner
        mainPanel.style.borderBottomLeftRadius = 15; // Rounds bottom-left corner
        mainPanel.style.borderBottomRightRadius = 15; // Rounds bottom-right corner
        mainPanel.style.paddingLeft = 12;
        mainPanel.style.paddingRight = 12;
        mainPanel.style.paddingTop = 15;
        mainPanel.style.paddingBottom = 15;
        mainPanel.style.marginBottom = 10;
        mainPanel.style.marginLeft = 10;
        mainPanel.style.marginRight = 10;
        mainPanel.style.marginTop = 10;
        mainPanel.AddToClassList("main-panel");


        Label title = new Label("Dependencies");
        title.AddToClassList("title");
        mainPanel.Add(title);
        title.style.fontSize = 16;
        title.style.paddingBottom = 5;

        statusLabel = new Label();
        statusLabel.AddToClassList("status-label");
        mainPanel.Add(statusLabel);

        VisualElement separator = new VisualElement();
        separator.style.height = 2;
        separator.style.marginTop = 10;
        separator.style.marginBottom = 10;
        separator.style.backgroundColor = new Color(1f, 1f, 1f, 0.2f); // subtle separator color

        mainPanel.Add(separator);


        foreach (var dependency in dependencies)
        {
            mainPanel.Add(CreateDependencyRow(dependency[0], dependency[1], dependency[2]));
        }

        container.Add(mainPanel);
        root.Add(container);
    }

    private void CheckDependencies()
    {
        bool missingDependencies = false;

        foreach (var dependency in dependencies)
        {
            if (!IsDependencyInstalled(dependency[1]))
            {
                missingDependencies = true;
                break;
            }
        }

        statusLabel.text = missingDependencies ? "⚠ Some dependencies are missing!" : "✅ All dependencies are installed!";
        statusLabel.style.color = missingDependencies ? Color.yellow : Color.green;
    }

    private bool IsDependencyInstalled(string packageName)
{
    var request = UnityEditor.PackageManager.Client.List(true);
    while (!request.IsCompleted) { } // Wait for the request to complete

    if (request.Status == UnityEditor.PackageManager.StatusCode.Failure)
    {
        Debug.LogError("Failed to retrieve package list.");
        return false;
    }

    foreach (var package in request.Result)
    {
        if (package.name == packageName)
            return true;
    }
    return false;

    
}

private string GetDependencyVersion(string packageName)
{
    var request = UnityEditor.PackageManager.Client.List(true);
    while (!request.IsCompleted) { } // Wait for the request to complete

    if (request.Status == UnityEditor.PackageManager.StatusCode.Failure)
    {
        Debug.LogError("Failed to retrieve package list.");
        return "Unknown";
    }

    foreach (var package in request.Result)
    {
        if (package.name == packageName)
            return package.version;
    }
    return "Not Installed";
}



    /// ✅ **FIX: Added the missing `AddSidebarButton` method**
    private void AddSidebarButton(VisualElement sidebar, string iconFileName, string buttonText, string url)
    {
        VisualElement buttonRow = new VisualElement();
        buttonRow.style.flexDirection = FlexDirection.Row;
        buttonRow.style.alignItems = Align.Center;
        buttonRow.AddToClassList("sidebar-button");
        buttonRow.style.marginBottom = 15;

        Image icon = new Image();
        icon.image = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/!Terrorized/Dependency Checker/Resources/Icons/{iconFileName}");
        icon.style.width = 24;
        icon.style.height = 24;
        icon.style.marginRight = 10;

        Button button = new Button(() => Application.OpenURL(url)) { text = buttonText };
        button.style.flexGrow = 1;
        button.AddToClassList("sidebar-button-label");

        buttonRow.Add(icon);
        buttonRow.Add(button);
        sidebar.Add(buttonRow);
    }

    private VisualElement CreateDependencyRow(string name, string searchKey, string url)
{
    VisualElement row = new VisualElement();
    row.AddToClassList("dependency-row");

    VisualElement rowContainer = new VisualElement();
    rowContainer.style.flexDirection = FlexDirection.Column;
    rowContainer.style.alignItems = Align.FlexStart;
    rowContainer.style.backgroundColor = new Color(0f, 0f, 0f, 0.1f);
    rowContainer.style.borderTopLeftRadius = 15;
    rowContainer.style.borderTopRightRadius = 15;
    rowContainer.style.borderBottomLeftRadius = 15;
    rowContainer.style.borderBottomRightRadius = 15;
    rowContainer.style.paddingLeft = 12;
    rowContainer.style.paddingRight = 15;
    rowContainer.style.paddingTop = 15;
    rowContainer.style.paddingBottom = 15;
    rowContainer.style.marginBottom = 10;
    rowContainer.style.marginLeft = 10;
    rowContainer.style.marginRight = 10;
    rowContainer.style.marginTop = 10;
    rowContainer.AddToClassList("status-row");

    VisualElement topRow = new VisualElement();
    topRow.style.flexDirection = FlexDirection.Row;
    topRow.style.alignItems = Align.Center;
    topRow.style.justifyContent = Justify.SpaceBetween;
    topRow.style.width = Length.Percent(100);

    Image statusIcon = new Image();
    bool isInstalled = IsDependencyInstalled(searchKey);
    statusIcon.image = AssetDatabase.LoadAssetAtPath<Texture2D>(
        isInstalled
            ? "Assets/!Terrorized/Dependency Checker/Resources/Icons/Good.png"
            : "Assets/!Terrorized/Dependency Checker/Resources/Icons/Error.png"
    );
    statusIcon.AddToClassList("status-icon");
    statusIcon.style.width = 50;
    statusIcon.style.height = 50;
    statusIcon.style.marginRight = 5;

    VisualElement nameVersionContainer = new VisualElement();
    nameVersionContainer.style.flexDirection = FlexDirection.Column;
    nameVersionContainer.style.alignItems = Align.FlexStart;
    nameVersionContainer.style.flexGrow = 1;

    Label nameLabel = new Label(name);
    nameLabel.AddToClassList("dependency-name");

    Label versionLabel = new Label($"Version: {GetDependencyVersion(searchKey)}");
    versionLabel.style.fontSize = 11;
    versionLabel.style.unityFontStyleAndWeight = FontStyle.Normal;
    versionLabel.style.color = Color.gray;
    versionLabel.style.marginTop = 4;

    nameVersionContainer.Add(nameLabel);
    nameVersionContainer.Add(versionLabel);

    topRow.Add(statusIcon);
    topRow.Add(nameVersionContainer);

    // Only add the "Auto Fix" button if the dependency is NOT installed
    if (!isInstalled)
    {
        Button fixButton = new Button(() => Application.OpenURL(url)) { text = "Open Link" };
        fixButton.AddToClassList("fix-button");
        fixButton.style.width = 70;
        fixButton.style.height = 45;
        fixButton.style.unityFontStyleAndWeight = FontStyle.Bold;
        fixButton.style.marginLeft = 20;

        topRow.Add(fixButton);
    }

    rowContainer.Add(topRow);
    row.Add(rowContainer);
    return row;
}

}