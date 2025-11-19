using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject popupPrefab;
    public Canvas canvas;

    void Start()
    {

    }
    public void ShowPopup(Vector3 worldPos, string message)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        GameObject popup = Instantiate(popupPrefab, canvas.transform);
        popup.GetComponent<PopUpText>().Init(message, screenPos);
    }

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
       /* if (queueManager == null || queueText == null) return;

        int current = queueManager.GetCurrentQueueSize();
        int max = queueManager.GetMaxQueueSize();

        queueText.text = $"Queue: {current} / {max}";*/
    }
}