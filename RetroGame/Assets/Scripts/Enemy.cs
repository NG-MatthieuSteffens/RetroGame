using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	private enum MovementDirection
	{
		Left = -1,
		Right = 1
	}
	
	[SerializeField]
	private float m_movementSpeed;
	
	[SerializeField]
	private MovementDirection m_movementDirection;
	
	private new Transform transform;
	private new Rigidbody2D rigidbody2D;
	
	public void Die()
	{
		Destroy( gameObject );
	}
	
	private void Awake()
	{
		transform = GetComponent<Transform>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate()
	{
		int direction = (int)m_movementDirection;
		transform.Translate( Vector3.right * m_movementSpeed * direction * Time.deltaTime );
		
		if( transform.position.y < CameraController.CameraBounds.min.y )
		{
			Die();
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Vector2 collisionDirection = collision.contacts[0].point - (Vector2)transform.position;	

		if( collision.transform.CompareTag("Player") )
		{
			if( collision.relativeVelocity.y > 0 )
			{
				Die();
			}
			else
			{
				Player.CurrentPlayer.GameOver();
			}
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
