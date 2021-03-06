﻿using System;
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
        [Route("controller/blink")]
        [HttpPost]
        public void BlinkController([FromBody] CloudGameStateController controller)
        {
            var c = GameContext.Current.ControllerByToken(controller.ControllerToken);
            if (c == null) { return; }
            Hubs.GameHub.BlinkController(c);
        }

        [Route("game/newid")]
        [HttpGet]
        public NewGameId GetNewGameId()
        {
            string userIp = System.Web.HttpContext.Current.Request.UserHostAddress;
            string gameId = new string(userIp.Where(c => char.IsDigit(c)).ToArray());

            int index = 0;
            while (GameContext.Current.IsGameIDInUse(gameId + index.ToString()))
            {
                index += 1;
            }

            return new NewGameId() {GameID = gameId + index.ToString()};
        }

        [Route("game/push")]
        [HttpPost]
        public CloudGameState PushGameState([FromBody] Game game)
        {
            if (string.IsNullOrEmpty(game.CloudId))
            {
                throw new Exception("Game has no ID");
            }

            var cg = GameContext.Current.GameByID(game.CloudId);
            cg.LastPush = DateTime.Now;
            cg.Game = game;
            GameContext.Current.SetControllerFrames(game);

            return cg.ToCloudGameState();
        }
    }
}
