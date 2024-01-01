using UnityEngine;

public class Ressources_GO : MonoBehaviour
{
    [SerializeField] GameObject interactionHelperUI;
    public int ID;

    void Start()
    {
        interactionHelperUI.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.Input.interact && interactionHelperUI.activeInHierarchy)
        {
            GameManager.instance.ObjectPickedUp(ID);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.progressNPC[ID] == 1)
            interactionHelperUI.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        interactionHelperUI.SetActive(false);
    }
}