using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EventBus
{
    // �̺�Ʈ Ÿ���� Ű��, �ش� �̺�Ʈ Ÿ���� �ڵ鷯(delegate)�� ������ �����ϴ� ��ųʸ�
    private static readonly Dictionary<Type, Delegate> eventDict
        = new Dictionary<Type, Delegate>();

    //Ư�� Ÿ���� �̺�Ʈ�� �����ϴ� �޼���
    public static void Subscribe<TEvent>(Action<TEvent> handler)
    {   
        Type eventType = typeof(TEvent);
        if(eventDict.TryGetValue(eventType, out Delegate existingHandler))
        {
            //�̹� �ش� Ÿ���� �ڵ鷯�� �����ϸ�, ��������Ʈ�� �߰�
            eventDict[eventType] = Delegate.Combine(existingHandler, handler);
        }
        else
        {
            //���ο� Ÿ���� �ڵ鷯���, ��ųʸ��� ���� �߰�
            eventDict[eventType] = handler;
        }
    }

    //Ư�� Ÿ���� �̺�Ʈ ������ �����ϴ� �޼���
    public static void UnSubscribe<TEvent>(Action<TEvent> handler)
    {
        Type eventType = typeof(TEvent);

        if(eventDict.TryGetValue(eventType, out Delegate existingHandler))
        {
            Delegate newHandler = Delegate.Remove(existingHandler, handler);
            if(newHandler == null)
            {   //�ش� Ÿ���� �ڵ鷯�� ��� ���ŵǾ��� ���
                eventDict.Remove(eventType);
            }
            else
            {
                eventDict[eventType] = newHandler;
                // Debug.Log($"[EventBus] Unsubscribed from {eventType.Name}");
            }
        }
    }


    //Ư�� Ÿ���� �̺�Ʈ�� �����ϴ� �޼���
    public static void Raise<TEvent>(TEvent eventArgs)
    {
        Type eventType = typeof(TEvent);
        if(eventDict.TryGetValue(eventType, out Delegate existingHandler))
        {   //����� ��������Ʈ�� ������ Action<TEvent> Ÿ������ ĳ�����Ͽ� ȣ��
            (existingHandler as Action<TEvent>)?.Invoke(eventArgs);
        }
        else
        {
            Debug.LogWarning($"[EventBus] No subscribers for event {eventType.Name}");
        }
    }

}
