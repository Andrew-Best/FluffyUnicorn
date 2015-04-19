using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    public Button m_Start;
    public Button m_Quit;

   public void StartGame()
    {
        Application.LoadLevel("Main");
    }

    public void Quit()
    {
		
    }
}
