using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using AIMLbot;
using Yandex.Speller.Api;
using Yandex.Speller.Api.DataContract;

namespace BotToplivo
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)          {     

                //Корректировка правописания запроса
                string inputmessage = activity.Text;

                IYandexSpeller speller = new YandexSpeller();
                SpellResult result = speller.CheckText(activity.Text, Lang.Ru | Lang.En, Options.Default, TextFormat.Plain);
                int countErrs = result.Errors.Count;
                if (countErrs > 0)
                {
                    for (int i = countErrs; i > 0; i--)
                    {
                        var err = result.Errors[i-1];

                        if (err.Steer.Count > 0)
                        {
                            inputmessage = inputmessage.Remove(err.Pos, err.Len);
                            inputmessage = inputmessage.Insert(err.Pos, err.Steer[0]);
                        }

                    };
                }
                Bot myBot = new Bot();
                myBot.loadSettings();
                User myUser = new User("WebUser", myBot);
                myBot.isAcceptingUserInput = false;
                myBot.loadAIMLFromFiles();
                myBot.isAcceptingUserInput = true;

                Request r = new Request(inputmessage, myUser, myBot);
                Result res = myBot.Chat(r);
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                // return our reply to the user

                string outputmessage = res.Output;
                Activity reply = activity.CreateReply(outputmessage);
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}