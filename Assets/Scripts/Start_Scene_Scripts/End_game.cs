﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_game : MonoBehaviour
{
    public void Quit()
    {
      #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;

      #elif UNITY_STANDALONE
      UnityEngine.Application.Quit();

      #endif
    }
}
