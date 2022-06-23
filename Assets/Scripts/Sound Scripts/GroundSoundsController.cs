using UnityEngine;

public class GroundSoundsController : MonoBehaviour
{
    [SerializeField] public AudioSource source;
    [SerializeField] public AudioClip metalSound;
    
    void Update()
    {
        
    }

    private void Start()
    {
    }
    public void PlayMetalWalkingSound(bool play)
    {
        if (play)
        {
            Debug.Log("FOR REAL MAN, SOUNDS SHOULD BE PLAYING RIGHT NOW");
            source.PlayOneShot(metalSound);
        }
        else source.Stop();
    }
    
}
