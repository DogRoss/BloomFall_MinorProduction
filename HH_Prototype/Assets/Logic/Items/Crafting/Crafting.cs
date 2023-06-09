using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Crafting : object
{
    [SerializeField] static List<Recipe> recipes = new List<Recipe>();
    [SerializeField] static List<GameObject> recipeObjects;

    public static List<Recipe> Recipes
    {
        get
        {
            return recipes;
        }

        set
        {
            recipes = value;
        }
    }
    public static List<GameObject> RecipeObjects
    {
        get
        {
            return recipeObjects;
        }

        set
        {
            recipeObjects = value;
        }
    }



    public static void SetUpRecipes(List<Recipe> _recipes)
    {
        recipes = recipes;

        foreach (Recipe recipe in recipes)
        {
            recipeObjects.Add(recipe.resultingObject);
        }
    }



    public static Recipe FindRecipe(GameObject o)
    {
        Recipe recipe = null;
        foreach(Recipe _recipe in recipes)
        {
            if (_recipe.resultingObject == o)
            {
                recipe = _recipe;
                break;
            }
        }

        return recipe;
    }
    public static Recipe FindRecipe(int index)
    {
        return recipes[index];
    }

    public static bool CheckRecipe(GameObject o)
    {
        Recipe recipe = FindRecipe(o);

        return CheckRecipe(recipe);
    }

    public static bool CheckRecipe(Recipe _recipe)
    {
        bool craftable = true;
        foreach(Recipe.Ingredient ingredient in _recipe.ingredients)
        {
            bool breakLoop = false;
            switch (ingredient.ingredientType)
            {
                case Recipe.Ingredient.IngredientType.Bullet:
                    if(Inventory.instance.BulletAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
                case Recipe.Ingredient.IngredientType.Trap:
                    if (Inventory.instance.TrapAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
                case Recipe.Ingredient.IngredientType.Gasoline:
                    if (Inventory.instance.GasolineAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
                case Recipe.Ingredient.IngredientType.Metal:
                    if (Inventory.instance.MetalAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;

                case Recipe.Ingredient.IngredientType.Bottle:
                    if (Inventory.instance.BottleAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;

                case Recipe.Ingredient.IngredientType.Cloth:
                    if (Inventory.instance.ClothAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;

                case Recipe.Ingredient.IngredientType.Wood:
                    if (Inventory.instance.WoodAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
            }

            if (breakLoop)
                break;
        }

        return craftable;
    }
    //public static bool CheckRecipe(Recipe _recipe, out GameObject resultObj)
    //{
    //    bool craftable = true;
    //    foreach (Recipe.Ingredient ingredient in _recipe.ingredients)
    //    {
    //        bool breakLoop = false;
    //        switch (ingredient.ingredientType)
    //        {
    //            case Recipe.Ingredient.IngredientType.Bullet:
    //                if (InventoryUILinker.Inventory.BulletAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //            case Recipe.Ingredient.IngredientType.Trap:
    //                if (InventoryUILinker.Inventory.TrapAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //            case Recipe.Ingredient.IngredientType.Gasoline:
    //                if (InventoryUILinker.Inventory.GasolineAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //            case Recipe.Ingredient.IngredientType.Metal:
    //                if (InventoryUILinker.Inventory.MetalAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //
    //            case Recipe.Ingredient.IngredientType.Bottle:
    //                if (InventoryUILinker.Inventory.BottleAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //
    //            case Recipe.Ingredient.IngredientType.Cloth:
    //                if (InventoryUILinker.Inventory.ClothAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //
    //            case Recipe.Ingredient.IngredientType.Wood:
    //                if (InventoryUILinker.Inventory.WoodAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //        }
    //
    //        if (breakLoop)
    //            break;
    //    }
    //
    //    if (craftable)
    //    {
    //        switch (ingredient.ingredientType)
    //        {
    //            case Recipe.Ingredient.IngredientType.Bullet:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 0);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Trap:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 2);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Gasoline:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 3);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Metal:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 4);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Bottle:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 5);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Cloth:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 6);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Wood:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 7);
    //                break;
    //        }
    //
    //        resultObj = _recipe.resultingObject;
    //    }
    //
    //    return craftable;
    //}

    public static bool CheckRecipe(int index)
    {
        Debug.Log("enter Check Recipe by int");

        bool craftable = true;
        foreach (Recipe.Ingredient ingredient in recipes[index].ingredients)
        {
            bool breakLoop = false;
            switch (ingredient.ingredientType)
            {
                case Recipe.Ingredient.IngredientType.Bullet:
                    if (Inventory.instance.BulletAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
                case Recipe.Ingredient.IngredientType.Trap:
                    if (Inventory.instance.TrapAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
                case Recipe.Ingredient.IngredientType.Gasoline:
                    if (Inventory.instance.GasolineAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
                case Recipe.Ingredient.IngredientType.Metal:
                    if (Inventory.instance.MetalAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;

                case Recipe.Ingredient.IngredientType.Bottle:
                    if (Inventory.instance.BottleAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;

                case Recipe.Ingredient.IngredientType.Cloth:
                    if (Inventory.instance.ClothAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;

                case Recipe.Ingredient.IngredientType.Wood:
                    if (Inventory.instance.WoodAmount < ingredient.amount)
                    {
                        breakLoop = true;
                        craftable = false;
                    }
                    break;
            }

            if (breakLoop)
                break;
        }

        Debug.Log("is craftable: " + craftable);
        return craftable;
    }
    //public static bool CheckRecipe(int index, out GameObject resultObj)
    //{
    //    bool craftable = true;
    //    foreach (Recipe.Ingredient ingredient in recipes[index].ingredients)
    //    {
    //        bool breakLoop = false;
    //        switch (ingredient.ingredientType)
    //        {
    //            case Recipe.Ingredient.IngredientType.Bullet:
    //                if (InventoryUILinker.Inventory.BulletAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //            case Recipe.Ingredient.IngredientType.Trap:
    //                if (InventoryUILinker.Inventory.TrapAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //            case Recipe.Ingredient.IngredientType.Gasoline:
    //                if (InventoryUILinker.Inventory.GasolineAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //            case Recipe.Ingredient.IngredientType.Metal:
    //                if (InventoryUILinker.Inventory.MetalAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //
    //            case Recipe.Ingredient.IngredientType.Bottle:
    //                if (InventoryUILinker.Inventory.BottleAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //
    //            case Recipe.Ingredient.IngredientType.Cloth:
    //                if (InventoryUILinker.Inventory.ClothAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //
    //            case Recipe.Ingredient.IngredientType.Wood:
    //                if (InventoryUILinker.Inventory.WoodAmount < ingredient.amount)
    //                {
    //                    breakLoop = true;
    //                    craftable = false;
    //                }
    //                break;
    //        }
    //
    //        if (breakLoop)
    //            break;
    //    }
    //
    //    if (craftable)
    //    {
    //        switch (ingredient.ingredientType)
    //        {
    //            case Recipe.Ingredient.IngredientType.Bullet:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 0);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Trap:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 2);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Gasoline:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 3);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Metal:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 4);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Bottle:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 5);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Cloth:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 6);
    //                break;
    //            case Recipe.Ingredient.IngredientType.Wood:
    //                InventoryUILinker.Inventory.AddAmountToItem(-ingredient.amount, 7);
    //                break;
    //        }
    //
    //        resultObj = _recipe.resultingObject;
    //    }
    //
    //    return craftable;
    //}



    public static void DecrementIngredients(Recipe _recipe)
    {
        foreach (Recipe.Ingredient ingredient in _recipe.ingredients)
        {
            switch (ingredient.ingredientType)
            {
                case Recipe.Ingredient.IngredientType.Bullet:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 0);
                    break;
                case Recipe.Ingredient.IngredientType.Trap:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 2);
                    break;
                case Recipe.Ingredient.IngredientType.Gasoline:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 3);
                    break;
                case Recipe.Ingredient.IngredientType.Metal:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 4);
                    break;
                case Recipe.Ingredient.IngredientType.Bottle:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 5);
                    break;
                case Recipe.Ingredient.IngredientType.Cloth:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 6);
                    break;
                case Recipe.Ingredient.IngredientType.Wood:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 7);
                    break;
            }
        }
    }
    public static void DecrementIngredients(int recipeIndex)
    {
        foreach (Recipe.Ingredient ingredient in recipes[recipeIndex].ingredients)
        {
            switch (ingredient.ingredientType)
            {
                case Recipe.Ingredient.IngredientType.Bullet:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 0);
                    break;
                case Recipe.Ingredient.IngredientType.Trap:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 2);
                    break;
                case Recipe.Ingredient.IngredientType.Gasoline:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 3);
                    break;
                case Recipe.Ingredient.IngredientType.Metal:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 4);
                    break;
                case Recipe.Ingredient.IngredientType.Bottle:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 5);
                    break;
                case Recipe.Ingredient.IngredientType.Cloth:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 6);
                    break;
                case Recipe.Ingredient.IngredientType.Wood:
                    Inventory.instance.AddAmountToItem(-ingredient.amount, 7);
                    break;
            }
        }
    }



    public static GameObject GetRecipeObject(int index)
    {
        return recipes[index].resultingObject;
    }
}
