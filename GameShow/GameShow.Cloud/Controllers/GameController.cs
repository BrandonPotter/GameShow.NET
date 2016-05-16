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
            var game = GameContext.Current.GameByID(gameId);
            if (game == null)
            {
                ViewBag.Text = "Game not found";
            }
            else
            {
                ViewBag.Text = "Welcome to " + game.Game.Name;
            }
            
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

        [Route("{gameId}/idle")]
        public ActionResult Idle(string gameId)
        {
            return View("GameIdle");
        }

        [Route("{gameId}/p/{controllerToken}")]
        public ActionResult ControllerPrompt(string gameId, string controllerToken)
        {
            var g = GameContext.Current.GameByID(gameId);
            var targetPrompt = g.Game.ControllerPrompts.FirstOrDefault(
                cp => cp.ControllerToken.ToUpperInvariant() == controllerToken.ToUpperInvariant());

            return View("ControllerPrompt", targetPrompt);
        }
    }
}