using UnityEngine;
using System.Collections;

public class MushroomPowerup : PowerupBase 
{
	public override void OnActivate (Player player)
	{
		player.Grow( true );
	}
	
	public override void OnDeActivate (Player player)
	{
		player.Grow( false );
	}
	
}
