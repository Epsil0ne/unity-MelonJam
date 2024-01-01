using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour
{

    public Transform Notes;
    public Transform HitZone;
    MeshRenderer[] notesRenderer;
    public AudioClip audioClip;
    float songDuration;
    public float noteRowLength;
    bool musicCanPlay = false;

    
    private void Awake()
    {
        songDuration = audioClip.length;
    }
    void Start()
    {
         notesRenderer = Notes.GetComponentsInChildren<MeshRenderer>();
    }
    MiniGameManager miniGameManager;
    public void beginMusic(MiniGameManager _miniGameManager)
    {
        gameObject.SetActive(true);
         miniGameManager = _miniGameManager;
        musicCanPlay = true;
        miniGameManager.audioSource.clip = audioClip;
        miniGameManager.audioSource.Play();

    }

    //  float speed = 2f;
    float timeElapsed;   
    float lerpedValue;
    void Update()
    {
        if (!musicCanPlay) return;


        // transform.position = Vector3.forward* speed * Time.deltaTime;
        if (timeElapsed < songDuration)
        {
            lerpedValue = Mathf.Lerp(0, noteRowLength, timeElapsed / songDuration);
            timeElapsed += Time.deltaTime;
            Debug.LogWarning(lerpedValue);

            Notes.transform.localPosition = Vector3.left * lerpedValue;
        }
        else
        {
            musicCanPlay = false;
            StartCoroutine(miniGameManager.EndSongGame());
        }       
    }
}
