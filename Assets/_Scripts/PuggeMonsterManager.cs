using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
static public class PuggeMonsterManager
{
	[SerializeField] static private SO_PuggeMonsterRegistry _puggeMonRegistry;
	[SerializeField] static private List<PuggeMonster> _playerPuggeMonRegistry = new();

	static public List<PuggeMonster> AllPuggeMonsters { get {  return _puggeMonRegistry.GetAllPuggeMonsters; } }

	/// <summary>
	/// Adds a randomized characteristic PuggeMon
	/// </summary>
	static public void AddPuggeMonster() {
		PuggeMonster _puggeMonster = new PuggeMonster();
		Debug.Log("User got a new puggeMonster!");
	}
}
