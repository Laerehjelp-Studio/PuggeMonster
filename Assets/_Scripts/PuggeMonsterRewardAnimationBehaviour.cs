using UnityEngine;
using UnityEngine.UI;

public class PuggeMonsterRewardAnimationBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform midPos;
    [SerializeField] private Transform endPos;
    [SerializeField] private float animationSpeed = 1f;

    [SerializeField] private BlackBarsFadeInController blackBarsAnimationScript;

    private PuggemonObjectPublicProperties PmonObject;

    private float changeAnimationCoolDown = 3f;

    private GameObject Go;

    private Button monsterButton;

    private int currentMonsterIndex;
    private bool isAnimatingTowardsMid = false;
    private bool isAnimatingTowardsLeft = false;

    public void PlayRewardAnimation(int monsterIndex) {
        GameManager.PuggeMonAppearSound();
        blackBarsAnimationScript.StartAnimatingBlackBars();
        currentMonsterIndex = monsterIndex;
        isAnimatingTowardsMid = true;

        changeAnimationCoolDown = 3f;
        Go = Instantiate(monsterPrefab);
        Go.transform.localScale = new Vector3(0.6f, 0.6f, 1);
        PmonObject = Go.GetComponent<PuggemonObjectPublicProperties>();
        PmonObject.Picture1.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(monsterIndex).GetPicture(1);
        PmonObject.Picture2.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(monsterIndex).GetPicture(1);
        Go.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        Go.transform.position = startPos.transform.position;
        Go.transform.rotation = startPos.transform.rotation;
    }

    private void Update()  // yes this code is bad, I was awake for 25 hours when I wrote it. on a 10 hour gamelab day
    {
        if(isAnimatingTowardsMid)
        {
            SlideMonsterFromTheRightToMid();
        }
        if(isAnimatingTowardsLeft)
        {
            SlideMonsterFromMidToLeft();
        }
    }

    private void SlideMonsterFromTheRightToMid()
    {
        if(Vector3.Distance(Go.transform.position, midPos.transform.position) > 0.5f)
        {
            var step = animationSpeed * Time.deltaTime;
            Go.transform.position = Vector3.MoveTowards(Go.transform.position, midPos.position, step * 1.5f);
        }
        else
        {
            isAnimatingTowardsMid = false;
            //Go.GetComponentInChildren<Image>().sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(currentMonsterIndex).GetPicture(0);
            PmonObject.Picture1.sprite = MonsterIndexLibrary.Instance.GetMonsterFromIndex(currentMonsterIndex).GetPicture(0);
            monsterButton = Go.GetComponentInChildren<Button>();
            monsterButton.interactable = true;
            monsterButton.onClick.AddListener(startAnimatingOutOfScreen);
        }
    }

    private void startAnimatingOutOfScreen() {
        GameManager.PlayPuggemonCollectSound(currentMonsterIndex);
        PlayerStats.Instance.AddPuggeMonster(currentMonsterIndex);
        isAnimatingTowardsLeft = true;
    }

    private void SlideMonsterFromMidToLeft()
    {
        monsterButton.onClick.RemoveAllListeners();

        if (Vector3.Distance(Go.transform.position, endPos.transform.position) > 0.5f || Go.transform.position.x > endPos.transform.position.x)
        {
            var step = animationSpeed * Time.deltaTime;
            Go.transform.position = Vector3.MoveTowards(Go.transform.position, endPos.position, step * 3f);
        }
        else
        {
            isAnimatingTowardsLeft = false;
            Destroy(Go.gameObject);
            blackBarsAnimationScript.RemoveBlackBars();
        }
    }


    private void AnimateTheObjectFromPointToPoint()
    {
        if (Go == null)
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
