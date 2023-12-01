using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntensity : MonoBehaviour
{
    
    private FMOD.Studio.EventInstance instance;

    
    public string fmodEvent;

    [SerializeField][Range(0f, 1f)]
    private float intensity;


    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    
    void Update()
    {
        intensity = (gameObject.transform.position.y / 20);
        instance.setParameterByName("Hauteur", intensity);
        
    }
}
