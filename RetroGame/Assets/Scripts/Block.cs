using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
	[SerializeField]
	private Material m_hitted;
	private bool m_isHitted;
	
	[SerializeField]
	private GameObject m_blockItem;

	private void OnHit()
	{
		m_isHitted = true;
		renderer.sharedMaterial = m_hitted;
		
		if( m_blockItem )
		{
			GameObject.Instantiate( m_blockItem, transform.position + Vector3.one, Quaternion.identity );
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if( m_isHitted )
		{ 
			return;
		}
	
		Vector2 velocity = collision.relativeVelocity;
		
		if( collision.transform.CompareTag("Player") && velocity.x == 0 && velocity.y < 0)
		{
			OnHit();
		}
	}
}
