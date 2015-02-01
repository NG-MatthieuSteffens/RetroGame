using UnityEngine;
using System.Collections;

public class Enemy : EnemyBase 
{
	protected enum MovementDirection
	{
		Left = -1,
		Right = 1
	}
	
	[SerializeField]
	protected float m_movementSpeed;
	
	[SerializeField]
	protected MovementDirection m_movementDirection;
	
	protected virtual void FixedUpdate()
	{
		if( !GameManager.cameraBounds.Contains( transform.position ) && !GameManager.cameraBounds.Contains( transform.position - Vector3.right * GameManager.cameraBounds.extents.x ))
		{
			return;
		}
		
		int direction = (int)m_movementDirection;
		transform.Translate( Vector3.right * m_movementSpeed * direction * Time.deltaTime );
		
		if( transform.position.y < GameManager.cameraBounds.min.y )
		{
			Die();
		}
	}
	
	protected override bool OnPlayerHit (Collision2D collision)
	{
		if( collision.relativeVelocity.y > 0 )
		{
			return Die();
		}
		else
		{
			return base.OnPlayerHit (collision);
		}
	}
	
	protected override void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 collisionDirection = collision.contacts[0].normal;

		// Ignore other collisions if player dies.
		if( HandlePlayerCollision(collision) )
		{
			return;
		}

		if( collisionDirection.y != 0 )
		{
			return;
		}
		
		if( m_movementDirection.Equals( MovementDirection.Right ) && collisionDirection.x < 0 )
		{
			m_movementDirection = MovementDirection.Left;
		}
		else if( m_movementDirection.Equals( MovementDirection.Left ) && collisionDirection.x > 0 )
		{
			m_movementDirection = MovementDirection.Right;
		}
	}
}
