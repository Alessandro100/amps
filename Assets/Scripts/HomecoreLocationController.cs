using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomecoreLocationController : MonoBehaviour, IPointerClickHandler
{
    public GameObject homecorePrefab;

    // Start is called before the first frame update
    void Start()
    {
        print("start homecore location controller");
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            // in the future will have to put a popup
            Instantiate(homecorePrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
