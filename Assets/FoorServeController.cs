using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoorServeController : MonoBehaviour
{
    [Header("CircleCast")]
    [SerializeField] float circleRadius = 0.5f;
    [SerializeField] LayerMask checkLayers;
    [SerializeField] CatMovement currentCat;
    [SerializeField] FoodMenuManager.FoodList selectedFood;


    [SerializeField] bool isTrash;
    [SerializeField] bool isCustomer;

    [SerializeField] TextMeshProUGUI interactText;
    [SerializeField] Color serveColor;
    [SerializeField] Color discardColor;
    [SerializeField] FoodMenuManager.FoodList availableFood;
    [SerializeField] bool nearToKitchen;
    [SerializeField] FoodManager foodManager;
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
                    interactText.SetText("E To Serve");
                }
                else
                {
                    interactText.SetText("");
                }
            }

            else if (_collider.CompareTag("Trash"))
            {
                isTrash = true;
                if (selectedFood != FoodMenuManager.FoodList.None)
                {
                    interactText.color = discardColor;
                    interactText.SetText("E To Discard");
                }
                else
                {
                    interactText.SetText("");
                }

            }
            
            if (_collider.CompareTag("Sushi"))
            {
                
                interactText.color = discardColor;
                availableFood = FoodMenuManager.FoodList.Sushi;
                nearToKitchen = true;
                if(selectedFood == FoodMenuManager.FoodList.None)
                {
                    interactText.SetText("E To SUSHI");
                }
                else
                {
                    interactText.SetText("");
                }
            }
            else if (_collider.CompareTag("EggSalad"))
            {
                availableFood = FoodMenuManager.FoodList.EggSalad;
                interactText.color = discardColor;
                
                nearToKitchen = true;
                if (selectedFood == FoodMenuManager.FoodList.None)
                {
                    interactText.SetText("E To EGG SALAD");
                }
                else
                {
                    interactText.SetText("");
                }
            }
            else if (_collider.CompareTag("Lasagna"))
            {
                interactText.color = discardColor;
                
                availableFood = FoodMenuManager.FoodList.Lasagna;
                nearToKitchen = true;
                if (selectedFood == FoodMenuManager.FoodList.None)
                {
                    interactText.SetText("E To LASAGNA");
                }
                else
                {
                    interactText.SetText("");
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
            nearToKitchen = false;
            isTrash = false;
            availableFood = FoodMenuManager.FoodList.None;
            interactText.SetText("");
        }

    }


    void ServeTheCat()
    {
        if (isCustomer && selectedFood != FoodMenuManager.FoodList.None)
        {
            currentCat.FoodServed(selectedFood);
            selectedFood = FoodMenuManager.FoodList.None;
        }
    }

    void ThrowToTrash()
    {
        if (isTrash)
        {
            selectedFood = FoodMenuManager.FoodList.None;
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
                    foodManager.StartMakingSushi();
                    break;
                case 2:
                    foodManager.StartMakingYS();
                    break;
                case 3:
                    foodManager.StartMakingLazanya();
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        CheckCircle();
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            ServeTheCat();
            ThrowToTrash();
            GetTheFood();
        }
    }
}
