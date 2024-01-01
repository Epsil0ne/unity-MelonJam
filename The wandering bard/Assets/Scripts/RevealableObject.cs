using UnityEngine;

public class RevealableObject : MonoBehaviour
{
    Material[] coloredMaterial;
    Renderer myRenderer;

    void OnEnable()
    {
        myRenderer = GetComponent<Renderer>();

        coloredMaterial = myRenderer.materials;

        Material initialMaterial = (Material)Resources.Load("InitialGrey", typeof(Material));

        Material[] newMaterials = new Material[myRenderer.materials.Length];

        for (int i = 0; i < myRenderer.materials.Length; i++)
        {
            newMaterials[i] = initialMaterial;
        }
        myRenderer.materials = newMaterials;
    }

    public void RevealElement()
    {
        myRenderer.materials = coloredMaterial;
        Destroy(this);
    }
}