using Azure;
using Azure.Messaging.EventGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EventService
    {
        private AzureKeyCredential AzureKeyCredential { get; }

        private Uri Azure_event_grid_domain_endpoint { get; }

        readonly EventGridPublisherClient eventGridPublisherClient;
        public EventService()
        {
            AzureKeyCredential = new AzureKeyCredential(Environment.GetEnvironmentVariable("azure_event_grid_key"));
            Azure_event_grid_domain_endpoint = new Uri(Environment.GetEnvironmentVariable("azure_event_grid_domain_endpoint"));

            eventGridPublisherClient = new EventGridPublisherClient(Azure_event_grid_domain_endpoint, AzureKeyCredential);
        }


        public async Task <Response> PublishEvent(string topic, int objectId)
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>
            {
                new EventGridEvent(
                    "ExampleEventSubject",
                    topic,
                    "1.0",
                    objectId)
                {
                    Topic = topic
                }
            };

            return await eventGridPublisherClient.SendEventsAsync(eventsList);
        }
    }
}
