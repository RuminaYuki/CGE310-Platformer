using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundCharacter : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Jump")]
    [SerializeField] private AudioClip audioJump;
    [Header("Footstep")]
    [SerializeField] private AudioClip[] footstepClips;
    [Header("Dash")]
    [SerializeField] private AudioClip dashClip;
    [Header("Attack")]
    [SerializeField] private AudioClip attackClip;

    void Awake()
    {
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

    public void DashSound()
    {
        if (dashClip == null) return;
        audioSource.clip = dashClip;
        audioSource.Play();
    }

    public void Attack()
    {
        if (attackClip == null) return;
        audioSource.volume = 1f;
        audioSource.PlayOneShot(attackClip);
    }
}
