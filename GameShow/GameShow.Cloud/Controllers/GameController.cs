using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameShow.Cloud.Controllers
{
    public class GameController : Controller
    {
        [Route("")]
        public ActionResult PromptForGameId()
        {
            return View();
        }

        [Route("{gameId}")]
        public ActionResult EnterGame(string gameId)
        {
            ViewBag.GameID = gameId;
            return View("GameFrame");
        }

        [Route("{gameId}/content/{frame}")]
        public ActionResult Frame(string gameId, string frame)
        {
            ViewBag.FrameName = frame;
            return View("GenericFrame");
        }

        [Route("{gameId}/disconnected")]
        public ActionResult Disconnected(string gameId)
        {
            return View("GameEnded");
        }
    }
}