using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModularActorVariables
{
    public static ModularActorVariables GlobalVariables
    {
        get
        {
            if (_globalVariablesInstance == null)
                _globalVariablesInstance = new ModularActorVariables();
            return _globalVariablesInstance;
        }
    }
    private static ModularActorVariables _globalVariablesInstance;

    public enum BooleanTags
    {
        Character_Grounded
    }
    public enum FloatTags
    {
        Character_Speed
    }

    #region Booleans
    private Dictionary<BooleanTags, bool> booleans;

    public void Set(BooleanTags tag, bool value)
    {
        if (booleans == null)
            booleans = new Dictionary<BooleanTags, bool>();

        booleans[tag] = value;
    }

    public bool Get(BooleanTags tag)
    {
        if (booleans == null || !booleans.ContainsKey(tag))
            return false;

        return booleans[tag];
    }
    #endregion

    #region Floats
    private Dictionary<FloatTags, bool> floats;

    public void Set(FloatTags tag, bool value)
    {
        if (floats == null)
            floats = new Dictionary<FloatTags, bool>();

        floats[tag] = value;
    }

    public bool Get(FloatTags tag)
    {
        if (floats == null || !floats.ContainsKey(tag))
            return false;

        return floats[tag];
    }
    #endregion

    public void Clear()
    {
        booleans = null;
    }
}
