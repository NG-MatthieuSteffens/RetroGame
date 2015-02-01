using UnityEngine;
using System.Collections;

public abstract class PowerupBase : Enemy
{	
	public static PowerupBase AddPowerUp(PowerupBase power, Player player)
	{
		PowerupBase playerPowerUp = AddPowerUp( power.GetType(), player );
		playerPowerUp.Clone(power);
		return playerPowerUp;
	}
	
	public static PowerupBase AddPowerUp(System.Type type, Player player)
	{
		PowerupBase playerPowerUp = player.gameObject.AddComponent( type ) as PowerupBase;
		playerPowerUp.m_player = player;
		return playerPowerUp;
	}

	public delegate void PowerUpDelegate(Player player);
	public abstract void OnActivate(Player player);
	public abstract void OnDeActivate(Player player);
	
	[SerializeField]
	protected Player m_player;
	
	public PowerupBase SetToPlayer(Player player)
	{
		return AddPowerUp(this,player);
	}
	
	public virtual void DoPowerUp(Player player)
	{
		
	}
	
	protected virtual void PowerUpEffect(Player player)
	{
		
	}

	protected virtual void Clone(PowerupBase clone)
	{
		m_score = clone.m_score;
	}

	protected override void Start()
	{
		// Mushroom powerup always goes to right first.
		m_movementDirection = Enemy.MovementDirection.Right;
	}	
	
	protected override void FixedUpdate ()
	{
		if( m_player )
		{
			PowerUpEffect( m_player );
		}
		else
		{
			base.FixedUpdate();
		}
	}
	
	protected override bool OnPlayerHit (Collision2D collision)
	{
		if( GameManager.currentPlayer.CurrentPowerup == null || GameManager.currentPlayer.CurrentPowerup.GetType() != this.GetType() )
		{
			GameManager.currentPlayer.SetPowerUp( this );
		}
		Die();
		
		return true;
	}
	
	protected override void OnCollisionEnter2D (Collision2D collision)
	{
		if( !m_player )
		{
			base.OnCollisionEnter2D (collision);
		}
	}
}
