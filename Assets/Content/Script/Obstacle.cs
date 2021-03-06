﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// [RequireComponent(typeof(Collider2D))]
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Die();
		}
		return;
	}

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
			Drop();
            Die();
        }
        return;
    }

	public void Drop()
	{
		if (!canDestroy)
		{
			return;
		}
		EventBus.Post<EnumEventType>(EnumEventType.ObstacleDestroy);
		foreach (ObstacleDrop drop in obstacleDrops)
		{
			for (int i = 0; i < drop.number; i++)
			{
				Vector3 spwanPos = GetRandomPos();
				Instantiate(drop.dropPrefab, GetRandomPos(), Quaternion.Euler(0, 0, 0));
			}
		}
	}

	public void Die()
    {
        if (!canDestroy)
        {
            return;
        }
        died = true;
		Destroy(gameObject);
		return;
    }

    public Vector3 GetRandomPos()
    {
        Vector3 randPos = transform.position + new Vector3(UnityEngine.Random.Range(-dropRange, dropRange), UnityEngine.Random.Range(-dropRange, dropRange));
        return randPos;
    }
}
