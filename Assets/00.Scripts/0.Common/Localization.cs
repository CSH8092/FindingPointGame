using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Localization : MonoBehaviour
{
    static Dictionary<string, LanguageString> table_Languages = new Dictionary<string, LanguageString>();
    public static CurrentLanguage curr_language;

    class LanguageString
    {
        public string language_en;
        public string language_kr;
    }

    public enum CurrentLanguage
    {
        ENGLISH,
        KOREAN,
    }

    private void Awake()
    {
        curr_language = CurrentLanguage.KOREAN;
    }

    void Start()
    {
        ReadLanguageTable();
    }

    void Update()
    {

    }

    static void ReadLanguageTable()
    {
#if true
        TextAsset csvFile = Resources.Load<TextAsset>("Languages");

        if(csvFile != null)
        {
            string str = csvFile.text;
            table_Languages = ReadCSVFile(str);
        }
        else
        {
            Debug.LogError("언어 파일이 존재하지 않습니다.");
        }
#else
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(documentsPath, "Languages.csv");

        if (File.Exists(filePath))
        {
            string str = File.ReadAllText(filePath);
            table_Languages = ReadCSVFile(str);
        }
        else
        {
            Debug.LogError("언어 파일이 존재하지 않습니다.");
        }
#endif
    }

    public static string GetStringByString(string target, string originString = "")
    {
#if UNITY_EDITOR
        curr_language = CurrentLanguage.KOREAN; // test
        if (table_Languages.Count == 0)
        {
            ReadLanguageTable();
        }
#endif

        string result = originString;
        LanguageString value;

        foreach (var entry in table_Languages)
        {
            if (entry.Value.language_en == target || entry.Value.language_kr == target)
            {
                string key = entry.Key;
                if (table_Languages.TryGetValue(key, out value))
                {
                    switch (curr_language)
                    {
                        case CurrentLanguage.ENGLISH:
                            result = value.language_en;
                            break;
                        case CurrentLanguage.KOREAN:
                            result = value.language_kr;
                            break;
                        default:
                            result = value.language_en;
                            break;
                    }
                    break;
                }
            }
        }

        return result;
    }

    public static string GetStringByKey(string key, string originString = "")
    {
#if UNITY_EDITOR
        curr_language = CurrentLanguage.KOREAN; // test
        if (table_Languages.Count == 0)
        {
            ReadLanguageTable();
        }
#endif

        string result = originString;
        LanguageString value;

        if (table_Languages.TryGetValue(key, out value))
        {
            // core 공통 table search
            switch (curr_language)
            {
                case CurrentLanguage.ENGLISH:
                    result = value.language_en;
                    break;
                case CurrentLanguage.KOREAN:
                    result = value.language_kr;
                    break;
                default:
                    result = value.language_en;
                    break;
            }
        }

        return result;
    }

    static Dictionary<string, LanguageString> ReadCSVFile(string str)
    {
        Dictionary<string, LanguageString> result = new Dictionary<string, LanguageString>();
        List<string> tmpList = new List<string>();

        string[] readLine = str.Split(Environment.NewLine);
        foreach (string line in readLine)
        {
            string[] parses = ParseCSVLine(line);
            LanguageString tmp = new LanguageString();
            string key = parses[0];
            tmp.language_en = parses[1];
            tmp.language_kr = parses[2];

            // add jp, ch, ...
            result.TryAdd(key, tmp);
            //Debug.Log(key + ", " + tmp.language_en + ", " + tmp.language_kr);
        }

        return result;
    }

    static string[] ParseCSVLine(string line)
    {
        // csv 파일의 ',' 처리
        List<string> tmp = new List<string>();
        bool flag = false;
        string currstr = "";

        foreach (char c in line)
        {
            if (c == '\"')
            {
                flag = !flag;
            }
            else if (c == ',' && !flag)
            {
                tmp.Add(currstr);
                currstr = "";
            }
            else
            {
                currstr += c;
            }
        }
        tmp.Add(currstr);

        return tmp.ToArray();
    }
}
