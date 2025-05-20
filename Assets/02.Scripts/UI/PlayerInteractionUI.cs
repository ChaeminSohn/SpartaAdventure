using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractionUI : MonoBehaviour
{
    [Header("Enter Interaction")]
    [SerializeField] private GameObject canInteractPanel;
    [SerializeField] private TextMeshProUGUI interactNameText;
    [SerializeField] private TextMeshProUGUI canInteractText;
    

    [Header("On Interaction")]
    [SerializeField] private GameObject interactGuidePanel;
    [SerializeField] private TextMeshProUGUI interactGuideText;


    private void OnEnable()
    {
        EventBus.Subscribe<PlayerEnteredInteractionRangeEvent>(ShowInteractionPrompt);
        EventBus.Subscribe<PlayerExitedInteractionRangeEvent>(HideInteractionPrompt);
        EventBus.Subscribe<PlayerInteractedWithObjectEvent>(OnInteractionPerformed);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerEnteredInteractionRangeEvent>(ShowInteractionPrompt);
        EventBus.UnSubscribe<PlayerExitedInteractionRangeEvent>(HideInteractionPrompt);
        EventBus.UnSubscribe<PlayerInteractedWithObjectEvent>(OnInteractionPerformed);
    }

    private void ShowInteractionPrompt(PlayerEnteredInteractionRangeEvent args) 
    {
        canInteractText.text = args.Target.GetInteractPrompt(); ;
        interactNameText.text = args.Target.ItemData.displayName;
        canInteractPanel.SetActive(true);
    }
    private void HideInteractionPrompt(PlayerExitedInteractionRangeEvent args)
    {
        canInteractPanel.SetActive(false);
        interactGuidePanel.SetActive(false);
    }
    private void OnInteractionPerformed(PlayerInteractedWithObjectEvent args)
    {
        string str = string.Empty;

        switch (args.Target.ItemType)
        {   
            case ItemType.Readonly:
                str = args.Target.ItemData.description;
                break;
            default:
                canInteractPanel.SetActive(false);
                return;
        }
        interactGuideText.text = str;
        canInteractPanel.SetActive(false);
        interactGuidePanel.SetActive(true);
    }
 
}
