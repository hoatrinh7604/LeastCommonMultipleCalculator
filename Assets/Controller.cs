using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    [SerializeField] Button addButton;
    [SerializeField] Button continueButton;

    [SerializeField] TMP_InputField numbers;

    [SerializeField] GameObject[] listItemResults;

    [SerializeField] TextMeshProUGUI GCFText;
    [SerializeField] TextMeshProUGUI GCFResultText;

    [SerializeField] GameObject listFieldParent;

    [SerializeField] GameObject numberPrefab;
    [SerializeField] GameObject GCFObj;

    [SerializeField] Button calButton;

    private List<GameObject> list = new List<GameObject>();
    private List<double> listInt = new List<double>();
    private int amount = 0;

    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    private int type = 1;

    [SerializeField] Color[] listColor;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();
    }

    private void DropdownitemSelected()
    {
        
    }

    private void DropdownitemAgeSelected()
    {
        
    }


    public void OnValueChanged()
    {
        if(CheckValidate())
        {
            calButton.interactable = true;
        }
        else
        {
            calButton.interactable= false;
        }
    }

    List<int> listNumbers = new List<int>();
    private bool CheckValidate()
    {
        listNumbers.Clear();
        if (numbers.text == "")
        {
            return false;
        }

        try
        {
            string[] arrayStr = numbers.text.Split(new char[] { ',' });
            foreach(var item in arrayStr)
            {
                try
                {
                    listNumbers.Add(int.Parse(item));
                }catch(Exception e)
                {
                    return false;
                }
            }
        }catch (Exception e)
        {
            return false;
        }
        if(listNumbers.Count > 10) return false;
        //return text.All(char.IsDigit);
        return true;
    }


    public void Sum()
    {
        CalWithAdult();
        listFieldParent.SetActive(true);
    }

    private void CalWithAdult()
    {
        List<List<int>> listprimes = new List<List<int>>();
        foreach (var root_m in listNumbers)
        {
            // find factors
            List<int> listFactors = new List<int>();
            for (int i = 1; i < root_m; i++)
            {
                if (root_m % i == 0)
                {
                    listFactors.Add(i);
                }
            }

            // Find prime
            List<int> listPrime = new List<int>();
            foreach (var item in listFactors)
            {
                if (CalcIsPrime(item))
                {
                    listPrime.Add(item);
                }
            }

            int temp = root_m;
            List<int> primeFactors = new List<int>();
            for (int i = 0; i < listPrime.Count; i++)
            {
                while (temp % listPrime[i] == 0)
                {
                    temp = temp / listPrime[i];
                    primeFactors.Add(listPrime[i]);
                }
            }

            listprimes.Add(primeFactors);

            string temp1 = root_m + "=";
            for (int i = 0; i < primeFactors.Count; i++)
            {
                temp1 += primeFactors[i];

                if (i != primeFactors.Count - 1)
                {
                    temp1 += "x";
                }
            }

            GameObject number_m = Instantiate(numberPrefab, Vector3.zero, Quaternion.identity, listFieldParent.transform);
            number_m.GetComponent<TextMeshProUGUI>().text = temp1;
        }

        GCFObj.transform.SetAsLastSibling();

        int GCF = 1;
        GCFText.text = "LCM=";
        if(listprimes.Count == 0)
        {
            GCFText.text += GCF;
            return;
        }

        List<int> listResult = new List<int>();

        for (int pr = 0; pr < listprimes.Count; pr++)
        {
            for (int i = 0; i < listprimes[pr].Count; i++)
            {
                listResult.Add(listprimes[pr][i]);

                for (int j = pr + 1; j < listprimes.Count; j++)
                {
                    int index = listprimes[j].IndexOf(listprimes[pr][i]);
                    if(index >= 0)
                        listprimes[j].RemoveAt(index);
                }
            }
        }

        listResult.Sort();
        for (int i= 0; i< listResult.Count; i++)
        {
            GCF *= listResult[i];
            GCFText.text += listResult[i];
            if (i < listResult.Count - 1)
            {
                GCFText.text += "x";
            }
        }
        GCFResultText.text = GCF.ToString();
    }

    public bool CalcIsPrime(int number)
    {

        if (number == 1) return false;
        if (number == 2) return true;

        if (number % 2 == 0) return false; // Even number     

        for (int i = 2; i < number; i++)
        { // Advance from two to include correct calculation for '4'
            if (number % i == 0) return false;
        }

        return true;

    }

    public void Continue()
    {
        if(amount==0) return;
        Clear();
    }

    public void Clear()
    {
        listFieldParent.SetActive(false);

        numbers.text = "";
        GCFText.text = "";
        GCFResultText.text = "";

        calButton.interactable = false;
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
