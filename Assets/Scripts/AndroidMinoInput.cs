using System;
using UnityEngine;

namespace SuperBricks
{
    public class AndroidMinoInput:MonoBehaviour, IMinoInput
    {
        [SerializeField]
        private float _minDistanceForSwipe;

        
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
                    _pointA = touch.position;
                    _pointB = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    _pointB = touch.position;
                    DetectSwipe(actionData);
                }
            }

            return actionData;
        }

        private void DetectSwipe(ActionData actionData)
        {
            if (IsSwipe())
            {
                actionData.isActionHappened = true;
                MoveAction action = MoveAction.Rotate;
                if (IsVerticalSwipe())
                {
                    float swipeDirection = _pointB.y - _pointA.y ;
                    if (swipeDirection > 0)
                    {
                        actionData.action = MoveAction.Rotate;
                    }
                    else
                    {
                        actionData.action = MoveAction.ToBottomEnd;
                    }

                }
                else
                {
                    float swipeDirection = _pointB.x - _pointA.x ;
                    if (swipeDirection > 0)
                    {
                        actionData.action = MoveAction.Right;
                    }
                    else
                    {
                        actionData.action = MoveAction.Left;
                    }
                }
            }
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


    }
}