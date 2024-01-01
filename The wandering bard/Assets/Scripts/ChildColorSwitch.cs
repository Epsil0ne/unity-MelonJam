using UnityEngine;

public class ChildColorSwitch : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (t.GetComponent<Renderer>() != null && t.GetComponent<Renderer>().material != null) //transform.GetComponent<Collider>() != null &&
                t.gameObject.AddComponent<RevealableObject>();
        }
    }
}