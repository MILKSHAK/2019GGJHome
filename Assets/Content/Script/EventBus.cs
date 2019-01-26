using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeathReason
{
	Collision,
	Burn,
}

public enum EnumEventType
{
	PickupEnergy,
	HitObstacleSmall,
	HitObstacleBig,
	PlayerDestroy,
	ObstacleDestroy,
}

public static class EventBus {

	public delegate void EventHandler<T>(T ev);

	static readonly Dictionary<Type, List<object>> eventHandlers = new Dictionary<Type, List<object>>();

	public static void Subscribe<T>(EventHandler<T> handler) {
		var type = typeof(T);
		if (!eventHandlers.ContainsKey(type)) {
			eventHandlers.Add(type, new List<object>());
		}

		eventHandlers[type].Add(handler);
	}

	public static void Unsubscribe<T>(EventHandler<T> handler) {
		var type = typeof(T);
		if (eventHandlers.ContainsKey(type)) {
			eventHandlers[type].Remove(handler);
		}
	}

	public static void Post<T>(T ev) {
		List<object> list;
		eventHandlers.TryGetValue(typeof(T), out list);
		if (list != null && list.Count > 0) {
			foreach (var _handler in list) {
				((EventHandler<T>) _handler) (ev);
			}
		} else {
			Debug.LogWarning(ev + " Has no receiver");
		}
	}

}
