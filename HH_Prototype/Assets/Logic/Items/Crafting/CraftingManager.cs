using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] List<Recipe> recipes = new List<Recipe>();

    // Start is called before the first frame update
    void Start()
    {
        Crafting.Recipes = recipes;
    }

    public void CraftWeaponAndUnlock(int recipeIndex)
    {
        Debug.Log("enter craft weapon and unlock");
        if (CheckRecipe(recipeIndex))
        {
            Debug.Log("Recipe Check is successful");
            ICraftable obj = GetRecipeObject(recipeIndex).GetComponent<ICraftable>();
            obj?.Craft();
            DecrementIngredients(recipeIndex);
        }

    }
    public void CraftWeaponAndEquip(int recipeIndex)
    {
        Debug.Log("enter craft weapon and unlock");
        if (CheckRecipe(recipeIndex))
        {
            Debug.Log("Recipe Check is successful");
            
            DecrementIngredients(recipeIndex);
        }
    }

    public void UnlockAndEquip(int recipeIndex)
    {
        ICraftable craftable = GetRecipeObject(recipeIndex).GetComponent<ICraftable>();
        IEquippable equippable = GetRecipeObject(recipeIndex).GetComponent<IEquippable>();

        craftable?.Craft();
        equippable?.Equip(true);
    }

    public void Unlock(int recipeIndex) 
    {
        ICraftable obj = GetRecipeObject(recipeIndex).GetComponent<ICraftable>();
        obj?.Craft();
    }

    //------------------------------------------------------------------------------------------------------------------------------------

    public Recipe FindRecipe(GameObject o)
    {
        return Crafting.FindRecipe(o);
    }
    public Recipe FindRecipe(int recipeIndex)
    {
        return Crafting.FindRecipe(recipeIndex);
    }



    public bool CheckRecipe(GameObject o)
    {
        return Crafting.CheckRecipe(o);
    }
    //public bool CheckRecipe(GameObject o, out GameObject resultObj)
    public bool CheckRecipe(Recipe _recipe)
    {
        return Crafting.CheckRecipe(_recipe);
    }
    //public bool CheckRecipe(Recipe _recipe, out GameObject resultObj)
    public bool CheckRecipe(int recipeIndex)
    {
        
        return Crafting.CheckRecipe(recipeIndex);
    }
    //public bool CheckRecipe(int index, out GameObject resultObj)



    public void DecrementIngredients(Recipe _recipe)
    {
        Crafting.DecrementIngredients(_recipe);
    }
    public void DecrementIngredients(int recipeIndex)
    {
        Crafting.DecrementIngredients(recipeIndex);
    }



    public GameObject GetRecipeObject(int recipeIndex)
    {
        return Crafting.GetRecipeObject(recipeIndex);
    }
}

