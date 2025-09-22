using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace sayunanaBase.Helper
{
    public partial class Translate
    {
        public enum Language
        {
            ja = 0,
            en
        }

        public static Language SystemLanguage = Language.ja;

        private static Dictionary<string, Dictionary<string, string>> _loadLanguageText =
            new Dictionary<string, Dictionary<string, string>>();

        public static string TranslateText(string projectName, Language language, string key)
        {
            var projectKey = $"{projectName}-{language}";
            if (!_loadLanguageText.TryGetValue(projectKey, out var translateDic))
            {
                translateDic = TranslateTextLoad(projectName, language);
            }

            return translateDic.ContainsKey(key) ? translateDic[key] : $"未設定 : {key}";
        }

        private static Dictionary<string, string> TranslateTextLoad(string projectName, Language language)
        {
            var loadLanguageFile = Resources.Load<TextAsset>($"{projectName}-{language}").ToString();
            var data = Deserialize(loadLanguageFile);
            _loadLanguageText.Add(loadLanguageFile, data);
            return data;
        }

        private static Dictionary<string, string> Deserialize(string json)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            json = json.Replace("{", "").Replace("}", "");

            var columns = json.Split(",\r\n");

            foreach (var column in columns)
            {
                var t = column.Split(":");
                var key = t[0];
                var value = t[1];
                string pattern = $@"{Regex.Escape("\"")}(.*?){Regex.Escape("\"")}";
                var keyOut = Regex.Match(key, pattern);
                var valueOut = Regex.Match(value, pattern);
                dic.Add(keyOut.Groups[1].Value, valueOut.Groups[1].Value);
            }

            return dic;
        }

        public static void ResetDictionary()
        {
            _loadLanguageText = new Dictionary<string, Dictionary<string, string>>();
        }
    }
}