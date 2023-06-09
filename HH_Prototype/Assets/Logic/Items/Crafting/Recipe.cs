using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public GameObject resultingObject;
    public List<Ingredient> ingredients = new List<Ingredient>();
    

    [System.Serializable]
    public class Ingredient
    {
        public IngredientType ingredientType;
        public int amount = 0;

        public enum IngredientType
        {
            Bullet,
            Trap,
            Gasoline,
            Metal,
            Bottle,
            Cloth,
            Wood
        }
    }
}
