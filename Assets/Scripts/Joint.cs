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
        puzzlePiece = GetComponentInParent<PuzzlePiece>();
        _closeJointList = new List<Joint>();
        
        _spriteRenderer.color = normal;
    }

    private void Update()
    {
        if (isConnected) return;
        if (!puzzlePiece.isBeingDragged) return;
        
        if (_closeJointList.Count == 0)
        {
            _spriteRenderer.color = normal;
            puzzlePiece.ClearConnectTarget();
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
            
            puzzlePiece.SetConnectTarget(this, closestJoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isConnected) return;
        
        if (other.CompareTag("Joint"))
        {
            Joint otherJoint = other.GetComponent<Joint>();

            if (otherJoint.puzzlePiece == puzzlePiece) 
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
            
            if (otherJoint.puzzlePiece == puzzlePiece) 
                return;
            
            if (otherJoint.isConnected)
                return;
            else
                otherJoint.SetColor(normal);
            
            _closeJointList.Remove(otherJoint);
        }
    }
}