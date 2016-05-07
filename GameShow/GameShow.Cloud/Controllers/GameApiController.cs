using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GameShow.GameModel;

namespace GameShow.Cloud.Controllers
{
    public class GameApiController : ApiController
    {
        public CloudGameState PushGameState([FromBody] Game game)
        {
            if (string.IsNullOrEmpty(game.CloudId))
            {
                throw new Exception("Game has no ID");
            }

            var cg = GameContext.Current.GameByID(game.CloudId);

            CloudGameState cgs = new CloudGameState();
            cgs.GameID = game.CloudId;
            cgs.Controllers = new List<CloudGameStateController>();

            foreach (var c in GameContext.Current.ControllersByGame(game.CloudId))
            {
                cgs.Controllers.Add(new CloudGameStateController() {ControllerToken = c.ControllerToken});
            }

            return cgs;
        }
    }
}
