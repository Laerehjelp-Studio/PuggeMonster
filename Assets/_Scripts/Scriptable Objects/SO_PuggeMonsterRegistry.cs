using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PuggeMon Registry", fileName = "PuggeMonster Registry" )]
public class SO_PuggeMonsterRegistry : ScriptableObject
{
	[SerializeField] private List<PuggeMonster> _puggeMonsters = new ();
	public List<PuggeMonster> GetAllPuggeMonsters { get { return _puggeMonsters; } }


}
