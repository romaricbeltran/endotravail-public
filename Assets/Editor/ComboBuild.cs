using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using UnityEditor.Build.Reporting;

public class comboBuild
{
    //This creates a menu item to trigger the dual builds https://docs.unity3d.com/ScriptReference/MenuItem.html 

    [MenuItem("Game Build Menu/Dual Build")]
    public static void BuildGame()
    {
      //This builds the player twice: a build with desktop-specific texture settings (WebGL_Build) as well as mobile-specific texture settings (WebGL_Mobile), and combines the necessary files into one directory (WebGL_Build)
      
      string dualBuildPath    = "../WebGLBuilds";
      string desktopBuildName = "build-webgl-endotravail-responsive";
      string mobileBuildName  = "build-webgl-mobile-endotravail-responsive";

      string desktopPath = Path.Combine(dualBuildPath, desktopBuildName);
      string mobilePath  = Path.Combine(dualBuildPath, mobileBuildName);
      string[] scenes = new string[] {"Assets/Scenes/Home.unity", "Assets/Scenes/Menu.unity", "Assets/Scenes/Form.unity", "Assets/Scenes/End.unity"};

      //EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.DXT;
      //BuildPipeline.BuildPlayer(scenes, desktopPath, BuildTarget.WebGL, BuildOptions.Development); 

      EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.ASTC;
      BuildPipeline.BuildPlayer(scenes,  mobilePath, BuildTarget.WebGL, BuildOptions.None); 

      // // Copy the mobile.data file to the desktop build directory to consolidate them both
      // FileUtil.CopyFileOrDirectory(Path.Combine(mobilePath, "Build", mobileBuildName + ".data"), Path.Combine(desktopPath, "Build", mobileBuildName + ".data"));

      // Compress files in the desktop build directory
      CompressFiles(mobilePath);
    }

    private static void CompressFiles(string directoryPath)
    {
        // Compress all .js, .wasm, and .data files in the specified directory
        string[] filesToCompress = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".js") || s.EndsWith(".wasm") || s.EndsWith(".data"))
            .ToArray();

        foreach (string filePath in filesToCompress)
        {
            CompressFile(filePath);
            File.Delete(filePath);
        }
    }

    private static void CompressFile(string filePath)
    {
        // Compress the specified file using gzip compression
        string compressedFilePath = filePath + ".gz";
        using (FileStream originalFileStream = File.OpenRead(filePath))
        {
            using (FileStream compressedFileStream = File.Create(compressedFilePath))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }
            }
        }
    }
}