using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class destroy_after_animation : MonoBehaviour
{
    void DestroyItself()
    {
        if(gameObject.tag == "EnemySlash")
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
      
        
    }
}
