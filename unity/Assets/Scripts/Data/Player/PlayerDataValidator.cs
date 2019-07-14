using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataValidator : MonoBehaviour {

    public enum NameValidationResult
    {
        OK,
        TooShort,
        TooLong,
        IsNull
    }

    public static readonly int MinNameLength = 3;
    public static readonly int MaxNameLength = 24;

	public static NameValidationResult ValidateName(string name)
    {
        if(name == null)
        {
            Debug.LogWarning("Validated name is null");
            return NameValidationResult.IsNull;
        }
        else if(name.Length < MinNameLength)
        {
            return NameValidationResult.TooShort;
        }
        else if(name.Length > MaxNameLength)
        {
            return NameValidationResult.TooLong;
        }
        else
        {
            return NameValidationResult.OK;
        }
    }
}
