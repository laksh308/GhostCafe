using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class GhostController : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
{
    public enum State
    {
        UnseatedWaiting,   // Spawned - Patience active
        Dragging,
        SeatedWaiting,     // Seated - Patience stopped - waiting for tap
        Eating,            // Eating timer
        Despawning
    }

    public State currentState;

    [Header("Drag")]
    public float dragSmoothing = 30f;
    private Vector3 dragOffset;
    private Camera cam;

    [Header("Seat")]
    public TableSeat currentSeat;
    private Vector3 homePosition;

    [Header("UI")]
    public Slider slider;
    public Image fill;

    private float patienceTimer;
    private float eatingTimer;

    [Header("Timing")]
    public float eatingDuration = 3f; // same for all ghosts

    [Header("VFX")]
    public ParticleSystem seatAcceptVfx;

    // Internal
    private GhostData data;
    private bool suppressClick = false;

    // ---------------------------------------------------------
    void Awake()
    {
        cam = Camera.main;
        data = GetComponent<GhostData>();
    }

    void OnEnable()
    {
        ForceReset();
    }

    // ---------------------------------------------------------
    public void ForceReset()
    {
        StopAllCoroutines();

        currentState = State.UnseatedWaiting;
        currentSeat = null;

        patienceTimer = 0f;
        eatingTimer = 0f;

        if (slider != null)
        {
            slider.value = 0;
            slider.gameObject.SetActive(false);
        }

        transform.localScale = Vector3.one;
    }

    public void SetHomePosition(Vector3 pos)
    {
        homePosition = pos;
        transform.position = pos;
    }

    // ---------------------------------------------------------
    // INPUT
    // ---------------------------------------------------------
    public void OnPointerDown(PointerEventData eventData)
    {
        suppressClick = false;

        // allow pickup only if not eating or despawning
        if (currentState == State.Eating || currentState == State.Despawning)
            return;

        // If player picks up a seated ghost - unseat it
        if (currentState == State.SeatedWaiting && currentSeat != null)
        {
            currentSeat.Unseat();
            currentSeat = null;
        }

        currentState = State.Dragging;
        dragOffset = transform.position - ScreenToWorld(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState != State.Dragging) return;

        suppressClick = true;

        Vector3 wp = ScreenToWorld(eventData.position) + dragOffset;
        transform.position = Vector3.Lerp(transform.position, wp, Time.deltaTime * dragSmoothing);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentState != State.Dragging) return;

        suppressClick = true;

        Vector3 wp = ScreenToWorld(eventData.position);
        TableSeat seat = TableManager.Instance.FindClosestAvailableSeat(wp, 1.5f);

        if (seat != null && seat.TrySeat(this))
            return;

        StartCoroutine(BounceBack());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (suppressClick)
        {
            suppressClick = false;
        }

        if (currentState == State.SeatedWaiting)
        {
            Debug.Log("serve");
            Serve();
        }
            
    }

    // ---------------------------------------------------------
    private Vector3 ScreenToWorld(Vector2 pos)
    {
        Vector3 v = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, -cam.transform.position.z));
        v.z = 0;
        return v;
    }

    private IEnumerator BounceBack()
    {
        Vector3 start = transform.position;
        Vector3 end = homePosition;
        float t = 0f;

        while (t < 0.18f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, t / 0.18f);
            yield return null;
        }

        transform.position = end;
        currentState = State.UnseatedWaiting;
    }

    // ---------------------------------------------------------
    // SEATING
    // ---------------------------------------------------------
    public void OnSeated(TableSeat seat)
    {
        currentSeat = seat;
        transform.position = seat.seatPoint.position;

        if (seatAcceptVfx != null)
            seatAcceptVfx.Play();

        // PATIENCE STOPS
        currentState = State.SeatedWaiting;

        // slider OFF during waiting-for-serve
        slider.gameObject.SetActive(false);
    }

    // ---------------------------------------------------------
    // UPDATE
    // ---------------------------------------------------------
    void Update()
    {
        switch (currentState)
        {
            case State.UnseatedWaiting:
                RunPatienceTimer();
                break;

            case State.Eating:
                RunEatingTimer();
                break;
        }
    }

    // ---------------------------------------------------------
    // PATIENCE
    // ---------------------------------------------------------
    private void RunPatienceTimer()
    {
        float patience = data ? data.waitTime : 10f;
        patienceTimer += Time.deltaTime;

        // NO SLIDER VISUAL DURING PATIENCE

        if (patienceTimer >= patience)
            TimeoutAndDespawn();
    }

    private void TimeoutAndDespawn()
    {
        currentState = State.Despawning;

        if (currentSeat != null)
            currentSeat.Unseat();

        StartCoroutine(FadeOut());
    }

    // ---------------------------------------------------------
    // SERVE - EATING
    // ---------------------------------------------------------
    public void Serve()
    {
       // if (currentState != State.SeatedWaiting)
          //  return;

        currentState = State.Eating;

        float baseReward = data.isGolden ? 1000f : 200f;
        float upgradeMult = UpgradeManager.Instance.GetMultiplier(UpgradeType.BlessedCauldron);
        float rushMult = RushManager.Instance.GetMultiplier();

        int reward = Mathf.RoundToInt(baseReward * upgradeMult * rushMult);
        CurrencyManager.Instance.AddSpiritPoints(reward);
        UIManager.Instance.ShowPopup(transform.position, "+" + reward);
        SFXManager.Instance.PlayServe(data.isGolden);
        HapticManager.VibrateLight();
        RushManager.Instance.AddToMeter(data.isGolden ? 30f : 10f);

        eatingTimer = 0f;

        // Slider only for EATING
        slider.gameObject.SetActive(true);
        slider.value = 0f;
        fill.color = Color.green;
    }

    private void RunEatingTimer()
    {
        eatingTimer += Time.deltaTime;
        slider.value = eatingTimer / eatingDuration;

        if (eatingTimer >= eatingDuration)
            FinishEating();
    }

    private void FinishEating()
    {
        if (currentSeat != null)
            currentSeat.Unseat();

        currentState = State.Despawning;
        StartCoroutine(FadeOut());
    }

    // ---------------------------------------------------------
    private IEnumerator FadeOut()
    {
        Vector3 originalScale = transform.localScale;

        float t = 0f;
        float dur = 0.35f;

        slider.gameObject.SetActive(false);

        while (t < 1f)
        {
            t += Time.deltaTime / dur;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }

        transform.localScale = originalScale;

        GhostPoolManager.Instance.ReturnGhost(gameObject);
    }

    public void PlaySpawnAnimation()
    {
        StartCoroutine(SpawnAnimationRoutine());
    }

    private IEnumerator SpawnAnimationRoutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // Set alpha = 0
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        // Soft tiny rise
        Vector3 endPos = transform.position;
        Vector3 startPos = endPos;
        startPos.y -= 0.25f;     // softer, cuter rise
        transform.position = startPos;

        float duration = 0.95f;  // slower, smoother
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = t / duration;

            // Very smooth cute easing
            float smooth = Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, n));

            // Fade gently
            c.a = smooth;
            sr.color = c;

            // Rise gently
            transform.position = Vector3.Lerp(startPos, endPos, smooth);

            yield return null;
        }

        // Final fix
        c.a = 1f;
        sr.color = c;
        transform.position = endPos;
    }
}