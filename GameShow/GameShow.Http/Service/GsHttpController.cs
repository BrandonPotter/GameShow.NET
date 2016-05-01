using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShow.Http.Service.DTO;
using ServiceStack;

namespace GameShow.Http.Service
{
    internal class GsHttpController : ServiceStack.Service
    {
        private void Log(string msg)
        {
            Logging.LogMessage("HTTP", msg);
        }

        public object Any(ControllerAnswer answer)
        {
            Log($"Controller {answer.ControllerID} answered {answer.AnswerID}");
            GsHttpServer.FireControllerAnswerEvent(answer.ControllerID, answer.AnswerID);
            return "OK";
        }
    }
}
