/*
* Copyright 2017 Ben D'Angelo
*
* MIT License
*
* Permission is hereby granted, free of charge, to any person obtaining a copy of this
* software and associated documentation files (the "Software"), to deal in the Software
* without restriction, including without limitation the rights to use, copy, modify, merge,
* publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
* to whom the Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all copies or
* substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
* INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
* PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
* FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
* OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
	public bool limitQueueProcessing = false;
	public float queueProcessTime = 0.0f;
	private static EventManager _sInstance = null;
	private Queue _eventQueue = new Queue();

	public delegate void EventDelegate<T>(T e) where T : GameEvent;
	private delegate void EventDelegate(GameEvent e);

	private Dictionary<System.Type, EventDelegate> _delegates = new Dictionary<System.Type, EventDelegate>();
	private Dictionary<System.Delegate, EventDelegate> _delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
	private Dictionary<System.Delegate, System.Delegate> _onceLookups = new Dictionary<System.Delegate, System.Delegate>();

	// override so we don't have the typecast the object
	public static EventManager Instance
	{
		get
		{
			if (_sInstance == null)
			{
				_sInstance = GameObject.FindObjectOfType(typeof(EventManager)) as EventManager;
			}
			return _sInstance;
		}
	}

	private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
	{
		// Early-out if we've already registered this delegate
		if (_delegateLookup.ContainsKey(del))
			return null;

		// Create a new non-generic delegate which calls our generic one.
		// This is the delegate we actually invoke.
		EventDelegate internalDelegate = (e) => del((T)e);
		_delegateLookup[del] = internalDelegate;

		EventDelegate tempDel;
		if (_delegates.TryGetValue(typeof(T), out tempDel))
		{
			_delegates[typeof(T)] = tempDel += internalDelegate;
		}
		else
		{
			_delegates[typeof(T)] = internalDelegate;
		}

		return internalDelegate;
	}

	public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
	{
		AddDelegate<T>(del);
	}

	public void AddListenerOnce<T>(EventDelegate<T> del) where T : GameEvent
	{
		EventDelegate result = AddDelegate<T>(del);

		if (result != null)
		{
			// remember this is only called once
			_onceLookups[result] = del;
		}
	}

	public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
	{
		EventDelegate internalDelegate;
		if (_delegateLookup.TryGetValue(del, out internalDelegate))
		{
			EventDelegate tempDel;
			if (_delegates.TryGetValue(typeof(T), out tempDel))
			{
				tempDel -= internalDelegate;
				if (tempDel == null)
				{
					_delegates.Remove(typeof(T));
				}
				else
				{
					_delegates[typeof(T)] = tempDel;
				}
			}

			_delegateLookup.Remove(del);
		}
	}

	public void RemoveAll()
	{
		_delegates.Clear();
		_delegateLookup.Clear();
		_onceLookups.Clear();
	}

	public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent
	{
		return _delegateLookup.ContainsKey(del);
	}

	public void TriggerEvent(GameEvent e)
	{
		EventDelegate del;
		if (_delegates.TryGetValue(e.GetType(), out del))
		{
			del.Invoke(e);

			// remove listeners which should only be called once
			foreach (EventDelegate k in _delegates[e.GetType()].GetInvocationList())
			{
				if (_onceLookups.ContainsKey(k))
				{
					_delegates[e.GetType()] -= k;

					if (_delegates[e.GetType()] == null)
					{
						_delegates.Remove(e.GetType());
					}

					_delegateLookup.Remove(_onceLookups[k]);
					_onceLookups.Remove(k);
				}
			}
		}
		else
		{
			Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
		}
	}

	//Inserts the event into the current queue.
	public bool QueueEvent(GameEvent evt)
	{
		if (!_delegates.ContainsKey(evt.GetType()))
		{
			Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
			return false;
		}

		_eventQueue.Enqueue(evt);
		return true;
	}

	//Every update cycle the queue is processed, if the queue processing is limited,
	//a maximum processing time per update can be set after which the events will have
	//to be processed next update loop.
	void Update()
	{
		float timer = 0.0f;
		while (_eventQueue.Count > 0)
		{
			if (limitQueueProcessing)
			{
				if (timer > queueProcessTime)
					return;
			}

			GameEvent evt = _eventQueue.Dequeue() as GameEvent;
			TriggerEvent(evt);

			if (limitQueueProcessing)
				timer += Time.deltaTime;
		}
	}

	public void OnApplicationQuit()
	{
		RemoveAll();
		_eventQueue.Clear();
		_sInstance = null;
	}
}
