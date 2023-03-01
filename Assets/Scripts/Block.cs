using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    [SerializeField] Text HPText;
    [SerializeField] int HPValue;
    private void Start()
    {
        UpdateUI();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HPValue--;
        if (HPValue <= 0)
            this.gameObject.SetActive(false);
        UpdateUI();
    }
    void UpdateUI()
    {
        HPText.text = HPValue.ToString();
    }
}
