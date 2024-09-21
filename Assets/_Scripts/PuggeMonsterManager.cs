using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
static public class PuggeMonsterManager
{
	[SerializeField] static private List<PuggeMonster> _puggeMonRegistry = new();
	[SerializeField] static private List<PuggeMonster> _playerPuggeMonRegistry = new();

	static public List<PuggeMonster> PuggeMonster { get {  return _puggeMonRegistry; } }

	/// <summary>
	/// Adds a randomized characteristic PuggeMon
	/// </summary>
	static public void AddPuggeMonster() {
		PuggeMonster _puggeMonster = new PuggeMonster();
		Debug.Log("User got a new puggeMonster!");
	}
}
