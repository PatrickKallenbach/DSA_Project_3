using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    public Tuple<double, double> location;

    public int numOfResults;
    public int year;
    Regex coordFormat;
    Regex resultsFormat;
    Regex yearFormat;

    bool coordsCheck;
    bool resultsCheck;
    bool yearCheck;
    bool heapOrQuick;

    [SerializeField] TMP_InputField coordX;
    [SerializeField] TMP_InputField coordY;
    [SerializeField] TMP_InputField resultsField;
    [SerializeField] TMP_InputField yearField;
    [SerializeField] GameObject coordsError;
    [SerializeField] GameObject resultsError;
    [SerializeField] GameObject yearError;
    [SerializeField] TextAsset incidencesTextFile;

    [SerializeField] GameObject processTime;

    [SerializeField] GameObject heapMethod;
    [SerializeField] GameObject quickMethod;

    public List<CancerIncidence> incidences;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1600, 1000, false);

        location = new Tuple<double, double>(0, 0); // source location
        numOfResults = 0; // user specified number of output results
        year = 0; // user specified year
        coordFormat = new Regex("^(\\-?\\d+(\\.\\d+)?)$"); // regex to accept only coordinate format
        // ^^ regex format found here: https://stackoverflow.com/questions/10686524/regex-for-latitude-longitude-pairs
        resultsFormat = new Regex("^([1-9]|[1-9][0-9]{1,3}|10000)$"); // regex to accept numbers from 1-10000
        yearFormat = new Regex("^(1999|200[0-9]|201[0-2])$"); // regex to accept numbers from 1900-2100

        heapOrQuick = true; // determines whether heap or quick sort is used

        var fullText = incidencesTextFile.text; // get cancer incidences from text file
        var data = fullText.Split("\n"); // split data into list

        incidences = new List<CancerIncidence>();

        for (int i = 0; i < 16512; i++) // for every item in the data
        {
            CancerIncidence incidence = new CancerIncidence();

            incidence.setAgeAdjustedRate(Convert.ToDouble(data[i * 15 + 0]));
            incidence.setAgeAdjustedCI_Lower(Convert.ToDouble(data[i * 15 + 1]));
            incidence.setAgeAdjustedCI_Upper(Convert.ToDouble(data[i * 15 + 2]));
            incidence.setCrudeRate(Convert.ToDouble(data[i * 15 + 3]));
            incidence.setCrudeRate_CI_Lower(Convert.ToDouble(data[i * 15 + 4]));
            incidence.setCrudeRate_CI_Upper(Convert.ToDouble(data[i * 15 + 5]));
            incidence.setYear(Convert.ToInt32(data[i * 15 + 6]));
            incidence.setGender(data[i * 15 + 7]);
            incidence.setRace(data[i * 15 + 8]);
            incidence.setEventType(data[i * 15 + 9]);
            incidence.setPopulation(Convert.ToInt32(data[i * 15 + 10]));
            incidence.setAffectedArea(data[i * 15 + 11]);
            incidence.setCount(Convert.ToInt32(data[i * 15 + 12]));
            incidence.setLocationX(Convert.ToDouble(data[i * 15 + 13]));
            incidence.setLocationY(Convert.ToDouble(data[i * 15 + 14]));
            // ^^ initialize incidence values

            incidences.Add(incidence);
            // add incidence to list
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void mode() // used to decide which method to use
    {
        heapOrQuick = !heapOrQuick;
    }

    public void run() // once start button is pressed
    {
        if (coordFormat.IsMatch(coordX.text) && coordFormat.IsMatch(coordY.text)) // if cordinate passes regex
        {
            location = new Tuple<double, double>((long)Convert.ToDouble(coordX.text), (long)Convert.ToDouble(coordY.text)); // set location
            coordsCheck = true;
            coordsError.SetActive(false);
        }
        else
        {
            coordsCheck = false;
            coordsError.SetActive(true);
        }

        if (resultsFormat.IsMatch(resultsField.text)) // if results regex passes
        {
            numOfResults = Convert.ToInt32(resultsField.text); // set num results
            resultsCheck = true;
            resultsError.SetActive(false);
        }
        else
        {
            resultsCheck = false;
            resultsError.SetActive(true);
        }

        if (yearFormat.IsMatch(yearField.text)) // if year regex passes
        {
            year = Convert.ToInt32(yearField.text); // set year
            yearCheck = true;
            yearError.SetActive(false);
        }
        else
        {
            yearCheck = false;
            yearError.SetActive(true);
        }

        if (coordsCheck && resultsCheck && yearCheck) // if all checks passed
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            for (int i = 0; i < heapMethod.GetComponent<HeapMethod>().incidencesDisplay.Count; i++) // delete gui incidence listings
            {
                Destroy(heapMethod.GetComponent<HeapMethod>().incidencesDisplay[i]);
            }
            heapMethod.GetComponent<HeapMethod>().incidencesDisplay.Clear();

            for (int i = 0; i < quickMethod.GetComponent<QuickSortMethod>().incidencesDisplay.Count; i++) // delete gui incidence listings
            {
                Destroy(quickMethod.GetComponent<QuickSortMethod>().incidencesDisplay[i]);
            }
            quickMethod.GetComponent<QuickSortMethod>().incidencesDisplay.Clear();

            if (heapOrQuick) // run heap method, or
            {
                heapMethod.GetComponent<HeapMethod>().ProcessData();
            }
            else // run quick method
            {
                quickMethod.GetComponent<QuickSortMethod>().ProcessData();
            }

            watch.Stop();

            processTime.SetActive(true);
            processTime.GetComponent<TextMeshProUGUI>().text = $"Completed in {watch.ElapsedMilliseconds} ms";
        }
    }
}
