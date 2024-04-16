using System.Collections.Generic;
using TorcheyeUtility;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public bool isBeingDragged;
    public bool canBeDragged = true;
    
    private Vector3 _offset;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
        if (transform.parent == null)
        {
            // create new parent object
            GameObject parent = new GameObject("PuzzlePieceParent");
            parent.transform.position = transform.position;
            transform.parent = parent.transform;
        }
    }

    private void OnMouseDown()
    {
        if (!canBeDragged) return;
        _offset = transform.parent.position - GetMouseWorldPos();
        isBeingDragged = true;
    }

    private void OnMouseDrag()
    {
        if (!canBeDragged) return;
        // Update the position of the object to follow the mouse, considering the offset
        transform.parent.position = GetMouseWorldPos() + _offset;
    }
    
    private void OnMouseUp()
    {
        isBeingDragged = false;

        var closeJointList = new List<Joint>();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            var p = transform.parent.GetChild(i);
            foreach (Joint j in p.GetComponentsInChildren<Joint>())
            {
                if (j.GetTargetJoint() == null) continue;
                if (j.isConnected) continue;
            
                closeJointList.Add(j);
            }
        }
        
        if (closeJointList.Count == 0) return;
        
        // find the closest pair
        Joint selfJoint = closeJointList[0];
        Joint targetJoint = closeJointList[0].GetTargetJoint();
        float minDistance = Vector3.Distance(targetJoint.transform.position, selfJoint.transform.position);
        foreach (Joint j in closeJointList)
        {
            Joint target = j.GetTargetJoint();
            float distance = Vector3.Distance(j.transform.position, target.transform.position);
            if (distance < minDistance)
            {
                targetJoint = target;
                selfJoint = j;
                minDistance = distance;
            }
        }
        
        // Connect the joints
        selfJoint.Connect();
        targetJoint.Connect();
        AudioManager.Instance.PlaySoundEffect(AudioManager.SoundEffect.Flesh);
        
        // set normal for other close joints
        foreach (Joint j in closeJointList)
        {
            if (j == selfJoint) continue;
            j.SetColor(j.normal);
            j.GetTargetJoint().SetColor(j.normal);
        }
        
        // Snap the piece to the target position and connect
        transform.parent.Translate(targetJoint.transform.position - selfJoint.transform.position, Space.World);
            
        var tempChildList = new List<Transform>();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            tempChildList.Add(transform.parent.GetChild(i));
        }

        foreach (var c in tempChildList)
        {
            c.parent = targetJoint.puzzlePiece.transform.parent;
        }

        GetComponent<Collider2D>().enabled = true;
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convert mouse screen position to world position
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -_mainCamera.transform.position.z; // Ensure the z-position is consistent
        return _mainCamera.ScreenToWorldPoint(mousePoint);
    }
    
    // rotate the piece
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            transform.parent.Rotate(0, 0, 30);
        }
    }
}
