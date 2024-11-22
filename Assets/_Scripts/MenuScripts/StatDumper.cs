using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDumper : MonoBehaviour
{
	[SerializeField] private GameObject _contentPrefab;
	[SerializeField] private Transform _targetTransform;

	private void Awake () {
		//StatManager.OnDatabaseUpdate += UpdateInterface;
	}

	private void UpdateInterface () {
		OperatorStore operatorStore = StatManager.GetStore;
		RemoveChildren();
		Debug.Log("Running Update Interface");
		GameObject _additionGameObject = Instantiate(_contentPrefab, _targetTransform);
		if (_additionGameObject.TryGetComponent(out ContentDumpOperator additionDumpOperator )) {
			//additionDumpOperator.OperatorDump(operatorStore.Addition, "Addition" );
			additionDumpOperator.OperatorDump("+", "Addition" );
		}

		GameObject _subtractionGameObject = Instantiate( _contentPrefab, _targetTransform );
		if (_subtractionGameObject.TryGetComponent( out ContentDumpOperator subtractionDumpOperator )) {
			//subtractionDumpOperator.OperatorDump( operatorStore.Subtraction, "Subtraction" );
			subtractionDumpOperator.OperatorDump("-", "Subtraction" );
		}

		GameObject _multiplicationGameObject = Instantiate( _contentPrefab, _targetTransform );
		if (_multiplicationGameObject.TryGetComponent( out ContentDumpOperator multiplicationDumpOperator )) {
			//multiplicationDumpOperator.OperatorDump( operatorStore.Multiplication, "Multiplication" );
			multiplicationDumpOperator.OperatorDump("*", "Multiplication" );
		}

		GameObject _divisionGameObject = Instantiate( _contentPrefab, _targetTransform );
		if (_divisionGameObject.TryGetComponent( out ContentDumpOperator divisionDumpOperator )) {
			//divisionDumpOperator.OperatorDump( operatorStore.Division, "Division" );
			divisionDumpOperator.OperatorDump("/", "Division" );
		}
		
		// Words Mastery
		//Dictionary<string, float> _wordMasteryScores = StatManager.GetWordMasteryScore;
		//GameObject _wordMasteryPrefab = Instantiate( _contentPrefab, _targetTransform );
		//if (_wordMasteryPrefab.TryGetComponent( out ContentDumpWord divisionDumpOperator )) {
		//	_wordMasteryPrefab.OperatorDump( operatorStore.Division, "Division" );
		//}
	}

	private void RemoveChildren () {
		for (int i = 0; i < _targetTransform.childCount; i++) {
			Destroy( _targetTransform.GetChild(i).gameObject );
		}
	}

	private void OnEnable () {
		UpdateInterface();
	}
}
