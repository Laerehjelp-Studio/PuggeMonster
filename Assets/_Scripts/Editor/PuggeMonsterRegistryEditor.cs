using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor( typeof( SO_PuggeMonsterRegistry ) )]
/* PuggeMonsterRegistryEditor Author's list
 *	Tor-Arne Sandstrak
 *	ChatGPT as directed by Tor-Arne https://chatgpt.com/share/66f65c4b-4534-8002-9f8b-40460a07690d
 */
public class PuggeMonsterRegistryEditor : Editor {
	private SO_PuggeMonsterRegistry registry;
	private List<PuggeMonster> puggeMonsters;
	private const float gridItemSize = 100f;
	private const float padding = 10f;

	private void OnEnable () {
		registry = (SO_PuggeMonsterRegistry)target;
		puggeMonsters = registry.GetAllPuggeMonsters;
	}

	public override void OnInspectorGUI () {
		// Draw the grid of PuggeMonsters
		DrawPuggeMonsterGrid();

		// "+" button to add new PuggeMonster through Unity's asset creation
		//if (GUILayout.Button( "+", GUILayout.Width( 30 ), GUILayout.Height( 30 ) )) {
		//	EditorApplication.ExecuteMenuItem( "Assets/Create/Scriptable Objects/New PuggeMonster" );
		//}

		// Handle drag-and-drop for adding PuggeMonsters
		HandleDragAndDrop();
	}

	private void DrawPuggeMonsterGrid () {
		EditorGUILayout.LabelField( "List of Pugge Monsters", EditorStyles.boldLabel );

		int columnCount = Mathf.FloorToInt( EditorGUIUtility.currentViewWidth / (gridItemSize + padding) );
		int rowCount = Mathf.CeilToInt( (float)puggeMonsters.Count / columnCount );

		GUILayout.Space( 10 );

		for (int row = 0; row < rowCount; row++) {
			EditorGUILayout.BeginHorizontal();

			for (int col = 0; col < columnCount; col++) {
				int index = row * columnCount + col;
				if (index >= puggeMonsters.Count)
					break;

				PuggeMonster monster = puggeMonsters[ index ];
				DrawGridItem( monster, index );
			}

			EditorGUILayout.EndHorizontal();
		}
	}

	private void DrawGridItem ( PuggeMonster monster, int index ) {
		Rect itemRect = EditorGUILayout.BeginVertical( GUILayout.Width( gridItemSize ), GUILayout.Height( gridItemSize ) );

		// Display the image
		if (monster.Picture != null) {
			GUILayout.Box( monster.Picture.texture, GUILayout.Width( gridItemSize ), GUILayout.Height( gridItemSize - 20 ) );
		}

		// Display the name of the monster
		GUILayout.Label( monster.Name, GUILayout.Width( gridItemSize ));

		// Detect hover and draw "-" button
		if (itemRect.Contains( Event.current.mousePosition )) {
			// Display the "-" button on hover
			Rect removeButtonRect = new Rect( itemRect.xMax - 20, itemRect.yMin, 20, 20 );
			if (GUI.Button( removeButtonRect, "-" )) {
				RemovePuggeMonster( index );
			}

			// Repaint the inspector to keep updating hover state
			Repaint();
		}

		EditorGUILayout.EndVertical();
	}

	private void RemovePuggeMonster ( int index ) {
		puggeMonsters.RemoveAt( index );
		EditorUtility.SetDirty( registry );
		Repaint();
	}

	private void HandleDragAndDrop () {
		Event evt = Event.current;
		Rect dropArea = GUILayoutUtility.GetRect( 0, 50, GUILayout.ExpandWidth( true ) );
		GUI.Box( dropArea, "Drag PuggeMonsters here to add" );

		switch (evt.type) {
			case EventType.DragUpdated:
			case EventType.DragPerform:
				if (!dropArea.Contains( evt.mousePosition ))
					break;

				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

				if (evt.type == EventType.DragPerform) {
					DragAndDrop.AcceptDrag();

					foreach (Object draggedObject in DragAndDrop.objectReferences) {
						if (draggedObject is PuggeMonster draggedMonster && !puggeMonsters.Contains( draggedMonster )) {
							puggeMonsters.Add( draggedMonster );
							EditorUtility.SetDirty( registry );
						}
					}
				}
				break;
		}
	}
}
