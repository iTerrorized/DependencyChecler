🔄 Change the Creator Logo

    Replace the logo image file:

        Navigate to:
        Assets/!Terrorized/Dependency Checker/Resources/Icons/CreatorLogo.png

        Replace with your own logo with the same file name (or update the path in the script if renaming).

    If changing the filename, modify this line in the script:

logo.image = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/!Terrorized/Dependency Checker/Resources/Icons/CreatorLogo.png");

➕➖ Add or Remove Dependencies

In DependencyChecker.cs, edit the dependencies array:

private readonly string[][] dependencies = new string[][]
{
    new string[] { "VRChat SDK", "com.vrchat.avatars", "https://vrchat.com/home/download" },
    new string[] { "Poiyomi Shader", "com.poiyomi.toon", "https://github.com/poiyomi/PoiyomiToonShader" },
    new string[] { "VRCFury", "com.vrcfury.vrcfury", "https://vrcfury.com/" }
};

    To add, insert a new line with the format:

    new string[] { "Name", "Unity Package Name", "URL" }

    To remove, simply delete the corresponding line.

🔗 Add or Remove Socials

In the CreateUI() method, find the section with:

AddSidebarButton(sidebar, "Discord.png", "Discord", "https://discord.gg/...");

    Add a new social button like this:

    AddSidebarButton(sidebar, "Twitter.png", "Twitter", "https://twitter.com/yourname");

    Remove by deleting the line corresponding to the platform.

Make sure you place the icon in:

Assets/!Terrorized/Dependency Checker/Resources/Icons/

🧪 Change Avatar Scene Name

Find this line in CreateUI():

Button openSceneButton = new Button(() => OpenScene("Assets/Avatar.unity"))

Replace "Assets/Avatar.unity" with the new path, for example:

() => OpenScene("Assets/YourFolder/YourAvatarScene.unity")

Make sure the scene exists at that path in your project!
