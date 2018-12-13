using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda1
{
    public class Function
    {
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)

        {
            string outputText = "";
            var requestType = input.GetRequestType();

            if (requestType == typeof(LaunchRequest))
            {
                return BodyResponse("Welcome to the Student Ticket Marketplace, please announce what you wish to accomplish here!", false);
            }

            else if (requestType == typeof(IntentRequest))
            {
                //outputText += "Request type is Intent";
                var intent = input.Request as IntentRequest;

                if (intent.Intent.Name.Equals("Available"))
                {
                    var EventName = intent.Intent.Slots["EventName"].Value;
                    var EventDate = intent.Intent.Slots["EventDate"].Value;

                    if (EventName == null)
                    {
                        return BodyResponse("I did not understand the name of the event, please try again.", false);
                    }

                    else if (EventDate == null)
                    {
                        return BodyResponse("I did not understand the date in which you requested, please try again.", false);
                    }

                    var playerInfo = await GetEventInfo(EventName, EventDate, context);
                    {
                        outputText = $"The ticket for , {EventInfo.Eventname} game on {EventInfo.EventDate}. " + $" Has an average price of {EventInfo.Eventaveragaeprice}dollars";
                    }
                // create web service and use database information to call GeteventInfo 
                    return BodyResponse(outputText, true);
                }

                else if (intent.Intent.Name.Equals("AMAZON.StopIntent"))
                {

                    return BodyResponse("You have now exited the Student Ticket Marketplace", true);
                }

                else
                {
                    return BodyResponse("I did not understand this intent, please try again", true);
                }
            }

            else
            {
                return BodyResponse("I did not understand your request, try again", true);
            }


        }

        private SkillResponse BodyResponse(string outputSpeech,
        bool shouldEndSession,
        string repromptText = "Just ask, what is the average ticket price for saturday, to learn more. To exit, say, exit.")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

            if (repromptText != null)
            {
                response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromptText } };
            }

            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };
            return skillResponse;
        }

    }

}





