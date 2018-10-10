using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StatusHolderBehaviour : MonoBehaviour //manages the slots and assigns the statuses to the slots
{
		
	[SerializeField]
	GameObject characterSheet; //character sheet object to be assigned

	CharacterName characterName;

	Character character;

	[SerializeField]
	StatusCategory state;

    List <Status> statuses;

	[SerializeField]
	List <StatusIconBehaviour> slots;

	void Start ()
	{
		characterName = characterSheet.GetComponent<CharacterInfoDisplayBehaviour>().CharaName; //retrives the specfic character name 
		character = CharacterManager.GetCharacterByName (characterName); //retrieves everything attached to that specific character
		statuses = character.Statuses();
        EventBus.ClientInstance.Subscribe(EventConstants.StatusStacksUpdated, OnStackUpdated);
    }

	void Update () 
	{
        foreach (StatusIconBehaviour slot in slots)
		{
			slot.RemoveStatus(); //sets the slots equal to null so they wont be repeated due to the (update loop)
		}

		statuses = character.Statuses(); //makes and adds the statses to a list of them

		foreach (Status status in statuses) //accesses the statuses in the list
		{
            if (!status.ShowInCharacterSheet || status.CharacterSheetCategory != state)
            {
                continue;
            }

            StatusIconBehaviour slot = FindFirstEmptySlot();

            if (slot != null)
            {
                slot.AddStatus(status);
            }
		}
	}

    void OnStackUpdated(object source, EventArgs args)
    {
        foreach (StatusIconBehaviour slot in slots)
        {
            slot.UpdateStackText();
        }
    }

    private StatusIconBehaviour FindFirstEmptySlot ()
    {
        foreach (StatusIconBehaviour slot in slots)
        {
            if (slot.Status == null)
            {
                return slot;
            }
        }
        return null; //if there is no empty slot, return nothing
    }

}
