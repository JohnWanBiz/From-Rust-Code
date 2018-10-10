using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class StatusTooltipBehaviour : TooltipBehaviour //updates the tool tip with the sent info
{

	[SerializeField]
	TextMeshProUGUI abilityName;

	[SerializeField]
	TextMeshProUGUI abilityText;

	[SerializeField]
	Status status;

	public override void Display(GameObject hoveredObject = null)
	{
        BroadcastDisplay();
		abilityName.text = "";
		abilityText.text = "";
		status = hoveredObject.GetComponent<StatusIconBehaviour> ().Status; //retrive the specfic stat
		abilityName.text = status.TooltipName;
		abilityText.text = status.TooltipText;
        var height = TextHeight();
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height + 6);
    }

    /// <summary>
    /// These event calls are used by the tutorial to advance text
    /// </summary>
    void BroadcastDisplay()
    {
        EventBus.ServerInstance.Broadcast(EventConstants.StatusTooltipDisplay, this, EventArgs.Empty);
        EventBus.ClientInstance.Broadcast(EventConstants.StatusTooltipDisplay, this, EventArgs.Empty);
    }

    private float TextHeight()
    {
        TextGenerator textGen = new TextGenerator();
        float worldHeight = abilityText.preferredHeight;
        var t = Camera.main.WorldToViewportPoint(new Vector3(0, 0, 0));
        var b = Camera.main.WorldToViewportPoint(new Vector3(0, worldHeight, 0));
        float viewHeight = (b.y - t.y);
        Debug.Log("Status World Height: " + worldHeight);
        Debug.Log("Status View Height: " + viewHeight);
        return viewHeight;
    }

}
