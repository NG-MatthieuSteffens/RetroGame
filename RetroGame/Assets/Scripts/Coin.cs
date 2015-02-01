using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour 
{
	[SerializeField]
	private float m_destroyDelay = 1;
	
	[SerializeField]
	private int m_score = 200;
	private IEnumerator Start () 
	{
		yield return new WaitForSeconds( m_destroyDelay );
		GameManager.AddCoin();
		GameManager.AddScore( m_score );
		Destroy(gameObject);
	}
}
