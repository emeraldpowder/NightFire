using System.IO;
using UnityEditor;
using UnityEngine;

public class TakeScreenshot : EditorWindow
{
    [MenuItem("Game/Take Screenshot")]
    private static void Take()
    {
        var path = "Screenshots";

        Directory.CreateDirectory(path);

        int i = 0;
        while (File.Exists(path + "/" + i + ".png"))
        {
            i++;
        }

        ScreenCapture.CaptureScreenshot(path + "/" + i + ".png");
    }
}
