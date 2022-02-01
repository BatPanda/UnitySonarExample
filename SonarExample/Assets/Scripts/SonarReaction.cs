using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarReaction : MonoBehaviour
{
    [SerializeField] private string objectName;

    public string getName() => objectName;
}
