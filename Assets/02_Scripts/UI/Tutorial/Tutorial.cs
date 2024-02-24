using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject[] page;
    private CanvasGroup[] pageCanvasGroup;
    public GameObject tutorialBook;
    private int currentPage;
    
    public GameObject rightBtn;
    public GameObject leftBtn;

    private void Awake()
    {
        pageCanvasGroup = new CanvasGroup[page.Length];

        for (int i = 0; i < page.Length; i++)
        {
            pageCanvasGroup[i] = page[i].GetComponent<CanvasGroup>();
        }
    }

    public void ClickRightBtn()
    {
        int previuosPageIndex = currentPage;
        Debug.Log(previuosPageIndex);
        currentPage++;
        Debug.Log(currentPage);

        if (currentPage == page.Length - 1)
        {
            rightBtn.SetActive(false);
        }
        if (currentPage == 1)
        {
            leftBtn.SetActive(true);
        }

        TutorialBookFadeOut(previuosPageIndex, currentPage);
    }

    public void ClickLeftBtn()
    {
        int previuosPageIndex = currentPage;
        Debug.Log(previuosPageIndex);
        currentPage--;

        Debug.Log(currentPage);
        if (currentPage == 0)
        {
            leftBtn.SetActive(false);
        }
        if (currentPage == page.Length - 2)
        {
            rightBtn.SetActive(true);
        }

        TutorialBookFadeOut(previuosPageIndex, currentPage);
    }

    public void ClickCloseBtn()
    {
        tutorialBook.SetActive(false);
    }

    public void ClickTutorialBtn()
    {
        tutorialBook.SetActive(true);
    }

    private void TutorialBookFadeOut(int previousPageIndex, int pageIndex)
    {
        Sequence sequence = DOTween.Sequence();

        SetButtonState(false);

        sequence.Append(pageCanvasGroup[previousPageIndex].DOFade(0f, 0.5f)).OnComplete(() => TutorialBookFadeIn(pageIndex));

        // ������ ����
        sequence.Play();
    }

    private void TutorialBookFadeIn(int pageIndex)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(pageCanvasGroup[pageIndex].DOFade(1f, 0.5f));

        SetButtonState(true);

        sequence.Play();
    }

    private void SetButtonState(bool OnOff)
    {
        rightBtn.GetComponent<Image>().raycastTarget = OnOff;
        leftBtn.GetComponent<Image>().raycastTarget = OnOff;
    }
}
