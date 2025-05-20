using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1. ��ȣ�ۿ� ���� ���� �̺�Ʈ ������
public class PlayerEnteredInteractionRangeEvent
{
    public IInteractable Target { get; private set; }

    public PlayerEnteredInteractionRangeEvent(IInteractable interactable)
    {
       Target = interactable;
    }
}

// 2. ���� ��ȣ�ۿ� �߻� �̺�Ʈ ������
public class PlayerInteractedWithObjectEvent
{
    public IInteractable Target { get; private set; }

    public PlayerInteractedWithObjectEvent(IInteractable interactable)
    {
        Target = interactable;
    }
}

// 3. ��ȣ�ۿ� ���� ��Ż �̺�Ʈ ������
public class PlayerExitedInteractionRangeEvent
{
    public IInteractable Target { get; private set; }

    public PlayerExitedInteractionRangeEvent(IInteractable interactable)
    {
        Target = interactable;
    }
}