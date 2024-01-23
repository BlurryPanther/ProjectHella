using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressEnter : MonoBehaviour
{

    public string SceneName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Cambiar a la escena especificada
            LoadScene();
        }
    }

    private void LoadScene()

    {
        SceneManager.LoadScene(SceneName);
    }
}
