using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CambiarParametroMixer : MonoBehaviour
{
    public AudioMixer miAudioMixer;
    public string nombreParametro;
    [SerializeField]
    public float valorMax = 1f;
    [SerializeField]
    public float valorMin = 0f;
 
    public void CambiarParametroLog(float valorSlider)
    {
        float valorLog = Mathf.Log10(valorSlider) * 20;

        if (valorSlider == 0)
        {
            miAudioMixer.SetFloat(nombreParametro, -80f);
        }
        else
        {
            miAudioMixer.SetFloat(nombreParametro, valorLog); 
        }
    }

    public void CambiarParametroLin(float valorSlider)
    {
        valorSlider = Map(valorSlider, 0, 1, valorMin, valorMax);
        miAudioMixer.SetFloat(nombreParametro, valorSlider);      
    }


    float Map(float s, float a1, float a2, float b1, float b2)
       {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
       }

}


