using DigitalRuby.RainMaker;
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public StarterAssetsInputs Input;
    [HideInInspector] public PlayerInput PlayerInput;
    public static GameManager instance = null;
    public List<Ressources_GO> ressourceList = new();
    public List<NPC_GO> npcList = new();
    public List<GameObject> uncoloredZoneList = new();
    public RainScript rainScript;

    SphereCollider LastNPCCollider;

    AudioSource audioSourceAmbiance, audioSourceEffects;
    AudioListener audioListener;
    [SerializeField] AudioClip audioObjectTaken;
    [SerializeField] AudioClip audioCharmGiven;
    [SerializeField] AudioClip audioAmbianceEndGame;

    [SerializeField] AudioSource audioAmbianceBase;
    [SerializeField] AudioSource audioAmbianceRain;
    [SerializeField] AudioSource audioAmbianceBird;

    static public int currentMinigameID = 0;

    private void Awake()
    {
        PlayerInput = Input.GetComponent<PlayerInput>();
        audioSourceEffects = Input.GetComponent<AudioSource>();
        audioListener = Input.GetComponent<AudioListener>();
    }

    void Start()
    {
        Debug.Assert(ressourceList.Count == 4);
        Debug.Assert(uncoloredZoneList.Count == 4);

        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        LastNPCCollider = npcList[3].GetComponent<SphereCollider>();
        LastNPCCollider.enabled = false;
    }

    public void ObjectPickedUp(int iD)
    {
        progressNPC[iD] = 2;
        audioSourceEffects.clip = audioObjectTaken;
        audioSourceEffects.Play();
        currentMinigameID = iD;
        StartCoroutine(BeginMiniGame());
    }

    private IEnumerator BeginMiniGame()
    {
        yield return new WaitForSeconds(1f);
        RenderSettings.fog = false;
        GameManager.instance.PlayerInput.actions.FindAction("Move").Disable();
        audioAmbianceBase.Pause();
        audioListener.enabled = false;
        SceneManager.LoadScene("MiniGame", LoadSceneMode.Additive);
    }

    public IEnumerator QuitMinigame()
    {
        yield return new WaitForSeconds(2f);
        RenderSettings.fog = true;
        SceneManager.UnloadSceneAsync("MiniGame");
        audioListener.enabled = true;
        audioAmbianceBase.Play();
        GameManager.instance.PlayerInput.actions.FindAction("Move").Enable();
        yield return new WaitForSeconds(1f);
    }

    public void RestartMinigame()
    {
        SceneManager.UnloadSceneAsync("MiniGame");
        SceneManager.LoadScene("MiniGame", LoadSceneMode.Additive);
    }

    internal void NPCTalkDone(int iD)
    {
        progressNPC[iD] = 1;
    }

    //1 when talked to
    //2 when object pickup
    //3 when object brought
    [HideInInspector] public int[] progressNPC = { 0, 0, 0, 0 };

    private void LateUpdate()
    {
        if (GameManager.instance.Input.interact)
            GameManager.instance.Input.interact = false;
    }

    public void CharmGiven()
    {
        audioSourceEffects.clip = audioCharmGiven;
        audioSourceEffects.Play();
    }

    internal void QuestDone(int iD)
    {
        progressNPC[iD] = 3;
        if (progressNPC[0] == 3 && progressNPC[1] == 3 && progressNPC[2] == 3)
        {
            LastNPCCollider.enabled = true;
        }

        RevealableObject[] objectsToColor = uncoloredZoneList[iD].GetComponentsInChildren<RevealableObject>();
        foreach (RevealableObject item in objectsToColor)
        {
            item.RevealElement();
        }
        ProgressQuest();
        //color the world
        if (iD == 3)
        {
            GameWon();
        }
    }
    int questProgress = 0;
    private void ProgressQuest()
    {
        questProgress++;
        if (questProgress<2)
        {
            rainScript.RainIntensity /= 2;
        }
        else
        {
            rainScript.RainIntensity = 0;
            audioAmbianceRain.Pause();
        }
       
        rainScript.EnableWind = questProgress <= 2;

       

    }

    private void GameWon()
    {
        audioAmbianceBase.clip = audioAmbianceEndGame;
        audioAmbianceBase.Play();
        audioAmbianceBird.Play();
    }
}