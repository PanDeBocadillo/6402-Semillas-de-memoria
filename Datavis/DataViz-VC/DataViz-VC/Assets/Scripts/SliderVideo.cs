using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SliderVideo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button PlayButton;
    public Button PauseButton;
    public VideoPlayer videoPlayer;
    public Slider videoSlider;
    bool slide = false;
    public bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.Play();
        videoPlayer.Pause();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        videoPlayer.Pause();
        slide = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (slide == false)
        {
            videoPlayer.Play();
        }
        else
        {
            videoPlayer.Pause();
        }
        float frame = (float) videoSlider.value * (float)videoPlayer.frameCount;
        videoPlayer.frame = (long)frame;
        slide = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (slide == false)
        {
            videoSlider.value = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
        }
        if(videoSlider.value >= 0.99f)
        {
            PlayButton.gameObject.SetActive(true);
        }
    }
}

