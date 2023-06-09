using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    void Throw(Vector3 landingTarget, float throwSpeed);
}
