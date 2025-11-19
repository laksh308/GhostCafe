using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public AudioSource audioSource;
    public AudioClip normalServe;
    public AudioClip goldenServe;
    public AudioClip upgradeclip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayServe(bool isGolden)
    {
        AudioClip clip = isGolden ? goldenServe : normalServe;
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void UpgradeSound()
    {
        audioSource.PlayOneShot(upgradeclip);
    }
}