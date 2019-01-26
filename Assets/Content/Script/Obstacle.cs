using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class Obstacle : MonoBehaviour, IDamagable
{
	public float health;

	public float dropRange = 10;

	public float speed = -1;

	public bool canDestroy = false;

	private bool died = false;

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
		if (!canDestroy)
		{
			return;
		}
		health -= damage;
		if (health <= 0 && !died)
		{
			Die();
		}
		return;
	}

	public void Die()
	{
		if (!canDestroy)
		{
			return;
		}
		died = true;
		EventBus.Post<EventType>(EventType.ObstacleDestroy);
		foreach (ObstacleDrop drop in obstacleDrops)
		{
			for (int i = 0; i < drop.number; i++)
			{
				Vector3 spwanPos = GetRandomPos();
				Instantiate(drop.dropPrefab, GetRandomPos(), Quaternion.Euler(0, 0, 0));
			}
			Destroy(gameObject);
		}
		return;
	}

	public Vector3 GetRandomPos()
	{
		Vector3 randPos = transform.position + new Vector3(UnityEngine.Random.Range(-dropRange, dropRange), UnityEngine.Random.Range(-dropRange, dropRange));
		return randPos;
	}
}
