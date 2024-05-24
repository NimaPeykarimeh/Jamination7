using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FoorServeController : MonoBehaviour
{
    AudioSource audioSource;
    MoneyManager moneyManager;
    [SerializeField] AudioClip catServedAdudio;
    [Header("CircleCast")]
    [SerializeField] float circleRadius = 0.5f;
    [SerializeField] LayerMask checkLayers;
    [SerializeField] CatMovement currentCat;
    [SerializeField] FoodMenuManager.FoodList selectedFood;


    [SerializeField] bool isTrash;
    [SerializeField] bool isCustomer;

    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] TextMeshProUGUI quickCook;
    [SerializeField] Color serveColor;
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color discardColor;
    [Space]

    [SerializeField] Color quickCookYesColor;
    [SerializeField] Color quickCookNoColor;

    [Space]
    [SerializeField] FoodMenuManager.FoodList availableFood;
    [SerializeField] bool nearToKitchen;
    [SerializeField] FoodManager foodManager;
    [SerializeField] TableManager currentTable;
    [SerializeField] SpriteRenderer plate;
    [SerializeField] int instantCookPrice = 20;

    [Header("InstantTimers")]
    [SerializeField] float quickCockTimer = 30f;
    [SerializeField] float sushiInsTimer;
    [SerializeField] float lasagnaInsTimer;
    [SerializeField] float saladInsTimer;

    [SerializeField] bool canInsSushi;
    [SerializeField] bool canInsLasagna;
    [SerializeField] bool canInsSalad;

    [SerializeField] Sprite saladSprite;
    [SerializeField] Sprite sushiSprite;
    [SerializeField] Sprite lasagnaSprite;

    [Header("InstantCookUI")]
    [SerializeField] RectTransform instantCookBG;
    [SerializeField] RectTransform instantCookFillBar;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    void CheckCircle()
    {
        Collider2D _collider = Physics2D.OverlapCircle(transform.position, circleRadius, checkLayers);
        if (_collider)
        {
            if (_collider.CompareTag("Customer"))
            {
                isTrash = false;

                currentCat = _collider.GetComponent<CatMovement>();
                isCustomer = currentCat.currentState == CatMovement.CatStages.WaitingForOrder;
                if (isCustomer && selectedFood != FoodMenuManager.FoodList.None)
                {
                    interactText.color = serveColor;
                    interactText.SetText("Serve");
                    //interactText.SetText("E To Serve");
                }
                else
                {
                    interactText.SetText("");
                    quickCook.SetText("");
                }
            }

            else if (_collider.CompareTag("Trash"))
            {
                isTrash = true;
                if (selectedFood != FoodMenuManager.FoodList.None)
                {
                    interactText.color = discardColor;
                    interactText.SetText("Discard");
                    //interactText.SetText("E To Discard");
                }
                else
                {
                    interactText.SetText("");
                    quickCook.SetText("");
                }

            }

            else if (_collider.CompareTag("BuyTable"))
            {
                currentTable = _collider.transform.parent.GetComponent<TableManager>();
                interactText.color = discardColor;
                interactText.SetText("Buy");
                //interactText.SetText("E To Buy");
            }
            else
            {
                currentTable = null;
            }

            if (_collider.CompareTag("Sushi"))
            {
                
                interactText.color = defaultColor;
                availableFood = FoodMenuManager.FoodList.Sushi;
                nearToKitchen = true;
                if(selectedFood == FoodMenuManager.FoodList.None)
                {
                    //interactText.SetText("E To Cook");
                    interactText.SetText("Cook");
                    if (canInsSushi)
                    {
                        instantCookBG.gameObject.SetActive(false);
                        quickCook.color = quickCookYesColor;
                    }
                    else
                    {
                        instantCookBG.gameObject.SetActive(true);
                        instantCookFillBar.sizeDelta = new Vector2(instantCookBG.sizeDelta.x * (1 - sushiInsTimer / quickCockTimer),instantCookBG.sizeDelta.y);
                        quickCook.color = quickCookNoColor;
                    }
                    quickCook.SetText("Instant Cook($" + instantCookPrice + ")");
                    //quickCook.SetText("Q To Instant Cook($" + instantCookPrice + ")");
                }
                else
                {
                    interactText.SetText("");
                    quickCook.SetText("");
                }
            }
            else if (_collider.CompareTag("EggSalad"))
            {
                availableFood = FoodMenuManager.FoodList.EggSalad;
                interactText.color = defaultColor;
                
                nearToKitchen = true;
                if (selectedFood == FoodMenuManager.FoodList.None)
                {
                    //interactText.SetText("E To Cook");
                    interactText.SetText("Cook");
                    if (canInsSalad)
                    {
                        instantCookBG.gameObject.SetActive(false);
                        quickCook.color = quickCookYesColor;
                    }
                    else
                    {
                        instantCookBG.gameObject.SetActive(true);
                        instantCookFillBar.sizeDelta = new Vector2(instantCookBG.sizeDelta.x * (1 - saladInsTimer / quickCockTimer), instantCookBG.sizeDelta.y);

                        quickCook.color = quickCookNoColor;
                    }
                    quickCook.SetText("Instant Cook($" + instantCookPrice + ")");
                    //quickCook.SetText("Q To Instant Cook($" + instantCookPrice + ")");
                }
                else
                {
                    interactText.SetText("");
                    quickCook.SetText("");
                }
            }
            else if (_collider.CompareTag("Lasagna"))
            {
                interactText.color = defaultColor;
                
                availableFood = FoodMenuManager.FoodList.Lasagna;
                nearToKitchen = true;
                if (selectedFood == FoodMenuManager.FoodList.None)
                {
                    if (canInsLasagna)
                    {
                        instantCookBG.gameObject.SetActive(false);
                        quickCook.color = quickCookYesColor;
                    }
                    else
                    {
                        instantCookBG.gameObject.SetActive(true);
                        instantCookFillBar.sizeDelta = new Vector2(instantCookBG.sizeDelta.x * (1 - lasagnaInsTimer / quickCockTimer), instantCookBG.sizeDelta.y);
                        quickCook.color = quickCookNoColor;
                    }
                    interactText.SetText("Cook");
                    //interactText.SetText("E To Cook");
                    quickCook.SetText("Instant Cook($" + instantCookPrice + ")");
                    //quickCook.SetText("Q To Instant Cook($" + instantCookPrice + ")");
                }
                else
                {
                    instantCookBG.gameObject.SetActive(false);
                    interactText.SetText("");
                    quickCook.SetText("");
                }

            }
            else
            {
                    
                availableFood = FoodMenuManager.FoodList.None;
                nearToKitchen = false;
            }
            

            

        }
        else
        {
            currentTable = null;
            nearToKitchen = false;
            isTrash = false;
            availableFood = FoodMenuManager.FoodList.None;
            instantCookBG.gameObject.SetActive(false);
            interactText.SetText("");
            quickCook.SetText("");
        }

    }

    void CookdInsTimers()
    {
        lasagnaInsTimer -= Time.deltaTime;
        sushiInsTimer -= Time.deltaTime;
        saladInsTimer -= Time.deltaTime;

        if (saladInsTimer <= 0)
        {
            canInsSalad = true;
        }
        if (sushiInsTimer <= 0)
        {
            canInsSushi = true;
        }
        if (lasagnaInsTimer <= 0)
        {
            canInsLasagna = true;
        }
    }

    void ServeTheCat()
    {
        if (isCustomer && selectedFood != FoodMenuManager.FoodList.None)
        {
            currentCat.FoodServed(selectedFood);
            plate.gameObject.SetActive(false);
            selectedFood = FoodMenuManager.FoodList.None;
        }
    }

    void ThrowToTrash()
    {
        if (isTrash)
        {
            plate.gameObject.SetActive(false);
            selectedFood = FoodMenuManager.FoodList.None;
        }
    }

    void BuyTable()
    {
        if (currentTable != null)
        {
            currentTable.BuyTable();
        }
    }

    void GetTheFood()
    {
        if (nearToKitchen && selectedFood == FoodMenuManager.FoodList.None)
        {
            selectedFood = availableFood;
            switch ((int)selectedFood) 
            {
                case 1:
                    plate.gameObject.SetActive(true);
                    plate.sprite = sushiSprite;
                    foodManager.StartMakingSushi();
                    break;
                case 2:
                    plate.gameObject.SetActive(true);
                    plate.sprite = saladSprite;
                    foodManager.StartMakingYS();
                    break;
                case 3:
                    plate.gameObject.SetActive(true);
                    plate.sprite = lasagnaSprite;
                    foodManager.StartMakingLazanya();
                    break;
                default:
                    break;
            }
        }
    }

    void InstantCook()
    {
        if (nearToKitchen && selectedFood == FoodMenuManager.FoodList.None)
        {
            
            if (availableFood == FoodMenuManager.FoodList.Lasagna && canInsLasagna && moneyManager.SpendMoney(instantCookPrice))
            {
                plate.gameObject.SetActive(true);
                lasagnaInsTimer = quickCockTimer;
                canInsLasagna = false;
                plate.sprite = lasagnaSprite;
                selectedFood = availableFood;
            }
            else if (availableFood == FoodMenuManager.FoodList.Sushi && canInsSushi && moneyManager.SpendMoney(instantCookPrice))
            {
                plate.gameObject.SetActive(true);
                sushiInsTimer = quickCockTimer;
                canInsSushi = false;
                plate.sprite = sushiSprite;
                selectedFood = availableFood;
            }
            else if (availableFood == FoodMenuManager.FoodList.EggSalad && canInsSalad && moneyManager.SpendMoney(instantCookPrice))
            {
                plate.gameObject.SetActive(true);
                saladInsTimer = quickCockTimer;
                canInsSalad = false;
                plate.sprite = saladSprite;
                selectedFood = availableFood;
            }
            

        }
    }

    public void MainAction()
    {
        ServeTheCat();
        ThrowToTrash();
        GetTheFood();
        
    }

    public void SecondaryAction()
    {
        InstantCook();
        BuyTable();
    }

    private void Update()
    {
        CheckCircle();
        CookdInsTimers();

        if (Input.GetKeyDown(KeyCode.E))
        {
            MainAction();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SecondaryAction();

        }
    }
}
