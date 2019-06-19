using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;

public class Converter : MonoBehaviour
{
    public Dropdown sourceDropdown;
    public Dropdown endDropdown;
    public Dropdown dayDropdown;
    public Dropdown monthDropdown;
    public Dropdown yearDropdown;
    public InputField amount;
    public Text result;

    List<string> currencies = new List<string> { "EUR", "BGN", "NZD", "ILS", "RUB", "CAD", "USD", "PHP", "CHF", "ZAR", "AUD", "JPY", "TRY", "HKD", "MYR", "THB", "HRK", "NOK", "CZK", "GBP", "MXN", "BRL", "PLN", "CNY", "SEK", "RON" };
    List<string> days = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28" };
    List<string> months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
    List<string> years = new List<string> { "2019", "2018", "2017", "2016", "2015", "2014", "2013", "2012", "2011", "2010", "2009", "2008", "2007", "2006", "2005", "2004", "2003", "2002", "2001", "2000", "1999" };

    // Start is called before the first frame update
    void Start()
    {
        sourceDropdown.AddOptions(currencies);
        endDropdown.AddOptions(currencies);
        dayDropdown.AddOptions(days);
        monthDropdown.AddOptions(months);
        yearDropdown.AddOptions(years);

        endDropdown.value = 6;
    }

    public void convert()
    {
        string source = currencies[sourceDropdown.value];
        string end = currencies[endDropdown.value];

        int day = dayDropdown.value + 1;
        int month = monthDropdown.value + 1;
        string year = years[yearDropdown.value];
        string fullDate = string.Format("{0}-{1}-{2}", year, month.ToString(), day.ToString());

        string url = string.Format("https://api.exchangeratesapi.io/{0}?base={1}&symbols={2}", fullDate, source, end);

        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error: " + uwr.error);
        }
        else
        {
            var json = JSON.Parse(uwr.downloadHandler.text);
            var exchangeRate = json["rates"][currencies[endDropdown.value]].Value;

            float resultNumber = float.Parse(exchangeRate) * float.Parse(amount.text);

            result.text = amount.text + currencies[sourceDropdown.value] + " = " + resultNumber.ToString() + currencies[endDropdown.value];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
