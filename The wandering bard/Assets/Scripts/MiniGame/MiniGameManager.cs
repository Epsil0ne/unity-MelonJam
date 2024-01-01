using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance = null;
    public List<Song> song = new();
    public TMP_Text dialogue;
    public List<string> dialogs;
    [HideInInspector] public AudioSource audioSource;

    private void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        audioSource = GetComponent<AudioSource>();

        foreach (var item in song)
        {
            item.gameObject.SetActive(false);
        }

        StartCoroutine(



                BeginSongGame());
    }

    public IEnumerator BeginSongGame()
    {
        yield return new WaitForSeconds(1f);
        if (GameManager.instance!=null)
        {
            dialogue.text = dialogs[GameManager.currentMinigameID];
            song[GameManager.currentMinigameID].beginMusic(this);
        }
        else
        {
            song[3].beginMusic(this);
        }
    }

    public IEnumerator EndSongGame()
    {
        yield return new WaitForSeconds(2f);
        //show text
        //wait a few second
        StartCoroutine(GameManager.instance.QuitMinigame());
    }

    internal void RestartGame()
    {
        GameManager.instance.RestartMinigame();
    }
}