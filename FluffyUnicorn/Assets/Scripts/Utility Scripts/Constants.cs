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

    public const float ENEMY_SPAWN_TIMER_MAX = 4.5f;
    public const float TRACK_COUNTDOWN_DEFAULT = 2;
    public const float BULLY_ATTACK_TIMER_RESET_VALUE = 5;
    public const int BULLY_VEL_X = -2;
    public const int MOVE_LEFT = -1; //Not yet implemented
    public const int MOVE_RIGHT = 1; //Not yet implemented

	public const float BULLY_UNIQUE_ATK_LENGTH = 4;
	public const float BLING_UNIQUE_ATK_LENGTH = 4;
	public const float PEPPER_UNIQUE_ATK_LENGTH = 4;//likely not even used
	public const float FAT_UNIQUE_ATK_LENGTH = 8;
	public const float JOCK_UNIQUE_ATK_LENGTH = 4;
	public const float DEFAULT_PEPPER_SPEED = 5;


    public const float BULLY_MAX_TRAVEL_DIST = 3;
    #endregion

	#region bosses
	public const float FATTEST_BULLY_HP = 25;
	public const float FATTEST_BULLY_JUMP_FORCE = 15;
	public const float FATTEST_BULLY_ROLL_SPEED = 20;
	public const float MAX_FATTEST_HEIGHT = 100.0f;
	public const float FATTEST_BULLY_JUMP_TIMER = 2;

	public const float KING_BULLY_HP = 10;
	public const float KB_WATER_GUN_LENGTH = 4;
	public const int KB_WATER_AMMO = 99;

	public const float QUEEN_BULLY_HP = 25;
	public const float DEFAULT_TIME_UNTIL_THROW = 20;
	public const float DEAD_FISH_HP = 3;
	public const float POP_CAN_HP = 5;
	public const float BURNT_TOAST_HP = 1;

	public const float HORDE_CHARGE_LEFT_SPEED = -7.5f;
	public const float HORDE_CHARGE_RIGHT_SPEED = 7.5f;
	public const float HORDE_CHARGE_UP_SPEED = 7.5f; 
	public const float HORDE_CHARGE_DOWN_SPEED = 7.5f;
	public const int JOCK_INDEX = 4;





	#endregion

    #region Upgrades
    public const int MAX_PLAYER_HEALTH = 5;
    public const int MAX_PLAYER_DAMAGE = 3;
    public const int MAX_PLAYER_CURRENCY = 3;

    public const float MAX_PLAYER_Attack_Rate = 0.1f;
    public const float MAX_PLAYER_SPEED = 13.0f;
    #endregion
}