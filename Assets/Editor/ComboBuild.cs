using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build.Reporting;

public class comboBuild
{
    //This creates a menu item to trigger the dual builds https://docs.unity3d.com/ScriptReference/MenuItem.html 

    [MenuItem("Game Build Menu/Dual Build")]
    public static void BuildGame()
    {
        //This builds the player twice: a build with desktop-specific texture settings (WebGL_Build) as well as mobile-specific texture settings (WebGL_Mobile), and combines the necessary files into one directory (WebGL_Build)
      
        string dualBuildPath    = "../Build/DualBuild";
        string desktopBuildName = "debugendo.dianaportela.fr";
        string mobileBuildName  = "debugendo.dianaportela.fr-mobile";

        string desktopPath = Path.Combine(dualBuildPath, desktopBuildName);
        string mobilePath  = Path.Combine(dualBuildPath, mobileBuildName);
        string[] scenes = new string[] {"Assets/Scenes/Home.unity", "Assets/Scenes/Menu.unity", "Assets/Scenes/Form.unity", "Assets/Scenes/End.unity"};

        UnityEditor.WebGL.UserBuildSettings.codeOptimization = UnityEditor.WebGL.WasmCodeOptimization.DiskSizeLTO;
        
        EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.DXT;
        BuildPipeline.BuildPlayer(scenes, desktopPath, BuildTarget.WebGL, BuildOptions.None);

        EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.ASTC;
        BuildPipeline.BuildPlayer(scenes, mobilePath, BuildTarget.WebGL, BuildOptions.None);

        // Copy the mobile.data file to the desktop build directory to consolidate them both
        FileUtil.CopyFileOrDirectory(Path.Combine(mobilePath, "Build", mobileBuildName + ".data.gz"), Path.Combine(desktopPath, "Build", mobileBuildName + ".data.gz"));
    }  
}