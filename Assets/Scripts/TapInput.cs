using UnityEngine;
using UnityEngine.EventSystems;

public class TapInput : MonoBehaviour
{
    public TapEffectManager tapEffectManager;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (tapEffectManager != null)
                tapEffectManager.ShowTapEffect(Input.mousePosition);
        }
    }
}