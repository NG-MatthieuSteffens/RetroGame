using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour   
{
	public void Load(int level)
	{
		Application.LoadLevel( level );
	}

	public void Continue()
	{
		Application.LoadLevel( Application.loadedLevel );
	}

	public void Exit()
	{
		Application.Quit();
	}
}
