using System.Collections;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    public Material inHitZoneMaterial;
    public Material outHitZoneMaterial;
    Transform currentNote = null;
    public ParticleSystem particles;
    public MeshRenderer charmLoading;
    public Material charmLoadingMaterial;

    int numberOfNotes;
    public Transform notes;
    AudioSource audioSource;

    private void Start()
    {
        Color color = charmLoadingMaterial.color;
        color.a = 0f;
        charmLoadingMaterial.color = color;

        numberOfNotes = notes.childCount;

        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentNote = other.transform;
       // currentNote.GetComponent<MeshRenderer>().material = inHitZoneMaterial;
       currentNote.transform.GetChild(0).transform.localScale = new Vector3(2,2,2);
    }

    private void OnTriggerExit(Collider other)
    {
       // currentNote.GetComponent<MeshRenderer>().material = outHitZoneMaterial;
        currentNote = null;
        MiniGameManager.instance.RestartGame();
    }

    private void Update()
    {
        if (GameManager.instance.Input.interact)
        {
            if (currentNote != null)
                NoteHit();
            else
                NoteMiss();

            GameManager.instance.Input.interact = false;
        }
    }

    private void NoteMiss()
    {        
        audioSource.Play();
    }

    private void NoteHit()
    {
        Destroy(currentNote.gameObject);

        StartCoroutine(MoveParticle());
    }

    IEnumerator MoveParticle()
    {
        particles.enableEmission = true;

        float duration = 0.25f;
        Vector3 targetPosition = charmLoading.transform.position;
        float timeElapsed = 0;
        Vector3 startPosition = particles.transform.position;
        while (timeElapsed < duration)
        {
            particles.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        particles.transform.position = targetPosition;
        particles.enableEmission = false;
        particles.transform.position = startPosition;

        Color color = charmLoading.material.color;
        
        color.a += (1f / numberOfNotes);
        charmLoading.material.color = color;
    }
}