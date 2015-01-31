using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
	[SerializeField]
	private Material m_hitted;
	private bool m_isHitted;
	
	[SerializeField]
	private GameObject m_blockItem;

	public void OnHit()
	{
		if( m_isHitted )
		{ 
			return;
		}
		
	
		m_isHitted = true;
		
		if( m_blockItem )
		{
			renderer.sharedMaterial = m_hitted;
			GameObject.Instantiate( m_blockItem, transform.position + Vector3.one, Quaternion.identity );
		}
		else
		{
			// If blocks doesn't have item, block will be destroyed instead.
			Destroy(gameObject);
		}
	}
}
