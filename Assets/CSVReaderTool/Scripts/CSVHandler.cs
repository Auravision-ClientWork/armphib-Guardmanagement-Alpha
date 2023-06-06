using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// CSVReaderTool Namespace
/// </summary>
namespace CSVReaderTool
{
    /// <summary>
    /// This class provides methods to load CSV files, read data from them, and return that data in the form of 2D string arrays
    /// </summary>
    public class CSVHandler : MonoBehaviour
    {
        string path;
        [SerializeField]
        string testFileName;
        [SerializeField]
        Text textBox;
        // Start is called before the first frame update
        void Start()
        {
            if (testFileName != "")
            {
                path = Application.streamingAssetsPath + string.Format("/CSV/{0}.csv", testFileName);
                ReadAndDisplay();
            }
        }
        /// <summary>
        /// Debug function to allow reading a CSV file and output a row using Debug.Log
        /// </summary>
        void ReadAndDisplay()
        {
            string[,] stringArray;
            ReadCSVString(path, out stringArray);
            textBox.text = "";

            string textOutput = "";
            for (int y = 0; y <= stringArray.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= stringArray.GetUpperBound(0); x++)
                {
                    textOutput += stringArray[x, y];
                    textOutput += "|";
                }
                textOutput += "\n";
            }
            Debug.Log(textOutput);
            textBox.text = textOutput;
            Dictionary<string, string> keyValuePairs;
            //ReadCSVDictionary(path, out keyValuePairs);
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// This method opens a CSV file, reads text from it, parses it, and stores it in a 2D string array
        /// </summary>
        /// <param name="filePath">Path to the folder with CSVs</param>
        /// <param name="outStringArray">Out reference parameter that stores the final data in a 2D string array</param>
        /// <param name="fileData">Used to pass file data in string format that has already been read. Defaults to empty string ""</param>
        /// <param name="ignoreFirstRow">Set this to true to ignore the first row of each CSV file. Defaults to true</param>
        public static void ReadCSVString(string filePath, out string[,] outStringArray, string fileData = "", bool ignoreFirstRow = true)
        {
            if (fileData.Length == 0)
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogError("CSV file not found!");
                    outStringArray = null;
                    return;
                }
                fileData = File.ReadAllText(filePath);
            }

            string[] lines = fileData.Split("\n"[0]);

