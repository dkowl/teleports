﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMappedList<T> where T : IUniqueName {

    bool ContainsName(string name);

    IList<T> AllValues { get; }
    IList<string> AllNames { get; }
    T RandomValue { get; }
}
