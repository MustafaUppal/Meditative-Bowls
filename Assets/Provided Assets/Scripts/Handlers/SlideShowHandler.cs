using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideShowHandler : MonoBehaviour
{
    public int prevState = -1;
    public int currentState = -1;

    public GameObject root;
    public float perImageTime;

    [Header("Objects")]
    public GameObject blocker;

    [Header("Animators")]
    public Animator sliderAnim;
    public Animator pervImageAnim;
    public Animator currenImageAnim;
    public Animator[] buttonSelectors;

    [Header("Images")]
    public Image prevImage;
    public Image currentImage;
    public Sprite[] images;

    [Header("Buttons")]
    public Button[] setsButton;

    Coroutine slideShowC;
    Sprite prevSprite;
    Sprite currenSprite;
    int currentSpriteIndex;

    InventoryManager Inventory => InventoryManager.Instance;

    private void OnEnable() 
    {
        for(int i = 0; i < Inventory.GetItemCount(2); i++)
        {
            setsButton[i].interactable = Inventory.allSlideShows[i].currentState == Item.State.Purchased;
            prevState = i;
            SelectButton();
        }

        blocker.SetActive(true);
    }

    private void OnDisable() 
    {
        if(slideShowC != null)
            StopCoroutine(slideShowC);
    }

    public void EnableSlideShow(bool start)
    {
        if(start) gameObject.SetActive(start);
        sliderAnim.SetInteger("State", start ? 1 : 0);

        // if(slideShowC != null)
        //     StopCoroutine(slideShowC);

        // slideShowC = StartCoroutine(StartSlideShowE(start));

    }

    public void ChangeState(int state)
    {
        prevState = currentState;
        currentState = state;
        SelectButton();
    }

    public void OnClickSlideShowSetButton(int index) 
    {
        ChangeState(index);
        images = Inventory.allSlideShows[index].images;
        blocker.SetActive(false);

        if(slideShowC != null)
            StopCoroutine(slideShowC);

        slideShowC = StartCoroutine(StartSlideShowE(true));
    }

    IEnumerator StartSlideShowE(bool start)
    {
        WaitForSeconds wait = new WaitForSeconds(perImageTime);

        while(start)
        {
            UpdateSprites();
            prevImage.gameObject.SetActive(false);
            prevImage.gameObject.SetActive(true);

            currentImage.gameObject.SetActive(false);
            currentImage.gameObject.SetActive(true);

            // pervImageAnim.Play("Out");
            // currenImageAnim.Play("In");
            yield return wait;
        }

        slideShowC = null;
    }

    public void SelectButton()
    {
        
        if(prevState != -1)
        buttonSelectors[prevState].Play("Deselect");

        if(currentState != -1)
        buttonSelectors[currentState].Play("Select");
    }

    void UpdateSprites()
    {
        prevImage.sprite = currentImage.sprite;
        currentImage.sprite = images[currentSpriteIndex++ % images.Length];

        if(currentSpriteIndex > 10000)
            currentSpriteIndex = 0;
    }
}
