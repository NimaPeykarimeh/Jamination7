using UnityEngine;

public class FoodMenuManager : MonoBehaviour
{


    public enum FoodList
    {
        None,
        Sushi,
        EggSalad,
        Lasagna
    }

    public FoodList GetRandomFood()
    {
       return (FoodList)Random.Range(1, 4);

    }
}
