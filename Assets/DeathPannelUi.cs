using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class DeathPannelUi : MonoBehaviour
{
   
float lerpDuration = 3; 

public float Layerclimb = 143; 
public float TimeAlive = 200; 
public float TimeAlamode = 99; 
float valueToLerp1;
float valueToLerp2;
float valueToLerp3;

public TextMeshProUGUI Val1;
public TextMeshProUGUI Val2;
public TextMeshProUGUI Val3;
  void OnEnable()
    {
        StartCoroutine(Lerp());
        
        
    }


    IEnumerator Lerp()
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp1 = Mathf.Lerp(0, Layerclimb, timeElapsed / lerpDuration);
             valueToLerp2 = Mathf.Lerp(0, TimeAlive, timeElapsed / lerpDuration);
              valueToLerp3 = Mathf.Lerp(0, TimeAlamode, timeElapsed / lerpDuration);

            timeElapsed += Time.deltaTime;

            Val1.text = ((int)valueToLerp1).ToString();
             Val2.text = ((int)valueToLerp2).ToString();
              Val3.text = ((int)valueToLerp3).ToString();
            yield return null;


        }
    
         valueToLerp1 = Layerclimb;
         valueToLerp2 = TimeAlive;
         valueToLerp3 = TimeAlamode;

         Val1.text = ((int)Layerclimb).ToString();
         Val2.text = ((int)TimeAlive).ToString();
         Val3.text = ((int)TimeAlamode).ToString();
         
    }

    public void LoadMenu()
    {
       SceneManager.LoadScene("MenuScene");
    }

    
}
