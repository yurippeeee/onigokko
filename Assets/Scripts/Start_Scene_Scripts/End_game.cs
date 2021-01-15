using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End_game : MonoBehaviour
{
    public static bool is_quit = false;

    public void Quit()
    {
        is_quit = true;

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

    #elif UNITY_STANDALONE
      UnityEngine.Application.Quit();

    #endif
    }
    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.wantsToQuit += WantsToQuit;
    }


    static bool WantsToQuit()
    {
        if (is_quit)
        {
            return true;
        }
        return false;
    }

}
