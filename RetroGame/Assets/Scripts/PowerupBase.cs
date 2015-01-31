using UnityEngine;
using System.Collections;

public abstract class PowerupBase : Enemy
{	
	public delegate void PowerUpDelegate(Player player);

	public abstract void OnActivate(Player player);
	public abstract void OnDeActivate(Player player);
	
	public virtual void DoPowerUp(Player player)
	{
		
	}
	
	protected override void Start()
	{
		// Mushroom powerup always goes to right first.
		m_movementDirection = Enemy.MovementDirection.Right;
	}	
	
	protected override bool OnPlayerHit (Collision2D collision)
	{
		Player.CurrentPlayer.SetPowerUp(this);
		Die();
		
		return true;
	}
}
