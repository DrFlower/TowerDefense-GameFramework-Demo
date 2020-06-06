using UnityEngine;
using System.Collections.Generic;

public class ListExample : MonoBehaviour
{
    //[HideInInspector] // we do this beacause we call base.OnInspectorGUI(); And we don't want that to draw the list aswell.
    public List<ListItemExample> list;
}