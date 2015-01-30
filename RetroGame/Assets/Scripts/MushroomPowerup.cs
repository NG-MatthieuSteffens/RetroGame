using UnityEngine;
using System.Collections;

public class MushroomPowerup : Enemy 
{
	private void Start()
	{
		// Mushroom powerup always goes to right first.
		m_movementDirection = Enemy.MovementDirection.Right;
	}
	
	protected override bool OnPlayerHit (Collision2D collision)
	{
		Player.CurrentPlayer.Grow( true );
		Die();
		
		return true;
	}
}
