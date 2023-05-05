// Version Incrementor Script for Unity by Francesco Forno (Fornetto Games)
// Inspired by http://forum.unity3d.com/threads/automatic-version-increment-script.144917/

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[InitializeOnLoad]
public class VersionIncrementor
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
        Debug.Log("Build v" + PlayerSettings.bundleVersion + " (" + PlayerSettings.Android.bundleVersionCode + ")");
        IncreaseBuild();
    }

    static void IncrementVersion(int majorIncr, int minorIncr, int buildIncr) {
        string[] lines = PlayerSettings.bundleVersion.Split('.');

        int MajorVersion = int.Parse(lines[0]) + majorIncr;
        int MinorVersion = int.Parse(lines[1]) + minorIncr;
        int Build = int.Parse(lines[2]) + buildIncr;

        PlayerSettings.bundleVersion = MajorVersion.ToString("0") + "." +
                                        MinorVersion.ToString("0") + "." +
                                        Build.ToString("0");
        PlayerSettings.Android.bundleVersionCode = MajorVersion * 10000 + MinorVersion * 1000 + Build;
    }

    [MenuItem("Build/Increase Minor Version")]
    private static void IncreaseMinor() {
        IncrementVersion(0, 1, 0);
    }

    [MenuItem("Build/Increase Major Version")]
    private static void IncreaseMajor() {
        IncrementVersion(1, 0, 0);
    }

    private static void IncreaseBuild() {
        IncrementVersion(0, 0, 1);
    }
}