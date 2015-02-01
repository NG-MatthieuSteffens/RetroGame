using UnityEngine;
using System.Collections;

public class StarPowerup : PowerupBase 
{

	[SerializeField]
	protected float m_duration;
	
	[SerializeField]
	protected float m_bouncePower = 5;
	
	/// <summary>
	/// Star uses different colors, so it would broke colors between fireflower and normal.
	/// </summary>
	[SerializeField]
	protected Material m_starMaterial;
	protected Material m_oldMaterial;

	// Star powerup doesn't get OnActivate or OndeActive calls, because it isn't set to player main powerup!
	public override void OnActivate (Player player)
	{
		m_oldMaterial = player.renderer.material;
		player.renderer.sharedMaterial = m_starMaterial;
	}
	
	public override void OnDeActivate (Player player)
	{
		player.renderer.material = m_oldMaterial;
	}

	protected override void PowerUpEffect (Player player)
	{
		// Resets colors if player material is changed.
		if( player.renderer.sharedMaterial != m_starMaterial )
		{
			OnActivate( player );
		}
		
		if( Time.time % 0.5f == 0 )
		{
			m_starMaterial.color = m_starMaterial.color == Color.white ?  Color.yellow :  Color.white;
		}
	}

	protected override bool OnPlayerHit (Collision2D collision)
	{
		AddPowerUp( this, GameManager.currentPlayer );
		Die();
		
		return true;
	}
	
	protected override void Clone (PowerupBase clone)
	{
		base.Clone( clone );
	
		StarPowerup newClone = clone as StarPowerup;
		m_starMaterial = newClone.m_starMaterial;
		m_duration = newClone.m_duration;
	}
	
	protected new IEnumerator Start ()
	{
		base.Start();
		
		if( m_player )
		{
			OnActivate(m_player);
			m_player.StartCoroutine( m_player.WaitInvulnerabilityTime( m_duration ) );
			
			yield return new WaitForSeconds( m_duration );
			
			OnDeActivate(m_player);
			Destroy( this );
		}
		else
		{
			yield return new WaitForSeconds( 5f );
			Destroy(gameObject);
		}
	}
	
	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
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
			
			rigidbody2D.AddForce( Vector2.up * m_bouncePower, ForceMode2D.Impulse );
		}
	}
}