            // finds the max width of row
            int width = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tempRow = SplitCsvLine(lines[i]);
                List<string> row = new List<string>();
                for (int j = 0; j < tempRow.Length; j++)
                {
                    if (tempRow[j].CompareTo("") != 0)
                        row.Add(tempRow[j]);
                }
                width = Mathf.Max(width, row.Count);
            }

            //Ignoring first line which has labels
            //Size adapted to account for blank/null ones being read
            List<List<string>> outputGridList = new List<List<string>>();
            //string[,] outputGrid = new string[width - 1, lines.Length - 2];
            for (int y = 0; y < lines.Length; y++)
            {
                string[] row = SplitCsvLine(lines[y]);

                if (y == 0 && ignoreFirstRow)
                    continue;
                else if (y == lines.Length - 1 && row.Length == 0)
                    continue;

                outputGridList.Add(new List<string>());
                for (int x = 0; x < row.Length; x++)
                {
                    if (x == row.Length - 1 && row[x].CompareTo("") == 0)
                        continue;
                    outputGridList[outputGridList.Count - 1].Add(row[x]);
                    //outputGrid[x, y - 1] = row[x];
                }
            }
            string[,] outputGridNew = new string[width, outputGridList.Count];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < outputGridList.Count; j++)
                {
                    outputGridNew[i, j] = outputGridList[j][i];
                }
            }

            outStringArray = outputGridNew;
        }
        /// <summary>
        /// This method opens a CSV file using Resources.Load, reads text from it, parses it, and stores it in a 2D string array
        /// </summary>
        /// <param name="filePath">Path to the folder with CSVs</param>
        /// <param name="outStringArray">Out reference parameter that stores the final data in a 2D string array</param>
        /// <param name="fileData">Used to pass file data in string format that has already been read. Defaults to empty string ""</param>
        /// <param name="ignoreFirstRow">Set this to true to ignore the first row of each CSV file. Defaults to true</param>
        public static void ReadCSVStringFromResources(string filePath, out string[,] outStringArray, string fileData = "", bool ignoreFirstRow = true)
        {
            if (fileData.Length == 0)
            {
                fileData = ((TextAsset)Resources.Load(filePath)).text;
            }
            ReadCSVString("", out outStringArray, fileData, ignoreFirstRow);
        }

        /// <summary>
        /// Unused method that reads a CSV file, reads text from it, parses it, and stores it in a Dictionary
        /// </summary>
        /// <param name="filePath">Path to the folder with CSVs</param>
        /// <param name="outStringDict">Out reference parameter that stores the final data in a Dictionary</param>
        /// <param name="fileData">Used to pass file data in string format that has already been read. Defaults to empty string ""</param>
        /// <param name="ignoreFirstRow">Set this to true to ignore the first row of each CSV file. Defaults to true</param>
        static void ReadCSVAsDictionary(string filePath, out Dictionary<string, List<string>> outStringDict, string fileData = "", bool ignoreFirstRow = true)
        {
            string[,] outStringArray;
            ReadCSVString(filePath, out outStringArray, fileData, ignoreFirstRow);
            outStringDict = new Dictionary<string, List<string>>();

            for (int i = 0; i <= outStringArray.GetUpperBound(1); i++)
            {
                List<string> tempList = new List<string>();
                for (int j = 1; j <= outStringArray.GetUpperBound(0); j++)
                {
                    tempList.Add(outStringArray[j, i]);
                }
                outStringDict.Add(outStringArray[0, i], tempList);
            }
            Debug.Log(outStringDict[outStringArray[0, 0]]);
        }

        /// <summary>
        /// Splits a line from a CSV based on commas using a Regex
        /// </summary>
        /// <param name="line">Single line input from a CSV</param>
        static string[] SplitCsvLine(string line)
        {
            return (from System.Text.RegularExpressions.Match m in Regex.Matches(line,
                    @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                    System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                    select m.Groups[1].Value).ToArray();
        }
        /// <summary>
        /// Helper function to get a string value for a specified label from CSV-read data
        /// </summary>
        /// <param name="s">Label field of string type</param>
        /// <param name="stringArray">CSV data in 2D string array format</param>
        /// <param name="outString">Out reference output value string</param>
        /// <param name="logError">Set this to true log an error if this method fails</param>
        public static void GetStringFromStringArray(string s, string[,] stringArray, out string outString, bool logError = false)
        {
            for (int i = 0; i <= stringArray.GetUpperBound(1); i++)
            {
                if (s.CompareTo(stringArray[0, i]) == 0)
                {
                    outString = stringArray[1, i];
                    return;
                }
            }
            outString = "";
            if (logError)
                Debug.LogError("Error reading values from CSV, string was: " + s);
        }
        /// <summary>
        /// Helper function to get an int value for a specified label from CSV-read data
        /// </summary>
        /// <param name="s">Label field of string type</param>
        /// <param name="stringArray">CSV data in 2D string array format</param>
        /// <param name="outInt">Out reference output value int</param>
        public static void GetIntFromStringArray(string s, string[,] stringArray, out int outInt, bool logError = false)
        {
            for (int i = 0; i <= stringArray.GetUpperBound(1); i++)
            {
                if (s.CompareTo(stringArray[0, i]) == 0)
                {
                    outInt = int.Parse(stringArray[1, i]);
                    return;
                }
            }
            outInt = -1;
            if (logError)
                Debug.LogError("Error reading values from CSV");
        }
        /// <summary>
        /// Helper function to get a float value for a specified label from CSV-read data
        /// </summary>
        /// <param name="s">Label field of string type</param>
        /// <param name="stringArray">CSV data in 2D string array format</param>
        /// <param name="outFloat">Out reference output value int</param>
        public static void GetFloatFromStringArray(string s, string[,] stringArray, out float outFloat, bool logError = false)
        {
            for (int i = 0; i <= stringArray.GetUpperBound(1); i++)
            {
                if (s.CompareTo(stringArray[0, i]) == 0)
                {
                    outFloat = float.Parse(stringArray[1, i]);
                    return;
                }
            }
            outFloat = -1;
            if (logError)
                Debug.LogError("Error reading values from CSV");
        }        
    }
}
