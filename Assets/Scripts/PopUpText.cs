using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour
{
    public float floatSpeed = 40f;
    public float duration = 1f;
    public TextMeshProUGUI text;
    private float timer;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init(string message, Vector2 screenPos)
    {
        transform.position = screenPos;
        text.text = message;
        timer = 0f;
        canvasGroup.alpha = 1f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        canvasGroup.alpha = 1f - (timer / duration);

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}