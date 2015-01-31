using UnityEngine;
using System.Collections;

public class FireFlowerPowerup : MushroomPowerup 
{
	[SerializeField]
	protected GameObject m_projectile;
	
	[SerializeField]
	protected Vector3 m_offset = Vector3.right;
	
	[SerializeField]
	protected Vector2 m_velocity;
	
	public override void OnActivate (Player player)
	{
		base.OnActivate (player);
	}
	
	public override void OnDeActivate (Player player)
	{
		base.OnDeActivate (player);
		player.SetPowerUp( null, typeof( MushroomPowerup ) );
	}

	public override void DoPowerUp (Player player)
	{
		FireballProjectile projectile = (GameObject.Instantiate( m_projectile, player.transform.position + (m_offset + Vector3.right * player.rigidbody2D.velocity.x) * player.PlayerDirection, Quaternion.identity ) as GameObject).GetComponent<FireballProjectile>();
		projectile.SetVelocity( new Vector2( (m_velocity.x + player.rigidbody2D.velocity.x) * player.PlayerDirection, m_velocity.y) );
	}
	
	protected override void Clone (PowerupBase clone)
	{
		FireFlowerPowerup newClone = clone as FireFlowerPowerup;
		m_projectile = newClone.m_projectile;
		m_velocity = newClone.m_velocity;
		m_offset = newClone.m_offset;
	}
}
