using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1. 상호작용 범위 진입 이벤트 데이터
public class PlayerEnteredInteractionRangeEvent
{
    public IInteractable Target { get; private set; }

    public PlayerEnteredInteractionRangeEvent(IInteractable interactable)
    {
       Target = interactable;
    }
}

// 2. 실제 상호작용 발생 이벤트 데이터
public class PlayerInteractedWithObjectEvent
{
    public IInteractable Target { get; private set; }

    public PlayerInteractedWithObjectEvent(IInteractable interactable)
    {
        Target = interactable;
    }
}

// 3. 상호작용 범위 이탈 이벤트 데이터
public class PlayerExitedInteractionRangeEvent
{
    public IInteractable Target { get; private set; }

    public PlayerExitedInteractionRangeEvent(IInteractable interactable)
    {
        Target = interactable;
    }
}