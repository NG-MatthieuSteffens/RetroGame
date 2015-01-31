using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour 
{
	protected new Transform transform;
	protected new Rigidbody2D rigidbody2D;
	
	public bool Die()
	{
		Destroy( gameObject );
		return false;
	}
	
	protected virtual bool OnPlayerHit(Collision2D collision)
	{
		GameManager.currentPlayer.TakeDamage();
		return true;
	}
	
	protected bool HandlePlayerCollision(Collision2D collision)
	{
		if( collision.transform.CompareTag("Player") )
		{
			return OnPlayerHit(collision);
		}
		
		return false;
	}
	
	protected virtual void Awake()
	{
		transform = GetComponent<Transform>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	protected virtual void Start()
	{
	
	}
	
	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		HandlePlayerCollision(collision);
	}
}