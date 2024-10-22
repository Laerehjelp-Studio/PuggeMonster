using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackArrowBehaviour : MonoBehaviour
{
    public void UnloadCurrentScene()
    {
        GameManager.Instance.UnloadCodeScene();
    }
}
