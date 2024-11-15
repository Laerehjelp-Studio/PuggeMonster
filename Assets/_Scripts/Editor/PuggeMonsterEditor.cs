using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/* PuggeMonsterEditor Author's list
 *	Tor-Arne Sandstrak
 *	ChatGPT as directed by Tor-Arne https://chatgpt.com/share/66f2a4f1-0630-8002-9a71-f1f1ba46da8a
 */
[CustomEditor( typeof( PuggeMonster ) )]
public class PuggeMonsterEditor : Editor {
	private SerializedProperty nameProp;
	private SerializedProperty shinyProp;
	private SerializedProperty rarityProp;
	private SerializedProperty imageIndexProp;
	private SerializedProperty imagesProp;
	private SerializedProperty loreProp;  // Added the lore field
	private SerializedProperty collectAudioProp;  // Added the lore field

	private void OnEnable () {
		// Cache the serialized properties
		nameProp = serializedObject.FindProperty( "Name" );
		shinyProp = serializedObject.FindProperty( "Shiny" );
		rarityProp = serializedObject.FindProperty( "Rarity" );
		imageIndexProp = serializedObject.FindProperty( "ImageIndex" );
		imagesProp = serializedObject.FindProperty( "Images" );
		loreProp = serializedObject.FindProperty( "Lore" ); 
		collectAudioProp = serializedObject.FindProperty( "CollectSound" ); 
	}

	public override void OnInspectorGUI () {
		serializedObject.Update();

		// Big text field for the Name
		EditorGUILayout.LabelField( "Name", EditorStyles.boldLabel );
		GUIStyle largeTextStyle = new GUIStyle( EditorStyles.textField );
		largeTextStyle.fontSize = 20;
		nameProp.stringValue = EditorGUILayout.TextField( nameProp.stringValue, largeTextStyle, GUILayout.Height( 30 ) );

		EditorGUILayout.Space();

		// Picture frame logic
		EditorGUILayout.LabelField( "Picture", EditorStyles.boldLabel );
		Rect imageRect = GUILayoutUtility.GetRect( 200, 200, GUILayout.ExpandWidth( true ) );
		// Background removed to support transparency

		Sprite currentSprite = null;
		if (imagesProp.arraySize > 0 && imageIndexProp.intValue < imagesProp.arraySize) {
			currentSprite = imagesProp.GetArrayElementAtIndex( imageIndexProp.intValue ).objectReferenceValue as Sprite;
		}

		if (currentSprite != null && currentSprite.texture != null) {
			Texture2D spriteTexture = currentSprite.texture;
			Rect spriteRect = new Rect( imageRect.x, imageRect.y, imageRect.width, imageRect.height );

			Rect textureRect = new Rect(
				currentSprite.textureRect.x / spriteTexture.width,
				currentSprite.textureRect.y / spriteTexture.height,
				currentSprite.textureRect.width / spriteTexture.width,
				currentSprite.textureRect.height / spriteTexture.height
			);

			GUI.DrawTextureWithTexCoords( spriteRect, spriteTexture, textureRect, true );
		} else {
			EditorGUI.LabelField( imageRect, "No Image Available", EditorStyles.centeredGreyMiniLabel );
		}

		HandleDragAndDrop( imageRect );


		EditorGUILayout.Space();

		// Display the Images array
		EditorGUILayout.PropertyField( imagesProp, true );

		// Display the Image Index slider
		EditorGUILayout.LabelField( "Default Picture Selector", EditorStyles.boldLabel );
		imageIndexProp.intValue = EditorGUILayout.IntSlider( imageIndexProp.intValue, 0, Mathf.Max( 0, imagesProp.arraySize - 1 ) );


		EditorGUILayout.Space();

		// Lore field (multiline)
		EditorGUILayout.LabelField( "Lore", EditorStyles.boldLabel );
		loreProp.stringValue = EditorGUILayout.TextArea( loreProp.stringValue, GUILayout.Height( 60 ) );

		EditorGUILayout.Space();

		// Bigger Shiny and Rarity fields
		GUIStyle boldLargeStyle = new GUIStyle( EditorStyles.boldLabel );
		boldLargeStyle.fontSize = 16;

		EditorGUILayout.LabelField( "Shiny", boldLargeStyle );
		shinyProp.boolValue = EditorGUILayout.Toggle( shinyProp.boolValue, GUILayout.Height( 30 ) );

		EditorGUILayout.LabelField( "Rarity", boldLargeStyle );
		rarityProp.enumValueIndex = EditorGUILayout.EnumPopup( (PuggeMonsterRarity)rarityProp.enumValueIndex, GUILayout.Height( 30 ) ).GetHashCode();

		EditorGUILayout.LabelField("Audio Events", boldLargeStyle );
		EditorGUILayout.PropertyField(collectAudioProp);
		
		serializedObject.ApplyModifiedProperties();
	}

	private void HandleDragAndDrop ( Rect imageRect ) {
		Event evt = Event.current;

		// Detect if dragging is happening
		if (evt.type == EventType.DragUpdated && imageRect.Contains( evt.mousePosition )) {
			DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
			evt.Use();
		}

		// Detect if something is dropped
		if (evt.type == EventType.DragPerform && imageRect.Contains( evt.mousePosition )) {
			DragAndDrop.AcceptDrag();

			foreach (Object draggedObject in DragAndDrop.objectReferences) {
				if (draggedObject is Sprite sprite) {
					// Add the sprite to the Images array
					imagesProp.InsertArrayElementAtIndex( imagesProp.arraySize );
					imagesProp.GetArrayElementAtIndex( imagesProp.arraySize - 1 ).objectReferenceValue = sprite;
				} else if (draggedObject is Texture2D texture) {
					// Check if there are multiple sprites in the texture
					Sprite[] spritesFromTexture = GetSpritesFromTexture( texture );
					if (spritesFromTexture.Length > 0) {
						foreach (Sprite extractedSprite in spritesFromTexture) {
							imagesProp.InsertArrayElementAtIndex( imagesProp.arraySize );
							imagesProp.GetArrayElementAtIndex( imagesProp.arraySize - 1 ).objectReferenceValue = extractedSprite;
						}
					} else {
						// If no sprites are found, create a single sprite from the texture
						Sprite newSprite = CreateSpriteFromTexture( texture );
						imagesProp.InsertArrayElementAtIndex( imagesProp.arraySize );
						imagesProp.GetArrayElementAtIndex( imagesProp.arraySize - 1 ).objectReferenceValue = newSprite;
					}
				}
			}
			// Mark the change to ensure Unity knows something was changed
			serializedObject.ApplyModifiedProperties();
			evt.Use();
		}
	}
	private Sprite[] GetSpritesFromTexture ( Texture2D texture ) {
		string path = AssetDatabase.GetAssetPath( texture );
		Object[] assets = AssetDatabase.LoadAllAssetsAtPath( path );

		List<Sprite> sprites = new List<Sprite>();
		foreach (var asset in assets) {
			if (asset is Sprite sprite) {
				sprites.Add( sprite );
			}
		}

		return sprites.ToArray();
	}


	private Sprite CreateSpriteFromTexture ( Texture2D texture ) {
		return Sprite.Create( texture, new Rect( 0, 0, texture.width, texture.height ), new Vector2( 0.5f, 0.5f ) );
	}
}
