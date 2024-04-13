using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public bool isBeingDragged;
    
    private Vector3 _offset;
    private Camera _mainCamera;
    private Joint _selfJoint, _otherJoint;
    
    public void SetConnectTarget(Joint selfJoint, Joint otherJoint)
    {
        _selfJoint = selfJoint;
        _otherJoint = otherJoint;
    }
    
    public void ClearConnectTarget()
    {
        _selfJoint = null;
        _otherJoint = null;
    }

    private void Start()
    {
        _mainCamera = Camera.main; // Cache the main camera
    }

    private void OnMouseDown()
    {
        // Calculate the offset between the mouse position and the object's position
        _offset = transform.parent.position - GetMouseWorldPos();
        isBeingDragged = true;
    }

    private void OnMouseDrag()
    {
        // Update the position of the object to follow the mouse, considering the offset
        transform.parent.position = GetMouseWorldPos() + _offset;
    }
    
    private void OnMouseUp()
    {
        isBeingDragged = false;
        if (_selfJoint != null)
        {
            // Snap the piece to the target position and connect
            transform.parent.Translate(_otherJoint.transform.position - _selfJoint.transform.position, Space.World);
            
            //Destroy(transform.parent.gameObject);
            for (int i = transform.parent.childCount - 1; i >= 0; i--)
            {
                transform.parent.GetChild(i).parent = _otherJoint.puzzlePiece.transform.parent;
            }
            
            _selfJoint.Connect();
            _otherJoint.Connect();
            
            _selfJoint = null;
            _otherJoint = null;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Convert mouse screen position to world position
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -_mainCamera.transform.position.z; // Ensure the z-position is consistent
        return _mainCamera.ScreenToWorldPoint(mousePoint);
    }
}
