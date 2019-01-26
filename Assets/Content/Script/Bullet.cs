using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
	public float damage;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Obstacle")
		{
			IDamagable damageTaker = other.gameObject.GetComponent<IDamagable>();
			damageTaker.TakeDamage(damage);
		}
	}
}
