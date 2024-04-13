using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; // Cache the main camera
    }

    void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the object's position
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        // Update the position of the object to follow the mouse, considering the offset
        transform.position = GetMouseWorldPos() + offset;
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convert mouse screen position to world position
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -mainCamera.transform.position.z; // Ensure the z-position is consistent
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
