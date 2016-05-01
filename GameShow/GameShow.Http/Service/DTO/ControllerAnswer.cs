using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace GameShow.Http.Service.DTO
{
    [Route("/controller/{ControllerID}/answer/{AnswerID}")]
    internal class ControllerAnswer
    {
        public string ControllerID { get; set; }
        public string AnswerID { get; set; }
    }
}
