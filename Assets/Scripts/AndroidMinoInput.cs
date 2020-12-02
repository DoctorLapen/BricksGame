using System;
using UnityEngine;

namespace SuperBricks
{
    public class AndroidMinoInput:MonoBehaviour, IMinoInput
    {
        [SerializeField]
        private float _minDistanceForSwipe;

        [SerializeField]
        private int _actionsPerSecond;


        private int _actionCount;
        private Vector2 _pointA;
        private Vector2 _pointB;
        
        

        public ActionData GetNextAction()
        {
            ActionData actionData = new ActionData();
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (IsSwipe())
                    {
                        _pointA = touch.position;
                        _pointB = touch.position;
                        _actionCount = 0;
                    }
                    else
                    {
                        actionData.isActionHappened = true;
                        actionData.action = MoveAction.Rotate;
                    }
                    
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _pointB = touch.position;
                    if(IsHorizontalSwipe()  && _actionCount < _actionsPerSecond)
                    {
                        actionData.isActionHappened = true;
                        actionData.moveDistance = CalculateRealHorizontalMovementDistance();
                        
                        float swipeDirection = _pointB.x - _pointA.x;
                        if (swipeDirection > 0)
                        {
                            actionData.action = MoveAction.Right;
                        }
                        else
                        {
                            actionData.action = MoveAction.Left;
                        }

                        _actionCount++;
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (IsToBottomSwipe())
                    {
                        actionData.isActionHappened = true;
                        actionData.action = MoveAction.ToBottomEnd;
                    }
                    
                   
                }
            }

            return actionData;
        }

        private bool IsHorizontalSwipe()
        {
            if (IsSwipe())
            {
                
                if (!IsVerticalSwipe())
                {

                    return true;
                }
                
            }

            return false;
        }

        private bool IsToBottomSwipe()
        {
            if (IsSwipe())
            {
                if (IsVerticalSwipe())
                {
                    float swipeDirection = _pointB.y - _pointA.y;
                    if (swipeDirection < 0)
                    {
                        return true;
                    }
                    
                }
            }

            return false;
        }

        private bool IsSwipe()
        {
            return HorizontalMovementDistance() > _minDistanceForSwipe || VerticalMovementDistance() > _minDistanceForSwipe;
        }

        private bool IsVerticalSwipe()
        {
            return VerticalMovementDistance() > HorizontalMovementDistance();
        }

        private float HorizontalMovementDistance()
        {
            return Mathf.Abs(_pointB.x - _pointA.x);
        }
        private float VerticalMovementDistance() 
        {
            return Mathf.Abs(_pointB.y - _pointA.y);
        }

        private float CalculateRealHorizontalMovementDistance()
        {
            Vector3 realPointA = Camera.main.ScreenToWorldPoint(new Vector3(_pointA.x,_pointA.y,-10f));
            Vector3 realPointB = Camera.main.ScreenToWorldPoint(new Vector3(_pointB.x,_pointB.y,-10f));
            return Mathf.Abs(realPointB.x - realPointA.x);

        }


    }
}