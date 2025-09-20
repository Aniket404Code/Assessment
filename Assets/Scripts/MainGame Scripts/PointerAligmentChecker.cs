using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointerAligmentChecker : MonoBehaviour
{
    [Header("Mask & Tracing")]
    public GameObject myMask;
    public float spawnInterval = 0.05f;
    private float lastSpawnTime = 0f;

    private float tracedPoints = 0;
    private float totalPoints = 0;
    public float requiredCompletion = 0.9f; // 90%

    [Header("Letter Sequence")]
    public List<GameObject> letters;           // Letters A, B, C …
    public int currentLetterIndex = 0;         // Current letter
    public Text levelCompleteText;             // UI Text for level complete

    [Header("UI Feedback")]
    public GameObject feedbackPopup;           // Single letter completed popup

    void Start()
    {
        // Activate only first letter
        for (int i = 0; i < letters.Count; i++)
        {
            letters[i].SetActive(i == 0);
        }

        if (levelCompleteText != null) levelCompleteText.gameObject.SetActive(false);
        if (feedbackPopup != null) feedbackPopup.SetActive(false);
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current != null ? Mouse.current.position.ReadValue() : Vector2.zero;
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        pos.z = 0;

        // --- CONTINUOUS SPAWNING ---
        if (TouchMovementHandler.Instance.isAligned)
        {
            if (Time.time - lastSpawnTime >= spawnInterval)
            {
                GameObject go = Instantiate(myMask, pos, Quaternion.identity);
                go.transform.parent = GameObject.Find("Masks").transform;

                tracedPoints++; // count traced mask points
                lastSpawnTime = Time.time;
            }
        }

        // --- Mouse Release ---
        if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            CheckCompletion();
        }

        // --- Touch Release ---
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
                {
                    CheckCompletion();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("myPath"))
        {
            TouchMovementHandler.Instance.isAligned = true;

            // Store total path length from line renderer
            LineRenderer lr = collision.GetComponent<LineRenderer>();
            if (lr != null) totalPoints = lr.positionCount;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("myPath"))
        {
            if (TouchMovementHandler.Instance.isAligned)
            {
                TouchMovementHandler.Instance.isAligned = false;
                CheckCompletion();
            }
        }
    }

    void CheckCompletion()
    {
        if (totalPoints <= 0) return;

        float percent = tracedPoints / totalPoints;

        if (percent >= requiredCompletion)
        {
            Debug.Log("✅ Alphabet Completed!");
            DestroyPointer(true);   // keep masks

            // --- Show popup feedback for this letter ---
            StartCoroutine(ShowLetterFeedback(1.5f));

            // Unlock next letter
            currentLetterIndex++;
            if (currentLetterIndex < letters.Count)
            {
                letters[currentLetterIndex].SetActive(true);
            }
            else
            {
                // All letters complete → Level Complete
                if (levelCompleteText != null)
                {
                    levelCompleteText.gameObject.SetActive(true);
                    StartCoroutine(HideLevelCompleteMessage());
                }
            }
        }
        else
        {
            Debug.Log("❌ Incomplete, try again!");
            DestroyPointer(false);  // remove masks
        }

        // Reset for next attempt
        tracedPoints = 0;
        totalPoints = 0;
    }

    void DestroyPointer(bool keepMasks)
    {
        if (TouchMovementHandler.Instance.PointerGO != null)
        {
            if (!keepMasks) // only delete masks if fail
            {
                Transform masksParent = GameObject.Find("Masks").transform;
                if (masksParent.childCount > 0)
                {
                    foreach (Transform child in masksParent)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

            Destroy(TouchMovementHandler.Instance.PointerGO);
        }
    }

    IEnumerator ShowLetterFeedback(float duration = 1.5f)
    {
        if (feedbackPopup == null) yield break;

        feedbackPopup.SetActive(true);
        yield return new WaitForSeconds(duration);
        feedbackPopup.SetActive(false);
    }

    IEnumerator HideLevelCompleteMessage()
    {
        yield return new WaitForSeconds(2f);
        if (levelCompleteText != null) levelCompleteText.gameObject.SetActive(false);
    }
}
