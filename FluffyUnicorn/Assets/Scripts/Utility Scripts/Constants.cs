using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour
{
    #region Farts and Beans
    public const float FART_SLIDER_MIN = 0.0f;          //Minumum the fart slider can be at
    public const float FART_SLIDER_MAX = 100.0f;        //Maximum the fart slider can be at
    public const float BEAN_VALUE = 20.0f;              //Value of the beans to increment the fart slider on bean pick up
    #endregion

    #region Level Constants
    public const int BULLY_LEVEL_REQUIREMENT = 5;       //Base requirement of bullies defeated to successfully pass the first level
                                                        //This will increase with the level, adding X bullies for the level number (level 1 = +1 bully, level 2 = +2 bullies, etc)
    #endregion

    #region Player Constants
    public const int PLAYER_MIN_HEALTH = 0;             //Lowest health the player can have
    public const int PLAYER_DEFAULT_MAX_HEALTH = 3;     //Default max health for the player
    #endregion

    #region bully variables
    public const float BULLY_ATTACK_TIMER_RESET_VALUE = 5;
	public const int BULLY_VEL_X = -2;

	public const int BULLY_HP = 3;
	public const int BULLY_PUNCH_DAMAGE = 1;
	public const int BULLY_KICK_DAMAGE = 2;
	public const int BULLY_UNIQUE_ATTACK_DAMAGE = 3;

	public const int BULLY_PUNCH_ODDS = 60;
	public const int BULLY_KICK_ODDS = 80;
	public const int BULLY_UNIQUE_ATK_ODDS = 100;

	public const float BULLY_KICK_RESTTIME = 2;
	public const float BULLY_PUNCH_RESTTIME = 1;
	public const float BULLY_UNIQUE_ATK_RESTTIME = 3;

	public const float BULLY_MAX_TRAVEL_DIST = 3;
	#endregion
}