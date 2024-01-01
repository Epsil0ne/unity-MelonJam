using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_GO : MonoBehaviour
{
    public int ID;

    [SerializeField] GameObject interactionHelperUI;

    [SerializeField] GameObject dialogueUI;
    public List<string> QuestTextsA = new();
    public List<string> QuestTextsB = new();
    int dialogAProgress = 0;
    int dialogBProgress = 0;
    public TMP_Text dialogueText;

    public Renderer tempObjColor;

    public Material completeMat;

   

    private void Start()
    {
        interactionHelperUI.SetActive(false);
        dialogueUI.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.instance.Input.interact)
        {
            if ((interactionHelperUI.activeInHierarchy || dialogueUI.activeInHierarchy) && (GameManager.instance.progressNPC[ID] == 0 || GameManager.instance.progressNPC[ID] == 1))
            {
                if (dialogAProgress == 0)
                {
                    interactionHelperUI.SetActive(false);
                    dialogueUI.SetActive(true);
                    dialogueText.text = QuestTextsA[0];
                    // GameManager.instance.PlayerInput.DeactivateInput();
                    GameManager.instance.PlayerInput.actions.FindAction("Move").Disable();
                    dialogAProgress++;
                }
                else if (dialogAProgress == QuestTextsA.Count)
                {
                    GameManager.instance.PlayerInput.actions.FindAction("Move").Enable();
                    dialogueUI.SetActive(false);
                    GameManager.instance.NPCTalkDone(ID);
                    dialogAProgress = 0;
                }
                else
                {
                    dialogueText.text = QuestTextsA[dialogAProgress];
                    dialogAProgress++;                   
                }
                GameManager.instance.Input.interact = false;
            }

            if ((interactionHelperUI.activeInHierarchy || dialogueUI.activeInHierarchy) && (GameManager.instance.progressNPC[ID] == 2 || GameManager.instance.progressNPC[ID] == 3))
            {
                if (dialogBProgress == 0)
                {
                    GameManager.instance.CharmGiven();
                    interactionHelperUI.SetActive(false);
                    dialogueUI.SetActive(true);
                    dialogueText.text = QuestTextsB[0];
                    GameManager.instance.PlayerInput.actions.FindAction("Move").Disable();
                    dialogBProgress++;
                   
                }
                else if (dialogBProgress == QuestTextsB.Count)
                {
                    GameManager.instance.PlayerInput.actions.FindAction("Move").Enable();
                    dialogueUI.SetActive(false);
                    GameManager.instance.QuestDone(ID);
                    dialogBProgress = 0;

                    tempObjColor.material = completeMat;
                }
                else
                {
                    dialogueText.text = QuestTextsB[dialogBProgress];
                    dialogBProgress++;
                }
                GameManager.instance.Input.interact = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (GameManager.instance.progressNPC[ID] != 3)
            interactionHelperUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        interactionHelperUI.SetActive(false);
        dialogueUI.SetActive(false);
    }
}