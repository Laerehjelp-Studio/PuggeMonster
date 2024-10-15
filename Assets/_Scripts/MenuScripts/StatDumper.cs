using System;
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
		Debug.Log("Running Update Interface");
		GameObject _additionGameObject = Instantiate(_contentPrefab, _targetTransform);
		if (_additionGameObject.TryGetComponent(out ContentDumpOperator additionDumpOperator )) {
			additionDumpOperator.OperatorDump(operatorStore.Addition, "Addition" );
		}

		GameObject _subtractionGameObject = Instantiate( _contentPrefab, _targetTransform );
		if (_subtractionGameObject.TryGetComponent( out ContentDumpOperator subtractionDumpOperator )) {
			subtractionDumpOperator.OperatorDump( operatorStore.Subtraction, "Subtraction" );
		}

		GameObject _multiplicationGameObject = Instantiate( _contentPrefab, _targetTransform );
		if (_multiplicationGameObject.TryGetComponent( out ContentDumpOperator multiplicationDumpOperator )) {
			multiplicationDumpOperator.OperatorDump( operatorStore.Multiplication, "Multiplication" );
		}

		GameObject _divisionGameObject = Instantiate( _contentPrefab, _targetTransform );
		if (_divisionGameObject.TryGetComponent( out ContentDumpOperator divisionDumpOperator )) {
			divisionDumpOperator.OperatorDump( operatorStore.Division, "Division" );
		}
	}



	private void OnEnable () {
		UpdateInterface();
	}
}
