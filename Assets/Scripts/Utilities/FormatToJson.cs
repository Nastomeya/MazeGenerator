//Reference from Dexy https://stackoverflow.com/questions/1207731/how-can-i-deserialize-json-to-a-simple-dictionarystring-string-in-asp-net
using System.Collections.Generic;

namespace DataExtensions
{
    //By Joseph Chiang 2021-07-25
    public class FormatToJson
    {
        public static string Serialize_FromDic<T1, T2>(Dictionary<T1, T2> _dictionary)
        {
            string jason = "{";
            int count = 0;
            foreach (var item in _dictionary)
            {
                jason += "\"" + item.Key + "\"" + ":" + item.Value;
                count++;
                jason += ((count >= _dictionary.Count) ? "}" : ",");
            }
            return jason;
        }
        public static Dictionary<int, bool> Deserialize_ToDicIntBool(string json)
        {
            if (json == "{}" || json == "")
                return null;
            else
            {
                json = json.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();

                Dictionary<int, bool> dict = new Dictionary<int, bool>();
                int isEndOfQuote = 0;
                bool getKeyNow = false, getValueNow = false;
                string newKey = "", currKey = "", currValue = "";

                for (int i = 0; i < json.Length; i++)
                {
                    char c = json[i];
                    if (c == '"' || c == '{' || c == '}' || c == ':' || c == ',')
                    {
                        if (c == ':')
                        {
                            newKey = currKey;
                            currKey = "";
                            getValueNow = true;
                            getKeyNow = false;
                        }
                        else if (c == ',' || c == '}')//end of one item
                        {
                            AddToDictionary(dict, newKey, currValue);

                            currValue = "";
                            getValueNow = false;
                        }
                        else if (c == '"')
                        {
                            isEndOfQuote = (isEndOfQuote + 1) % 2;
                            if (isEndOfQuote < 2) getKeyNow = true;
                        }
                        continue;
                    }

                    if (getKeyNow) currKey += c;
                    if (getValueNow) currValue += c;
                }
                return dict;
            }
        }

        private static void AddToDictionary(Dictionary<int, bool> dict, string newKey, string currValue)
        {
            int key = int.Parse(newKey);
            bool value = bool.Parse(currValue);
            dict.Add(key, value);
        }
    }
}