using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{
    public bool isCooking = false;
    [SerializeField] private GameObject foodCompletionMark;

    [Header("Sushi")]
    [SerializeField] private GameObject sushiPanel;
    [SerializeField] private GameObject sushiMainPlate;
    [SerializeField] private GameObject sushiLeftPlate;
    [SerializeField] private GameObject sushiRightPlate;
    [SerializeField] private GameObject sushiSeaWeedLeftPlate;
    [SerializeField] private GameObject sushiSeaWeedMainPlate;
    [SerializeField] private GameObject sushiRiceRightPlate;
    [SerializeField] private GameObject sushiRiceWithSeaWeedMainPlate;
    [SerializeField] private GameObject sushiSlider;
    [SerializeField] private Sprite sushiSprite1;
    [SerializeField] private Sprite sushiSprite2;
    [SerializeField] private Sprite sushiSprite3;
    [SerializeField] private Sprite sushiSprite4;

    private int cookingPhase;
    [SerializeField] private int currentSushiPhase;

    [Header("Yumurta Salatasi")]
    [SerializeField] private GameObject ysPanel;
    [SerializeField] private GameObject ysMainPlate;
    [SerializeField] private GameObject ysLeftPlate;
    [SerializeField] private GameObject ysRightPlate;
    [SerializeField] private GameObject ysLettuceLeftPlate;
    [SerializeField] private GameObject ysLettuceMainPlate;
    [SerializeField] private GameObject ysEggRightPlate;
    [SerializeField] private List<GameObject> ysEggMainPlate;
    [SerializeField] private List<GameObject> ysSliders;
    [SerializeField] private List<Sprite> ysLettuceSprites;

    [Header("Lazanya")]
    [SerializeField] private GameObject LazanyaPanel;
    [SerializeField] private GameObject LazanyaMainPlate;
    [SerializeField] private GameObject LazanyaLeftPlate;
    [SerializeField] private GameObject LazanyaRightPlate;
    [SerializeField] private GameObject LazanyaBorekLeftPlate;
    [SerializeField] private List<GameObject> LazanyaBoreklerMainPlate;
    [SerializeField] private GameObject LazanyaKiymaRightPlate;
    [SerializeField] private List<GameObject> LazanyaKiymaMainPlate;
    private int borekIndex;
    private int kiymaIndex;
    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip ChoppingMarulSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Slider slider = sushiSlider.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate
        {
            SushiSliderListener(slider);

        });
        foreach (var ys in ysSliders)
        {
            ys.GetComponent<Slider>().onValueChanged.AddListener(delegate
            {
                YSSliderListener();
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMakingSushi()
    {
        int N = sushiMainPlate.transform.childCount;
        sushiMainPlate.transform.GetChild(0).gameObject.SetActive(false);
        sushiPanel.SetActive(true);
        isCooking = true;
        cookingPhase = 0;
    }
    public void StartMakingYS()
    {
        ysSliders[0].SetActive(true);
        ysPanel.SetActive(true);
        isCooking = true;
        cookingPhase = 0;
    }
    public void StartMakingLazanya()
    {
        LazanyaPanel.SetActive(true);
        isCooking = true;
        kiymaIndex = 0;
        borekIndex = 0;
    }
    public void SeaweedOnClick()
    {
        if (isCooking && cookingPhase == 0)
        {
            sushiSeaWeedMainPlate.SetActive(true);
            cookingPhase = 1;
        }
    }

    public void RiceOnClick()
    {
        if (isCooking && cookingPhase == 1)
        {
            sushiRiceWithSeaWeedMainPlate.GetComponent<Image>().sprite = sushiSprite1;
            sushiSeaWeedMainPlate.SetActive(!true);
            sushiRiceWithSeaWeedMainPlate.SetActive(true);
            cookingPhase = 2;
            currentSushiPhase = 0;
            sushiSlider.GetComponent<Slider>().value = 0;
            sushiSlider.SetActive(true);
        }
    }

    public void EggOnClick()
    {
        for (int i = 0; i < ysEggMainPlate.Count; i++)
        {
            if (!ysEggMainPlate[i].activeInHierarchy)
            {
                ysEggMainPlate[i].SetActive(true);
                ControlYSFinished();
                return;
            }
        }
    }

    public void LettuceOnClick()
    {
        ysLettuceMainPlate.SetActive(true);
    }

    public void BorekOnClick()
    {
        if(borekIndex == kiymaIndex)
        {
            LazanyaBoreklerMainPlate[borekIndex++].SetActive(true);
            if(borekIndex >= 3)
            {
                StartCoroutine(PopCompleted(2));
            }
        }
    }

    public void KiymaOnClick()
    {
        if (kiymaIndex < 2 && borekIndex - 1 == kiymaIndex)
        {
            LazanyaKiymaMainPlate[kiymaIndex++].SetActive(true);
        }
    }

    public void ControlYSFinished()
    {
        int N = 0;
        for (int i = 0; i < ysEggMainPlate.Count; i++)
        {
            if (ysEggMainPlate[i].activeInHierarchy)
            {
                ysEggMainPlate[i].SetActive(true);
                N++;
            }
        }
        if (N == 3 && cookingPhase >= 3)
        {
            StartCoroutine(PopCompleted(1));
        }
    }
    public void YSSliderListener()
    {
        if(cookingPhase < 4)
        if (ysSliders[cookingPhase].GetComponent<Slider>().value == 100)
        {
            
            ControlYSFinished();
            if(cookingPhase < 4)
            {
                ysSliders[cookingPhase].SetActive(false);
            }
            if (cookingPhase < 4) 
            {
                ysLettuceMainPlate.GetComponent<Image>().sprite = ysLettuceSprites[cookingPhase + 1];
                audioSource.PlayOneShot(ChoppingMarulSound);
                cookingPhase++;
            }
            
            if (cookingPhase < 4)
            {
                ysSliders[cookingPhase].SetActive(true);
            }

        }
    }
    public void SushiSliderListener(Slider slider)
    {
        if (isCooking = true && currentSushiPhase != 0 && slider.value >= 0 && slider.value < 33)
        {
            sushiRiceWithSeaWeedMainPlate.GetComponent<Image>().sprite = sushiSprite1;
            currentSushiPhase = 0;
        }
        else if (isCooking = true && currentSushiPhase != 1 && slider.value >= 33 && slider.value < 66)
        {
            sushiRiceWithSeaWeedMainPlate.GetComponent<Image>().sprite = sushiSprite2;
            currentSushiPhase = 1;
        }
        else if (isCooking = true && currentSushiPhase != 2 && slider.value >= 66 && slider.value < 96)
        {
            sushiRiceWithSeaWeedMainPlate.GetComponent<Image>().sprite = sushiSprite3;
            currentSushiPhase = 2;
        }
        else if (isCooking = true && currentSushiPhase != 3 && slider.value >= 96 && slider.value <= 100)
        {
            currentSushiPhase = 3;
            sushiRiceWithSeaWeedMainPlate.GetComponent<Image>().sprite = sushiSprite4;
            sushiSlider.SetActive(false);
            StartCoroutine(PopCompleted(0));
        }
    }

    IEnumerator PopCompleted(int food)
    {
        switch (food)
        {
            case 0:
                foodCompletionMark.SetActive(true);
                yield return new WaitForSecondsRealtime(1f);
                sushiSlider.GetComponent<Slider>().value = 0;
                foodCompletionMark.SetActive(false);
                sushiRiceWithSeaWeedMainPlate.SetActive(false);
                sushiPanel.SetActive(false);
                isCooking = false;
                break;
            case 1:
                foodCompletionMark.SetActive(true);
                yield return new WaitForSecondsRealtime(1f);
                foodCompletionMark.SetActive(false);
                foreach (var ys in ysSliders)
                {
                    ys.GetComponent<Slider>().value = 0;
                    ys.SetActive(false);
                }
                ysSliders[0].SetActive(true);
                for (int i = 0; i < ysEggMainPlate.Count; i++)
                {
                    ysEggMainPlate[i].SetActive(false);
                }
                ysLettuceMainPlate.GetComponent<Image>().sprite = ysLettuceSprites[0];
                ysLettuceMainPlate.SetActive(false);
                ysPanel.SetActive(false);
                isCooking = false;
                break;
            case 2:
                foodCompletionMark.SetActive(true);
                yield return new WaitForSecondsRealtime(1f);
                foodCompletionMark.SetActive(false);
                foreach(var kiyma in LazanyaKiymaMainPlate)
                {
                    kiyma.SetActive(false);
                }
                foreach (var borek in LazanyaBoreklerMainPlate)
                {
                    borek.SetActive(false);
                }
                LazanyaPanel.SetActive(false);
                isCooking = false;
                break;
        } 

    }
}
