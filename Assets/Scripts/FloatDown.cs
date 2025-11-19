/*using UnityEngine;

public class FloatDown : MonoBehaviour
{
    public float speed = 1f;
    private Vector3 target;
    private bool isMoving = false;

    public void MoveTo(Vector3 targetPosition)
    {
        target = targetPosition;
        isMoving = true;

        GhostInteractable interactable = GetComponent<GhostInteractable>();
        if (interactable != null)
        {
            interactable.canBeServed = false;
            interactable.canTimeout = false;
        }
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            isMoving = false;

            GhostInteractable interactable = GetComponent<GhostInteractable>();
            if (interactable != null)
            {
                if (Vector3.Distance(target, FindObjectOfType<QueueManager>().stopPoint.position) < 0.1f)
                {
                    interactable.canBeServed = true;
                    interactable.canTimeout = true;
                    interactable.isAtStopPoint = true;
                }
            }
        }
    }
}*/