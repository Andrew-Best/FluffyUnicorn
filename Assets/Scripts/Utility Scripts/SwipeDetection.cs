using UnityEngine;
using System.Collections;

public class SwipeDetection : MonoBehaviour 
{
	public float minSwipeDistY;
    public float minSwipeDistX;
	private Vector2 startPos;

	void Update()
	{
		if (Input.touchCount > 0) 
		{
			Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Ended:
                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;

                    if (swipeDistVertical > minSwipeDistY)
                    {
                        float swipeValue = Mathf.Sign(touch.position.y - startPos.y);

                        //User swiped up
                        if (swipeValue > 0)
                        {
                        }
                        //User swiped down
                        else if (swipeValue < 0)
                        {
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeDistHorizontal > minSwipeDistX)
                    {
                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);

                        //User swiped right
                        if (swipeValue > 0)
                        {
                        }
                        //User swiped left
                        else if (swipeValue < 0)
                        {
                        }
                    }
                    break;
            }
		}
	}
}