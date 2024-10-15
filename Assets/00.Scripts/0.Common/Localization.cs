using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Localization : MonoBehaviour
{
    static Dictionary<string, LanguageString> table_Languages = new Dictionary<string, LanguageString>();

    public List<SET_LOCALIZATION_STRING> list_localization = new List<SET_LOCALIZATION_STRING>();

    [Serializable]
    public struct SET_LOCALIZATION_STRING
    {
        public TextMeshProUGUI text_target;
        public string key;
    }

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
        if(table_Languages.Count == 0)
        {
            ReadLanguageTable();
        }
    }

    void Start()
    {
        SetTranslate();
    }

    void Update()
    {

    }

    public void SetTranslate()
    {
        for(int i = 0; i < list_localization.Count; i++)
        {
            list_localization[i].text_target.text = GetStringByKey(list_localization[i].key, list_localization[i].text_target.text);
        }
    }

    static void ReadLanguageTable()
    {
#if true
        TextAsset csvFile = Resources.Load<TextAsset>("Languages");

        if(csvFile != null)
        {
            Debug.LogFormat("<color=green>Read CSV File!</color>");

            string str = csvFile.text;
            table_Languages = ReadCSVFile(str);
        }
        else
        {
            Debug.LogError("��� ������ �������� �ʽ��ϴ�.");
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
            Debug.LogError("��� ������ �������� �ʽ��ϴ�.");
        }
#endif
    }

    public static string GetStringByString(string target)
    {
        string result = target;
        LanguageString value;

        foreach (var entry in table_Languages)
        {
            if (entry.Value.language_en == target || entry.Value.language_kr == target)
            {
                string key = entry.Key;
                if (table_Languages.TryGetValue(key, out value))
                {
                    switch (SingletonCom.curr_language)
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
        string result = originString;
        LanguageString value;

        if (table_Languages.TryGetValue(key, out value))
        {
            // core ���� table search
            switch (SingletonCom.curr_language)
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
        // csv ������ ',' ó��
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
