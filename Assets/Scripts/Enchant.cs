using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enchant : MonoBehaviour
{
    public GameObject cursor; // Cursor object with a particle system as a child
    public Image progressGlobe;
    public int currentCustomer = 1;
    public bool finishedDrawing = false;

    private bool isDrawing = false;
    private float drawAmount = 0.0f; // Current amount drawn
    private float requiredDrawAmount = 100.0f; // Required amount to complete drawing
    
    private void Start()
    {
        // Assuming the particle system is a child of the cursor GameObject
        cursor.SetActive(false);
    }

    private void Update()
    {
        HandleInput();
    }

    private void OnEnable()
    {
        finishedDrawing = false;
        drawAmount = 0.0f;
        progressGlobe.rectTransform.localPosition = new Vector3(0, -progressGlobe.rectTransform.rect.height, 0);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // On mouse button down
        {
            isDrawing = true;
            cursor.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(0)) // On mouse button up
        {
            isDrawing = false;
            cursor.SetActive(false);
        }

        if (isDrawing)
        {
            // Convert mouse position to world point
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursor.transform.position = cursorPosition;
            drawAmount += Time.deltaTime * 20; // Increment draw amount, adjust rate as necessary
            drawAmount = Mathf.Clamp(drawAmount, 0, requiredDrawAmount);
            UpdateProgressBar();
        }
    }

    private void UpdateProgressBar()
    {
        float progress = drawAmount / requiredDrawAmount;
        progressGlobe.rectTransform.localPosition = new Vector3(0, progressGlobe.rectTransform.rect.height * progress - progressGlobe.rectTransform.rect.height, 0);
        if (progress >= 1.0f && !finishedDrawing)
        {
            FinishEnchanting();
            finishedDrawing = true;
        }
    }
    
    private void FinishEnchanting()
    {
        // find the puzzle piece parent transform with the most children
        Transform parent = null;
        int maxChildren = 0;
        foreach (PuzzlePiece p in FindObjectsOfType<PuzzlePiece>())
        {
            if (p.transform.parent.childCount > maxChildren)
            {
                parent = p.transform.parent;
                maxChildren = p.transform.parent.childCount;
            }
        }
        
        foreach (PuzzlePiece p in FindObjectsOfType<PuzzlePiece>())
        {
            if (p.transform.parent != parent)
            {
                Destroy(p.gameObject);
            }
        }
        
        // make the parent grow over time
        StartCoroutine(GrowParent(parent));
    }
    
    private IEnumerator GrowParent(Transform parent)
    {
        float growTime = 2.0f;
        float elapsedTime = 0.0f;
        Vector3 startScale = parent.localScale;
        Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);
        
        while (elapsedTime < growTime)
        {
            parent.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / growTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        parent.localScale = targetScale;
        // reset
        drawAmount = 0.0f;
        progressGlobe.rectTransform.localPosition = new Vector3(0, -progressGlobe.rectTransform.rect.height, 0);
        enabled = false;
        
        // callback to finish summoning
        switch (currentCustomer)    
        {
            case 1:
                FindObjectOfType<Dialogue>().FinishSummon1(parent);
                break;
            case 2:
                FindObjectOfType<Dialogue>().FinishSummon2(parent);
                break;
            case 3:
                FindObjectOfType<Dialogue>().FinishSummon3(parent);
                break;
            default:
                Debug.LogError("Invalid customer number: " + currentCustomer);
                break;
        }
    }
}
