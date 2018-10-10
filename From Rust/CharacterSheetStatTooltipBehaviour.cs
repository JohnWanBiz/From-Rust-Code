using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CharacterSheetStatTooltipBehaviour : TooltipBehaviour //tooltip behaviour for EP and CP stats on the character sheet
{
	[SerializeField]
	CharacterStat stat;

	[SerializeField]
	TextMeshProUGUI titleText;

	[SerializeField]
	Text tooltipText;

	[SerializeField]
	float injuryMod = 1;

    int gearValues;

	public override void Display(GameObject hoveredObject = null)
	{

		CharacterName name = hoveredObject.GetComponentInParent<CharacterInfoDisplayBehaviour> ().CharaName; //retrives the specfic character name 
		Character character = CharacterManager.GetCharacterByName (name); //retrieves everything attached to that character

        int baseValue = (stat == CharacterStat.CP) ? character.BaseCP : character.BaseEP;
        tooltipText.text = string.Format("Base: {0}\n", baseValue);

        Dictionary<string, Tuple<int, StatusCategory>> dictionary = character.StatModifyingSources [stat]; //puts all of the status effects into a dictionary

        injuryMod = (stat == CharacterStat.CP) ? character.CPInjuryMultiplier : character.EPInjuryMultiplier;

        Debug.Log("Dictionary count: " + dictionary.Count);
        //gear
		foreach (KeyValuePair<string, Tuple<int, StatusCategory>> kv in dictionary)
		{
            Debug.Log(string.Format("Character Sheet Stat Tooltip Status: {0}, {1}, {2}", kv.Key, kv.Value.Item1, kv.Value.Item2));
            if (kv.Value.Item2 == StatusCategory.Effect)
            {
                continue;
            }
            Debug.Log("Tooltip is adding gear");
			string symbol = (kv.Value.Item1 > 0) ? "+" : "-";
            string stringText = string.Format("{0}: {1}{2}\n", kv.Key, symbol, Math.Abs(kv.Value.Item1));
			tooltipText.text += stringText;
            gearValues += kv.Value.Item1;
		}

        //injuries
        if (injuryMod != 1)
        {
            var baseStat = (stat == CharacterStat.CP) ? character.BaseCP : character.BaseEP;
            float output = InjuryEquation(injuryMod, baseStat, gearValues);
            string symbolI = (output > 0) ? "+" : "-";
            string stringTextI = string.Format("<color=red>Injury: {0}{1}</color>\n", symbolI, Math.Abs(output));
            tooltipText.text += stringTextI;
        }

        //abilties
        foreach (KeyValuePair<string, Tuple<int, StatusCategory>> kv in dictionary)
        {
            if (kv.Value.Item2 == StatusCategory.Equipped)
            {
                continue;
            }
            Debug.Log("Tooltip is adding effect");
            string symbol = (kv.Value.Item1 > 0) ? "+" : "-";
            string stringText = string.Format("<color=white>{0}: {1}{2}</color>\n", kv.Key, symbol, Math.Abs(kv.Value.Item1));
            tooltipText.text += stringText;
        }

        //calculate height of charactersheet
        var height = TextHeight(tooltipText.text);
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height + 4);
	}


    //function that runs an equation to display the correct injury take away or bonus
    private float InjuryEquation (float injuryModifier, int input, int gear) 
	{
        float baseNum = (float)input;
        float abilityAdd = (float)gear;

        float baseInjury;
        float output;

        baseNum = baseNum + abilityAdd;
        baseInjury = baseNum * injuryModifier;
		output = baseInjury - baseNum;

        if (output % 1 != 0) //rounding down the injury no matter what 
        {
            float roundDown = (output < 0) ? 0.5f : -0.5f;
            output += roundDown;
        }

        if(baseNum == 1 && output == 1) //if it is already at the minimum make the output 0
        {
            output = 0;
        }

		return output;
	}

    private float TextHeight(string newText)
    {
        TextGenerator textGen = new TextGenerator();
        TextGenerationSettings generationSettings = tooltipText.GetGenerationSettings(tooltipText.rectTransform.rect.size);
        float worldHeight = tooltipText.cachedTextGeneratorForLayout.GetPreferredHeight(newText, generationSettings);
        var t = Camera.main.WorldToViewportPoint(new Vector3(0,0,0));
        var b = Camera.main.WorldToViewportPoint(new Vector3(0, worldHeight, 0));
        float viewHeight = (b.y - t.y);
        return viewHeight;
    }
}
