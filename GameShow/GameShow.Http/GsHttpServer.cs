using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShow.Http.Service;
using ServiceStack;

namespace GameShow.Http
{
    public delegate void ControllerAnswerEvent(string controllerId, string answerId);
    public class GsHttpServer
    {
        public static event ControllerAnswerEvent ControllerAnswer;

        private static ServiceStackHost _httpHost = null;
        public static void Start(int port)
        {
            var listeningOn = $"http://*:{port}/";
            _httpHost = new GsHttpHost()
                .Init()
                .Start(listeningOn);

            Console.WriteLine("HTTP Host created at {0}, listening on {1}",
                DateTime.Now, listeningOn);
        }

        public static void Stop()
        {
            if (_httpHost == null)
            {
                return; 
            }

            _httpHost.Dispose();
            _httpHost = null;
        }

        internal static void FireControllerAnswerEvent(string controllerId, string answerId)
        {
            if (ControllerAnswer != null)
            {
                ControllerAnswer(controllerId, answerId);
            }
        }
    }
}
