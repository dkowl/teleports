using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ErrorHandler  {

    public static void ReportError(string message)
    {
        Debug.LogWarningFormat("Error: {0}", message);
    }
}
