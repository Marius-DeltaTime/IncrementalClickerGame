using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public TMP_Text txtEnergy;

    public float energyConsumption, energyProduction;

    public float kardashevValue, wattsConsumed;

    public Slider sliderKardashev;
    public TMP_Text textKardashev;
    private float _timer = 0.1f;
    // Do a update function where it ticks daily(Look at the season tick rate)
    // Where it takes the energyConsumed at that time i.e at the end of the day
    // And adds it to the total watts consumed.
    // But it should also be daily so it should be energyConsumption * 60 * 60 * 24
    private void Start()
    {
        // Earth's current is 20000000000000
        wattsConsumed = 20000000000000;
        kardashevValue = (Mathf.Log10(wattsConsumed) - 6) / 10;
        sliderKardashev.value = kardashevValue;
        textKardashev.text = string.Format("You are currently {0} on the Kardashev Scale.", kardashevValue);

        // So i'm just going to run this formula once every day so that it doesn't have to be constantly updated.
    }

    private void Update()
    {
        if ((_timer -= Time.deltaTime) <= 0)
        {
            _timer = 5f;

            wattsConsumed = 0;
            wattsConsumed = energyProduction;
            kardashevValue = (Mathf.Log10(wattsConsumed) - 6) / 10;
            sliderKardashev.value = kardashevValue;
            textKardashev.text = string.Format("You are currently {0} on the Kardashev Scale.", kardashevValue);
        }
    }
}
