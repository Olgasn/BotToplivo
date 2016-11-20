using AIMLbot;
using System;
using Yandex.Speller.Api;
using Yandex.Speller.Api.DataContract;

namespace BotToplivo
{
    //Класс для проверки ошибок и получения ответа от интерпретатора AIML
    public class Replay
    {
        private string correctedMessage;
        public string InputMessage { get; set; }
        public string CorretedMessage
        {
            get
            {
                correctedMessage = CorrectInputMessage(InputMessage)??"";
                return correctedMessage;
            }
         }
        public string MessageFromAIML
        {
            get
            {
                string messageFromAIML = GetResponseFromAIML(CorretedMessage)??"";
                return messageFromAIML;
            }
        }


        //Корректировка правописания запроса
        private string CorrectInputMessage(string inputText)
        {
            string correctedmessage = inputText;
            IYandexSpeller speller = new YandexSpeller();
            SpellResult result = speller.CheckText(correctedmessage, Lang.Ru | Lang.En, Options.Default, TextFormat.Plain);
            int countErrs = result.Errors.Count;
            if (countErrs > 0)
            {
                for (int i = countErrs; i > 0; i--)
                {
                    var err = result.Errors[i - 1];

                    if (err.Steer.Count > 0)
                    {
                        correctedmessage = correctedmessage.Remove(err.Pos, err.Len);
                        correctedmessage = correctedmessage.Insert(err.Pos, err.Steer[0]);
                    }
                };
            }
            return correctedmessage;
        }

        public string GetResponseFromAIML(string inputText)
        {
            string responseAIML = "";
            Bot myBot = new Bot();
            myBot.loadSettings();
            User myUser = new User("WebUser", myBot);
            myBot.isAcceptingUserInput = false;
            myBot.loadAIMLFromFiles();
            myBot.isAcceptingUserInput = true;

            Request r = new Request(inputText, myUser, myBot);
            Result res = myBot.Chat(r);
            responseAIML = res.Output;
            return responseAIML;

        }

    }
}