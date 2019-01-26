using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
	public float damage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Obstacle")
		{
			IDamagable damageTaker = collision.gameObject.GetComponent<IDamagable>();
			damageTaker.TakeDamage(damage);
		}
	}
}
