using UnityEngine;
using System.Collections;

public class ItemBlock : MonoBehaviour 
{
	[SerializeField]
	private Material m_hitted;
	private bool m_isHitted;
	
	[SerializeField]
	private GameObject m_blockItem;
	
	[SerializeField]
	private int m_maxItemDrops = 1;
	private int m_totalItemDrops;

	public void OnHit()
	{
		if( m_isHitted )
		{ 
			return;
		}
		
		m_totalItemDrops++;
		if( m_totalItemDrops >= m_maxItemDrops )
		{
			m_isHitted = true;
			renderer.sharedMaterial = m_hitted;
		}
		
		if( m_blockItem )
		{
			GameObject.Instantiate( m_blockItem, transform.position + Vector3.up, Quaternion.identity );
		}

	}
}
