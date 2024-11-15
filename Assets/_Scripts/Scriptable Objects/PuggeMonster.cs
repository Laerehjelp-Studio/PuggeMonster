using System;
using UnityEngine;

[CreateAssetMenu( menuName = "Scriptable Objects/New PuggeMonster", fileName = "PM_PuggeMonster" )]
[Serializable]
public class PuggeMonster : ScriptableObject {
	public bool Shiny;
	public PuggeMonsterRarity Rarity = PuggeMonsterRarity.KjempeVanlig;
	public int ImageIndex = 0;
	public Sprite[] Images;
	public Sprite Picture { get { return Images[ ImageIndex ]; } }
	public string Name;
	public string Lore;
	public SimpleAudioEvent CollectSound;
	public PuggeMonster ( PuggeMonsterRarity rarity, int spriteIndex) {
		Rarity = rarity;
		ImageIndex = spriteIndex;
	}
	public PuggeMonster (  ) {
		// Randomize everything.
	}
	public Sprite GetPicture(int index) {
		return Images[index];
	}
}

public enum PuggeMonsterRarity {
	KjempeVanlig,
	Vanlig,
	Uvanlig,
	Episk,
	Legendarisk
}