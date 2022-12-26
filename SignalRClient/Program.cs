using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRClient
{
    internal class Program 
    {
        static async Task Main()
        {
            using (HubConnection hubConnection = new HubConnection("http://notificationservices1.southcentralus.cloudapp.azure.com"))
            {
                IHubProxy messageHubProxy = hubConnection.CreateHubProxy("MessageHub");
                await hubConnection.Start();

                Console.WriteLine(hubConnection.State.ToString());
                Console.WriteLine("Press Enter key to exit.");
                Console.ReadLine();
            }
        }
    }
}
