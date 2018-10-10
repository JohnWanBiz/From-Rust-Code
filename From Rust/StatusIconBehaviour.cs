using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusIconBehaviour : MonoBehaviour //displays the status in the slots by activiting the picture and send the info the assigned tool tip prefab
{

	Status status;
	public Status Status { get {return status; } }

	[SerializeField]
	Image icon;

    [SerializeField]
    Text stackText;

    HasTooltipBehaviour tooltipBehaviour;

	void Start()
	{
		icon.enabled = false;
        stackText.text = "";
        tooltipBehaviour = GetComponent<HasTooltipBehaviour>();
        if (tooltipBehaviour != null)
            tooltipBehaviour.DisableTooltip();
    }

	public void AddStatus(Status currentStatus)
	{
		icon.enabled = true;
		icon.sprite = currentStatus.CharacterSheetIcon;
        stackText.text = (currentStatus.CurrentStack > 1) ? currentStatus.CurrentStack.ToString() : "";
		status = currentStatus;
        if (tooltipBehaviour != null)
            tooltipBehaviour.EnableTooltip();
	}

	public void RemoveStatus()
	{
		status = null;
		icon.enabled = false;
        stackText.text = "";
        if (tooltipBehaviour != null)
            tooltipBehaviour.DisableTooltip();
    }

    public void UpdateStackText()
    {
        Debug.Log("Updating Stack Text");
        if (status != null)
        {
            stackText.text = (status.CurrentStack > 1) ? status.CurrentStack.ToString() : "";
        }
    }
}
