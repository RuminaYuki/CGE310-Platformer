using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviour
{
    private PlayerController playerController;
    private AudioSource audioSource;

    [Header("Jump")]
    [SerializeField] private AudioClip audioJump;
    [Header("Footstep")]
    [SerializeField] private AudioClip[] footstepClips;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void JumpSound()
    {
        if (audioJump == null) return;

        audioSource.PlayOneShot(audioJump);
    }

    public void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        int index = Random.Range(0, footstepClips.Length);
        audioSource.PlayOneShot(footstepClips[index]);
    }
}
