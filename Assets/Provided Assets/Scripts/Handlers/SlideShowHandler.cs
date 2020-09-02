using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideShowHandler : MonoBehaviour
{
    public float perImageTime;

    [Header("Objects")]
    public Animator button;

    [Header("Animators")]
    public Animator sliderAnim;
    public Animator pervImageAnim;
    public Animator currenImageAnim;

    [Header("Images")]
    public Image prevImage;
    public Image currentImage;
    public Sprite[] images;

    Coroutine slideShowC;
    Sprite prevSprite;
    Sprite currenSprite;
    int currentSpriteIndex;


    public void StartSlideShow(bool start)
    {
        if(start) gameObject.SetActive(start);
        
        if(slideShowC != null)
            StopCoroutine(slideShowC);

        slideShowC = StartCoroutine(StartSlideShowE(start));

    }

    IEnumerator StartSlideShowE(bool start)
    {
        WaitForSeconds wait = new WaitForSeconds(perImageTime);

        sliderAnim.SetInteger("State", start ? 1 : 0);

        yield return new WaitForSeconds(1);

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

        
        gameObject.SetActive(start);
        slideShowC = null;
    }

    void UpdateSprites()
    {
        prevImage.sprite = currentImage.sprite;
        currentImage.sprite = images[currentSpriteIndex++ % images.Length];

        if(currentSpriteIndex > 10000)
            currentSpriteIndex = 0;
    }
}
