using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadOffset : MonoBehaviour
{   
    [SerializeField] private GameObject gameObjectPai;
    public void offsetUpdate(Vector3 newOffset){
        transform.position = gameObjectPai.transform.position + newOffset;
    }
}
