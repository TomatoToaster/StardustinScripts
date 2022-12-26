using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargeMeter : MonoBehaviour
{
    private Player player;
    private TextMeshProUGUI chargeText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        chargeText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float chargeRaw = player.getCharge();
        bool isBulletAvailable = player.getAvailableBullets() > 0;

        chargeText.text = chargeRaw.ToString("F1");
        chargeText.color = isBulletAvailable ? new Color(0, 1, 0, 1) : new Color(1, 0, 0, 1);
    }
}
