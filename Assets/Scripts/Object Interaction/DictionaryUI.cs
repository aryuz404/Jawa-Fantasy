using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DictionaryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ngoko1Text;
    [SerializeField] private TextMeshProUGUI ngoko2Text;
    [SerializeField] private TextMeshProUGUI ngoko3Text;
    [SerializeField] private TextMeshProUGUI ngoko4Text;
    [SerializeField] private TextMeshProUGUI ngoko5Text;
    [SerializeField] private TextMeshProUGUI kramaMadya1Text;
    [SerializeField] private TextMeshProUGUI kramaMadya2Text;
    [SerializeField] private TextMeshProUGUI kramaMadya3Text;
    [SerializeField] private TextMeshProUGUI kramaMadya4Text;
    [SerializeField] private TextMeshProUGUI kramaMadya5Text;
    [SerializeField] private TextMeshProUGUI kramaInggil1Text;
    [SerializeField] private TextMeshProUGUI kramaInggil2Text;
    [SerializeField] private TextMeshProUGUI kramaInggil3Text;
    [SerializeField] private TextMeshProUGUI kramaInggil4Text;
    [SerializeField] private TextMeshProUGUI kramaInggil5Text;

    [SerializeField] private Button nextButton, previousButton;

    private DictionaryData dictionaryData;

    public static DictionaryUI instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDictionary(DictionaryData dictionaryData)
    {
        this.dictionaryData = dictionaryData;

        ngoko1Text.text = dictionaryData.ngoko1;
        ngoko2Text.text = dictionaryData.ngoko2;
        ngoko3Text.text = dictionaryData.ngoko3;
        ngoko4Text.text = dictionaryData.ngoko4;
        ngoko5Text.text = dictionaryData.ngoko5;
        kramaMadya1Text.text = dictionaryData.kmadya1;
        kramaMadya2Text.text = dictionaryData.kmadya2;
        kramaMadya3Text.text = dictionaryData.kmadya3;
        kramaMadya4Text.text = dictionaryData.kmadya4;
        kramaMadya5Text.text = dictionaryData.kmadya5;
        kramaInggil1Text.text = dictionaryData.kinggil1;
        kramaInggil2Text.text = dictionaryData.kinggil2;
        kramaInggil3Text.text = dictionaryData.kinggil3;
        kramaInggil4Text.text = dictionaryData.kinggil4;
        kramaInggil5Text.text = dictionaryData.kinggil5;
    }
}
