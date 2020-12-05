using System;
using UnityEngine;

namespace SuperBricks
{
    public class AndroidMinoInput:MonoBehaviour, IMinoInput
    {
        [SerializeField]
        private float _minDistanceForSwipe;

        [SerializeField]
        private int _actionsPerSwipe;


        private int _actionCount;
        private Vector2 _pointA;
        private Vector2 _pointB;
        private bool _isFirstMove;
        
        

        public ActionData DetectAction()
        {
            
            ActionData actionData = new ActionData();
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _pointA = touch.position;
                    _pointB = touch.position;
                    _isFirstMove = true;
                    _actionCount = 0;
                    
                    
                    
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    _pointB = touch.position;
                    if(IsHorizontalSwipe())
                    {
                        
                        if (_isFirstMove || _actionCount > _actionsPerSwipe)
                        {
                            actionData.isActionHappened = true;

                            float swipeDirection = _pointB.x - _pointA.x;
                            if (swipeDirection > 0)
                            {
                                actionData.action = MoveAction.Right;
                            }
                            else
                            {
                                actionData.action = MoveAction.Left;
                            }

                            _isFirstMove = false;
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
                    else if (_pointB == _pointA)
                    {
                        actionData.isActionHappened = true;
                        actionData.action = MoveAction.Rotate;
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

       


    }
}