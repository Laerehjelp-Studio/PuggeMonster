using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
		int selectedPuggeMon = Random.Range( 0, _puggeMonRegistry.Length );
		PuggeMonster _puggeMonster = GameObject.Instantiate(_puggeMonRegistry.GetAllPuggeMonsters[ selectedPuggeMon ]);
		PlayerStats.Instance.PuggemonArray[ selectedPuggeMon ]++;
		Debug.Log("User got a new puggeMonster!");
	}
}
