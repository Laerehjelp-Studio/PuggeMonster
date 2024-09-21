using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

[Serializable]
public class PuggeMonster {
	public bool Shiny;
	public PuggeMonsterRarity Rarity {get; private set;}
	public Sprite Image;
	
	public PuggeMonster (bool shiny, PuggeMonsterRarity rarity, Sprite sprite) {
		Shiny = shiny;
		Rarity = rarity;
		Image = sprite;
	}
	public PuggeMonster (  ) {
		// Randomize everything.
	}
}

public enum PuggeMonsterRarity {
	KjempeVanlig,
	Vanlig,
	Uvanlig,
	Episk,
	Legendarisk
}