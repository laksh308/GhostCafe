using UnityEngine;

public class TapEffectManager : MonoBehaviour
{
    public GameObject tapPulsePrefab;

    public void ShowTapEffect(Vector2 screenPosition)
    {
        if (TapInputBlocker.BlockInput) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0f;

        GameObject pulse = Instantiate(tapPulsePrefab, worldPos, Quaternion.identity);

        Animator anim = pulse.GetComponent<Animator>();
        float animLength = anim.runtimeAnimatorController.animationClips[0].length;
        Destroy(pulse, animLength);
    }
}