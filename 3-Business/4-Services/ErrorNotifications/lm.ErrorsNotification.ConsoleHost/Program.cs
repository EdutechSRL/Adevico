using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.ErrorsNotification.DataContract.Service;
using System.Messaging;
using System.ServiceModel;
using System.Configuration;

namespace lm.ErrorsNotification.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Work();
        }




        private static void Work()
        {
            string queueName = ConfigurationManager.AppSettings["ErrorsQueueName"];
            string poisonQueueName = ConfigurationManager.AppSettings["PoisonErrorsQueueName"];

            if (!MessageQueue.Exists(queueName))
                MessageQueue.Create(queueName, true);

            if (!MessageQueue.Exists(poisonQueueName))
                MessageQueue.Create(poisonQueueName, true);

            Type serviceType = typeof(ErrorsNotification.Service.ErrorsNotificationService);
            using (ServiceHost host = new ServiceHost(serviceType))
            {
                host.Open();

                Console.WriteLine("Number of base addresses : {0}", host.BaseAddresses.Count);
                foreach (Uri uri in host.BaseAddresses)
                {
                    Console.WriteLine("\t{0}", uri.ToString());
                }

                Console.WriteLine();
                Console.WriteLine("Number of dispatchers listening : {0}", host.ChannelDispatchers.Count);
                foreach (System.ServiceModel.Dispatcher.ChannelDispatcher dispatcher in host.ChannelDispatchers)
                {
                    Console.WriteLine("\t{0}, {1}", dispatcher.Listener.Uri.ToString(), dispatcher.BindingName);
                }

                Console.WriteLine();
                Console.WriteLine("Press <ENTER> to terminate Host");
                Console.ReadLine();
            }
        }
    }
}
