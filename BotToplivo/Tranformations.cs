using System;

namespace BotToplivo
{
    public class Tranformations
    {
        //извлечение из строки SQL команды
        public static string SQLtext(string text)
        {
            string[] stringSeparators = { "<SQL>", "</SQL>" };
            string result="";

            if (text.Contains(stringSeparators[0]) & text.Contains(stringSeparators[1]))
            {
                string[] words;
                words = text.Split(stringSeparators, StringSplitOptions.None);
                result = words[0] ?? "";
                if (words.Length > 1)
                {
                    result = words[1];
                }

            }
            return result;
        }
        //Форматирование текста запроса
        public string Formate(string text)
        {
            string formatedmessage = text;

            return formatedmessage;
        }
    }
}