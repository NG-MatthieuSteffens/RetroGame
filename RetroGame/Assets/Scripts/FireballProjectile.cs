using UnityEngine;
using System.Collections;

public class FireballProjectile : MonoBehaviour 
{	
	[SerializeField]
	private int m_maxBounces = 3;
	
	private int m_totalBounces;

	private Vector2 m_velocity;
	private float m_time;
	
	private new Transform transform;
	private new Rigidbody2D rigidbody2D;
	
	public void SetVelocity( Vector2 velocity )
	{
		m_velocity = velocity;
		transform.Translate( m_velocity * Time.deltaTime, Space.Self );
	}
	
	private void Awake()
	{
		transform = GetComponent<Transform>();
		rigidbody2D = GetComponent<Rigidbody2D>();	
	}
	 
	private void FixedUpdate()
	{	
		float bounce = Mathf.Abs( Mathf.Sin( m_time) ) * 0.5f + 1.0f; 
		transform.Translate( m_velocity.x * Time.fixedDeltaTime, m_velocity.y * bounce * Time.fixedDeltaTime, 0 , Space.Self );
		m_time += Time.fixedDeltaTime;
		
		// For sure...
		if( transform.position.y < -20 )
		{
			Destroy(gameObject);
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		Enemy enemy = collision.transform.GetComponent<Enemy>();
		
		if( enemy && !collision.transform.CompareTag("Player") )
		{
			enemy.Die();
			Destroy(gameObject);
			return;
		}
		
		m_totalBounces++;
		
		if( m_totalBounces >= m_maxBounces || (collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x < 0 && collision.contacts[0].normal.y == 0) )
		{
			Destroy(gameObject);
			return;
		}

	}
}
