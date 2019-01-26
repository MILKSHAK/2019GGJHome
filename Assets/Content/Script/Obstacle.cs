using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour, IDamagable
{
	public float health;

	public float dropRange = 1;

	public float speed = -1;

	[Serializable]
	public class ObstacleDrop
	{
		public Transform dropPrefab;
		public int number;
	}

	[SerializeField]
	private ObstacleDrop[] obstacleDrops;

	private void Update()
	{
		transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Die();
		}
		return;
	}

	public void Die()
	{
		EventBus.Post<EventType>(EventType.ObstacleDestroy);
		foreach (ObstacleDrop drop in obstacleDrops)
		{
			for (int i = 0; i < drop.number; i++)
			{
				Instantiate(drop.dropPrefab, GetRandomPos(), UnityEngine.Random.rotation);
			}
		}
		return;
	}

	public Vector3 GetRandomPos()
	{
		Vector3 randPos = new Vector3(UnityEngine.Random.Range(-dropRange, dropRange), UnityEngine.Random.Range(-dropRange, dropRange));
		return randPos;
	}
}
