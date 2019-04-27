using UnityEditor;

[InitializeOnLoad]
public class StopPlayingOnRecompile
{
	static StopPlayingOnRecompile()
    {
        EditorApplication.update -= MonitorCompilation;

        EditorApplication.update += MonitorCompilation;
	}

    private static void MonitorCompilation()
    {
        if (EditorApplication.isCompiling && EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
    }
}