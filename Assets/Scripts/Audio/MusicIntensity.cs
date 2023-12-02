using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicIntensity : MonoBehaviour
{
    
    private FMOD.Studio.EventInstance instance;

    public GameObject player;
    public string fmodEvent;

    [SerializeField][Range(0f, 1f)]
    private float intensity;

    public float PitchSpeed;
    private float pitchValue;
    public int theme; 

    public bool DIE;


    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
        theme = Random.Range(0,2);
        instance.setParameterByName("MODE", theme);

    }

    
    void Update()
    {
        intensity = (player.transform.position.y / 20);
        instance.setParameterByName("Hauteur", intensity);


         if (DIE)
        {
            pitchValue += PitchSpeed;
            instance.setParameterByName("PITCH SLOW", pitchValue);
        }
        
    }

    public void StopMusic()
    {
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
