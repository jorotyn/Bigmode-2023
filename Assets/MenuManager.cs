using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    private FMOD.Studio.EventInstance SelectInstance;
    public string SelectEvent;

    void Start()
    {
         SelectInstance = FMODUnity.RuntimeManager.CreateInstance(SelectEvent);

    }

    public void LoadPlayScene()
    {
        SelectInstance.start();
        SceneManager.LoadScene("devtest");
    }
}
