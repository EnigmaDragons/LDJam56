using System;
using System.Diagnostics; // For running external processes
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    // EditorPrefs to store FMOD Executable path
    private const string FmodExecutablePathKey = "FMODExecutablePath";
    private const string DefaultFmodExecutablePath = @"C:\Program Files\FMOD SoundSystem\FMOD Studio 2.02.24\";
    private const string FmodExecutable = "fmodstudio.exe";
    private static string FmodExecutablePath
    {
        get => EditorPrefs.GetString(FmodExecutablePathKey, DefaultFmodExecutablePath);
        set => EditorPrefs.SetString(FmodExecutablePathKey, value);
    }

    // EditorPrefs to store FMOD Project path
    private const string FmodProjectPathKey = "FMODProjectPath";
    private const string DefaultFmodProjectPath = @"src\FMOD_LD56\FMOD_LD56.fspro";
    private static string FmodProjectPath
    {
        get => EditorPrefs.GetString(FmodProjectPathKey, DefaultFmodProjectPath);
        set => EditorPrefs.SetString(FmodProjectPathKey, value);
    }

    private static string ProjectPath
        => Directory.GetParent(Application.dataPath).FullName;

    // Traverse two levels up to get to the solution folder
    private static string SolutionPath
        => Directory.GetParent(Directory.GetParent(ProjectPath).FullName).FullName;

    private static string BuildPathWebGl
        => Path.Combine(SolutionPath, "Builds", "WebGL");

    private static string BuildPathWindows
        => Path.Combine(SolutionPath, "Builds", "Windows");

    private static string WindowsExecutableFilename
        => Path.GetFileNameWithoutExtension(ProjectPath) + ".exe";

    // Validation to disable the menu item if the editor is in play mode
    [MenuItem("File/Build All", true)]
    [MenuItem("File/Build FMOD", true)]
    [MenuItem("File/Build WebGL", true)]
    [MenuItem("File/Build Windows", true)]
    private static bool ValidateBuild()
        => !EditorApplication.isPlaying; // Disable during play mode

    [MenuItem("File/Set FMOD Tool Path")]
    public static void SetFmodExecutablePath()
    {
        // Prompt the user to select the FMOD install directory
        string path = EditorUtility.OpenFolderPanel(
            "Select FMOD Tool Directory",
            Directory.GetParent(FmodExecutablePath).FullName,
            Path.GetFileName(FmodExecutablePath));

        // Save the selected path to Unity EditorPrefs
        if (!string.IsNullOrEmpty(path))
        {
            FmodExecutablePath = path;
            UnityEngine.Debug.Log("FMOD install path set to: " + path);
        }
        else
        {
            UnityEngine.Debug.LogError("No FMOD install path selected.");
        }
    }

    [MenuItem("File/Build FMOD")]
    public static void BuildFmod()
        => BuildFmod(SolutionPath);

    [MenuItem("File/Build All")]
    public static void BuildAll()
    {
        BuildFmod(SolutionPath);
        BuildWebGL(BuildPathWebGl);
        BuildWindows(BuildPathWindows);
    }

    [MenuItem("File/Build WebGL")]
    public static void BuildWebGl()
        => BuildWebGL(BuildPathWebGl);

    [MenuItem("File/Build Windows")]
    public static void BuildWindows()
        => BuildWindows(BuildPathWindows);

    private static void BuildFmod(string solutionPath)
    {
        // Define paths for FMOD executable and project
        string fullFmodExecutable = Path.Combine(FmodExecutablePath, FmodExecutable);

        // Path to your FMOD project (relative to project root)
        string fullFmodProjectPath = Path.Combine(solutionPath, FmodProjectPath);

        UnityEngine.Debug.Log($"fullFmodExecutable: {fullFmodExecutable}");
        UnityEngine.Debug.Log($"fullFmodProjectPath: {fullFmodProjectPath}");

        // Ensure FMOD Studio executable exists
        if (!File.Exists(fullFmodExecutable))
        {
            UnityEngine.Debug.LogError("FMOD Studio executable not found at: " + fullFmodExecutable);
            return;
        }

        // Ensure FMOD project exists
        if (!File.Exists(fullFmodProjectPath))
        {
            UnityEngine.Debug.LogError("FMOD project not found at: " + fullFmodProjectPath);
            return;
        }

        // Create a process to run FMOD Studio command line build
        Process fmodProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fullFmodExecutable,
                Arguments = $"-build {fullFmodProjectPath}", // FMOD command-line build argument
                // https://fmod.com/docs/2.02/unreal/user-guide.html#commandlet
                // Arguments = $"{fullFmodProjectPath} -run=FMODGenerateAssets -rebuild",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        // Run FMOD build
        UnityEngine.Debug.Log("Starting FMOD build...");
        fmodProcess.Start();

        // Capture output and errors
        string output = fmodProcess.StandardOutput.ReadToEnd();
        string error = fmodProcess.StandardError.ReadToEnd();

        // Wait for FMOD process to exit
        bool exited = fmodProcess.WaitForExit(120000); // Wait for 2 minutes
        if (!exited)
        {
            UnityEngine.Debug.LogError("FMOD build timed out.");
            return;
        }
        var statusfmod = fmodProcess.ExitCode == 0 ? "successfully" : $"with errors: {error}";
        UnityEngine.Debug.Log($"FMOD build completed {statusfmod}");

        try
        {
            UnityEngine.Debug.Log("Refreshing FMOD banks...");
            FMODUnity.EventManager.RefreshBanks();
            UnityEngine.Debug.Log("FMOD banks refreshed successfully.");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"Failed to refresh FMOD banks: {ex.Message}");
        }

        // Always log FMOD build output
        UnityEngine.Debug.Log($"FMOD build output: {output}");
    }

    private static void BuildWebGL(string buildPathWebGl)
    {
        if (!Directory.Exists(buildPathWebGl)) Directory.CreateDirectory(buildPathWebGl);

        // WebGL Build
        BuildPlayerOptions buildPlayerOptionsWebGl = new BuildPlayerOptions
        {
            scenes = GetScenes(), // Include your scenes here
            locationPathName = BuildPathWebGl,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        // Log build start for WebGL
        UnityEngine.Debug.Log("Starting WebGL build...");
        var reportWebGl = BuildPipeline.BuildPlayer(buildPlayerOptionsWebGl);
        var statusWebGl = reportWebGl.summary.result == UnityEditor.Build.Reporting.BuildResult.Failed ? "failed" : "result";
        UnityEngine.Debug.Log($"WebGL build {statusWebGl}: {reportWebGl.summary.result}");
        UnityEngine.Debug.Log($"WebGL build path {buildPathWebGl}");
    }

    private static void BuildWindows(string buildPathWindows)
    {
        if (!Directory.Exists(buildPathWindows)) Directory.CreateDirectory(buildPathWindows);

        // Windows Build
        BuildPlayerOptions buildPlayerOptionsWindows = new BuildPlayerOptions
        {
            scenes = GetScenes(), // Include your scenes here
            locationPathName = Path.Combine(buildPathWindows, WindowsExecutableFilename),
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        // Log build start for Windows
        UnityEngine.Debug.Log("Starting Windows build...");
        var reportWindows = BuildPipeline.BuildPlayer(buildPlayerOptionsWindows);
        var statusWindows = reportWindows.summary.result == UnityEditor.Build.Reporting.BuildResult.Failed ? "failed" : "result";
        UnityEngine.Debug.Log($"Windows build {statusWindows}: {reportWindows.summary.result}");
        UnityEngine.Debug.Log($"Windows build path {buildPathWindows}");
    }

    // Dynamically get all scene files from the Build Settings
    private static string[] GetScenes()
        => EditorBuildSettings.scenes
            .Where(scene => scene.enabled) // Only include enabled scenes
            .Select(scene => scene.path)   // Get the path for each scene
            .ToArray();                    // Convert to an array
}
