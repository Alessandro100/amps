using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomecoreLocationController : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // will have to add a check if the player can afford
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.Instance.AddHomecore(transform.position);
            GameManager.Instance.PurchaseObject("homecore");
            Destroy(this.gameObject);
        }
    }
}
