using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour
{
	public StateScript m_StartState;
	private StateScript curState_;

	void Start()
	{
		if (m_StartState == null)
		{
			Debug.LogError("Each state machine must have a start state set via the inspector view.");
		}
		curState_ = m_StartState;
		curState_.OnStateEntered();
	}

	void Update()
	{
		curState_.StateUpdate(); // this could be in fixed update, also we might
		// want a fixed update specific function for our state.
	}

	void OnGUI()
	{
		curState_.StateGUI();
	}

	public void ChangeState(StateScript newState)
	{
//		curState_.OnStateExit();
		curState_ = newState;
		curState_.OnStateEntered();
	}

}
