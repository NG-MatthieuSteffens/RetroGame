using UnityEngine;
using System.Collections;

public class StarPowerup : PowerupBase 
{

	[SerializeField]
	protected float m_duration;

	// Star powerup doesn't get OnActivate or OndeActive calls, because it isn't set to player main powerup!
	public override void OnActivate (Player player)
	{
		
	}
	
	public override void OnDeActivate (Player player)
	{
		
	}

	protected override bool OnPlayerHit (Collision2D collision)
	{
		AddPowerUp( this, GameManager.currentPlayer );
		Die();
		
		return true;
	}
	
	protected override void Clone (PowerupBase clone)
	{
		StarPowerup newClone = clone as StarPowerup;
		m_duration = newClone.m_duration;
	}
	
	protected new IEnumerator Start ()
	{
		base.Start();
		
		if( m_player )
		{
			m_player.StartCoroutine( m_player.WaitInvulnerabilityTime( m_duration ) );
			
			yield return new WaitForSeconds( m_duration );
			
			Destroy( this );
		}
	}
	
	protected override void OnCollisionEnter2D (Collision2D collision)
	{
		if( m_player )
		{
			Enemy enemy = collision.transform.GetComponent<Enemy>();
			
			if( enemy && enemy.CompareTag("Enemy") )
			{
				enemy.Die();
			}
		}
		else
		{
			base.OnCollisionEnter2D(collision);
		}
	}
}
