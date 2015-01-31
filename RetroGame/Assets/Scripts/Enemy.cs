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
		Vector2 collisionDirection = collision.contacts[0].point - (Vector2)transform.position;	

		// Ignore other collisions if player dies.
		if( HandlePlayerCollision(collision) )
		{
			return;
		}

		if( collision.relativeVelocity.y < 0 || collisionDirection.y > 0.5f )
		{
			return;
		}
		
		if( m_movementDirection.Equals( MovementDirection.Right ) && collisionDirection.x > 0 )
		{
			m_movementDirection = MovementDirection.Left;
		}
		else if( m_movementDirection.Equals( MovementDirection.Left ) && collisionDirection.x < 0 )
		{
			m_movementDirection = MovementDirection.Right;
		}
	}
}
