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

    [SerializeField] GameObject heapMethod;
    [SerializeField] GameObject quickMethod;

    public List<CancerIncidence> incidences;



        /*
    public class CancerIncidence // CancerIncidence class derived from Bridges class definition, converted to C# by Patrick
    {
        [SerializeField] double age_adjusted_rate;       // expected cancer rate, adjusted for age
        [SerializeField] double[] age_adjusted_rate_ci = new double[2];  // confidence interval-lower,upper
        [SerializeField] double crude_rate;              // cancer rate adjusted by population
        [SerializeField] double[] crude_rate_ci = new double[2];     // confidence interval

        [SerializeField] int count;                  // incidence count
        [SerializeField] int year;                   // reporting year
        [SerializeField] int population;             // population of this area

        [SerializeField] string gender;               // gender (male, female, male and female
        [SerializeField] string race;
        [SerializeField] string event_type;               // incidence, mortality
        [SerializeField] string affected_area;            // location, typically, state

        [SerializeField] double[] loc = new double[2];           // location (cartesian coords)

        public CancerIncidence()
        {
            loc[0] = loc[1] = 0.0;
            age_adjusted_rate_ci[0] = age_adjusted_rate_ci[1] = 0.0;
            crude_rate_ci[0] = crude_rate_ci[1] = 0.0;
        }

        public double getAgeAdjustedRate() { return age_adjusted_rate; }
        public void setAgeAdjustedRate(double aar){ age_adjusted_rate = aar; }

        public double getAgeAdjustedCI_Lower() { return age_adjusted_rate_ci[0]; }
        public void setAgeAdjustedCI_Lower(double ci_l) { age_adjusted_rate_ci[0] = ci_l; }

        public double getAgeAdjustedCI_Upper() { return age_adjusted_rate_ci[1]; }
        public void setAgeAdjustedCI_Upper(double ci_u) { age_adjusted_rate_ci[1] = ci_u; }

        public double getCrudeRate() { return crude_rate; }
        public void setCrudeRate(double cr) { crude_rate = cr; }

        public double getCrudeRate_CI_Lower() { return crude_rate_ci[0]; }
        public void setCrudeRate_CI_Lower(double cr_l) { crude_rate_ci[0] = cr_l; }

        public double getCrudeRate_CI_Upper() { return crude_rate_ci[1]; }
        public void setCrudeRate_CI_Upper(double cr_u) { crude_rate_ci[1] = cr_u; }

        public int getYear() { return year; }
        public void setYear(int y) { year = y; }

        public string getGender() { return gender; }
        public void setGender(string g) { gender = g; }

        public string getRace() { return race; }
        public void setRace(string r) { race = r; }

        public string getEventType() { return event_type; }
        public void setEventType(string et) { event_type = et; }

        public int getPopulation() { return population; }
        public void setPopulation(int pop) { population = pop; }

        public string getAffectedArea() { return affected_area; }
        public void setAffectedArea(string area) { affected_area = area; }

        public int getCount() { return count; }
        public void setCount(int c) { count = c; }

        public double getLocationX() { return loc[0]; }
        public void setLocationX(double locX) { loc[0] = locX; }

        public double getLocationY() { return loc[1]; }
        public void setLocationY(double locY) { loc[1] = locY; }
    }
    */

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

            //Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }
    }
}
