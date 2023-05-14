using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void ShowExample()
    {
        SceneManager.UnloadSceneAsync("EngineAssemble");
        SceneManager.LoadScene("Example");
    }
    public void Exit()
    {
        SceneManager.UnloadSceneAsync("Example");
        SceneManager.LoadScene("EngineAssemble");
    }
}
