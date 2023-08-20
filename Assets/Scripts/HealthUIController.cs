using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUIController : MonoBehaviour
{
    public GameObject heartContainer;
    private float fillValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fillValue = (float)gameController.Health;
        fillValue = fillValue / gameController.MaxHelth;
        heartContainer.GetComponent<Image>().fillAmount = fillValue;
    }
}
