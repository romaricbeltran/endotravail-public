using UnityEngine;
using System.Runtime.InteropServices;

public class Exit : MonoBehaviour
{
	public static void QuitFullscreen()
    {
    	#if !UNITY_EDITOR && UNITY_WEBGL
    	ExitFullScreen();
        #endif
    }

	[DllImport("__Internal")]
	private static extern void ExitFullScreen();
}
