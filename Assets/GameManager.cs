using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalScoreText;
    int totalPoint = 0;
    private void Start()
    {
        totalScoreText.SetText("Score: 0");
    }
    public void CreatePointText(TextMeshProUGUI catScoreText, float fillAmount)
    {
        catScoreText.gameObject.SetActive(true);
        catScoreText.gameObject.GetComponent<RectTransform>().DOLocalMoveY(20, 0.5f);
        if (fillAmount > 0.5f)
        {
            totalPoint += 400;
            catScoreText.SetText("+400 Points");
            StartCoroutine(TurnOffCatScoreTxt(catScoreText.gameObject));
        }
        else
        {
            catScoreText.SetText("+200 Points");
            StartCoroutine(TurnOffCatScoreTxt(catScoreText.gameObject));
            totalPoint += 200;
        }

        totalScoreText.SetText("Score: " + totalPoint.ToString());
    }

    IEnumerator TurnOffCatScoreTxt(GameObject catScoreTextObj)
    {
        yield return new WaitForSecondsRealtime(0.7f);
        catScoreTextObj.SetActive(false);
    }
}
