using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Joint : MonoBehaviour
{
    public PuzzlePiece puzzlePiece;
    public float magneticRadius = 0.5f;
    public Color normal, highlighted, connected;
    public bool isConnected;
    
    private SpriteRenderer _spriteRenderer;
    private List<Joint> _closeJointList;
    private Joint _targetJoint;

    public Joint GetTargetJoint()
    {
        return _targetJoint;
    }
    
    public void Connect()
    {
        _spriteRenderer.color = connected;
        isConnected = true;
    }
    
    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    private void OnValidate()
    {
        GetComponent<CircleCollider2D>().radius = magneticRadius;
    }

    private void Awake()
    {
        gameObject.tag = "Joint";
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _closeJointList = new List<Joint>();
        
        _spriteRenderer.color = normal;
    }

    private void Update()
    {
        if (isConnected) return;
        bool pieceOfSameParentIsBeingDragged = false;
        for (int i = 0; i < puzzlePiece.transform.parent.childCount; i++)
        {
            var p = puzzlePiece.transform.parent.GetChild(i);
            if (p.GetComponent<PuzzlePiece>().isBeingDragged)
            {
                pieceOfSameParentIsBeingDragged = true;
                break;
            }
        }
        if (!pieceOfSameParentIsBeingDragged) return;

        for (var i = _closeJointList.Count - 1; i >= 0; i--)
        {
            if (IsSameParent(_closeJointList[i]))
                _closeJointList.RemoveAt(i);
        }

        if (_closeJointList.Count == 0)
        {
            _spriteRenderer.color = normal;
            
            _targetJoint = null;
        }
        else
        {
            // get closest joint in list
            float minSqrDist = float.MaxValue;
            Joint closestJoint = _closeJointList[0];
            foreach (Joint joint in _closeJointList)
            {
                float sqrDist = Vector3.SqrMagnitude(transform.position - joint.transform.position);
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    closestJoint = joint;
                }
            }
            
            SetColor(highlighted);
            closestJoint.SetColor(highlighted);
            
            _targetJoint = closestJoint;
        }
    }

    private bool IsSameParent(Joint other)
    {
        return other.puzzlePiece.transform.parent == puzzlePiece.transform.parent;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isConnected) return;
        
        if (other.CompareTag("Joint"))
        {
            Joint otherJoint = other.GetComponent<Joint>();

            if (IsSameParent(otherJoint)) 
                return;
            
            if (otherJoint.isConnected) 
                return;

            _closeJointList.Add(otherJoint);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (isConnected) return;
        
        if (other.CompareTag("Joint"))
        {
            Joint otherJoint = other.GetComponent<Joint>();
            
            if (otherJoint.isConnected) 
                return;
            otherJoint.SetColor(otherJoint.normal);
            
            if (_closeJointList.Contains(otherJoint))
                _closeJointList.Remove(otherJoint);
        }
    }
}