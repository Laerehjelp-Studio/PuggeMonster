using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuggeMonsterRewardAnimationBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform midPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float animationSpeed = 1f;
    private float changeAnimationCoolDown = 3f;

    private GameObject Go;

    public void PlayRewardAnimation(int monsterIndex)
    {
        midPos.transform.position = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), midPos.transform.position.z);

        changeAnimationCoolDown = 3f;
        Go = Instantiate(monsterPrefab);
        Go.GetComponentInChildren<Image>().sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(monsterIndex).GetPicture(0);
        Go.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        Go.transform.position = startPos.transform.position;
        Go.transform.rotation = startPos.transform.rotation;
    }

    private void Update()  // yes this code is bad, I was awake for 25 hours when I wrote it. on a 10 hour gamelab day
    {
        if(Go == null)
        {
            return;
        }
        if (changeAnimationCoolDown > 0)
        {
            changeAnimationCoolDown -= Time.deltaTime;
            var step = animationSpeed * Time.deltaTime;
            Go.transform.position = Vector3.MoveTowards(Go.transform.position, midPos.position, step * 4f);
            Go.transform.rotation = Quaternion.RotateTowards(Go.transform.rotation, midPos.rotation, step * 1.2f);
        }
        else
        {
            var step = animationSpeed * Time.deltaTime;
            Go.transform.position = Vector3.MoveTowards(Go.transform.position, endPos.position, step * 2.5f);
            Go.transform.rotation = Quaternion.RotateTowards(Go.transform.rotation, endPos.rotation, step * 1.2f);
            Destroy(Go.gameObject, 2f);
        }
    }
}
