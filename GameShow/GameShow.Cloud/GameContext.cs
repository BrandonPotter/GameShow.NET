using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameShow.Cloud.Models;
using GameShow.GameModel;

namespace GameShow.Cloud
{
	public class GameContext
	{
        private static GameContext _context = null;
	    public static GameContext Current
	    {
	        get
	        {
	            if (_context == null)
	            {
                    _context = new GameContext();
	            }

                return _context;
            }
	    }

	    private ConcurrentDictionary<string, CloudGame> _games = new ConcurrentDictionary<string, CloudGame>();
        private ConcurrentDictionary<string, CloudGameController> _controllers = new ConcurrentDictionary<string, CloudGameController>();

	    public bool IsGameIDInUse(string gameId)
	    {
	        if (string.IsNullOrEmpty(gameId)) { return true; }
	        return _games.ContainsKey(gameId.ToUpperInvariant());
	    }

        public CloudGame GameByID(string gameId)
	    {
	        if (string.IsNullOrEmpty(gameId))
	        {
	            return null;
	        }
	        if (!_games.ContainsKey(gameId.ToUpperInvariant()))
	        {
	            _games[gameId.ToUpperInvariant()] = new CloudGame() {GameID = gameId.ToUpperInvariant()};
	        }

	        return _games[gameId.ToUpperInvariant()];
	    }

        public CloudGameController ControllerByToken(string controllerToken)
        {
            if (string.IsNullOrEmpty(controllerToken))
            {
                return null;
            }
            if (!_controllers.ContainsKey(controllerToken.ToUpperInvariant()))
            {
                _controllers[controllerToken.ToUpperInvariant()] = new CloudGameController() { ControllerToken = controllerToken.ToUpperInvariant() };
            }

            return _controllers[controllerToken.ToUpperInvariant()];
        }

        public void SetControllerHeartbeat(string controllerToken, string gameId, string connectionId, string nickname)
        {
            var c = ControllerByToken(controllerToken);
            string oldGameId = c.GameID;
            c.GameID = (gameId ?? string.Empty).ToUpperInvariant();
            c.LastHeartbeat = DateTime.Now;
            c.ConnectionID = connectionId;

            if (!string.IsNullOrEmpty(nickname))
            {
                c.Nickname = nickname;
            }

            if (oldGameId != c.GameID)
            {
                if (!string.IsNullOrEmpty(oldGameId))
                {
                    Hubs.GameHub.NotifyHostGameStateChanged(GameByID(oldGameId));
                }

                if (!string.IsNullOrEmpty(c.GameID))
                {
                    Hubs.GameHub.NotifyHostGameStateChanged(GameByID(c.GameID));
                }
            }
        }

	    public IEnumerable<CloudGameController> ControllersByGame(string gameId)
	    {
	        return _controllers.Values.Where(v => v.GameID == (gameId ?? string.Empty).ToUpperInvariant()).ToArray();
	    }

	    public void SetControllerFrames(Game game)
	    {
	        foreach (var c in ControllersByGame(game.CloudId))
	        {
	            if (game.ControllerPrompts != null)
	            {
	                var cPrompt =
	                    game.ControllerPrompts.FirstOrDefault(
	                        cp =>
	                            (cp.ControllerToken ?? string.Empty).ToUpperInvariant() ==
	                            c.ControllerToken.ToUpperInvariant());

	                if (cPrompt != null)
	                {
	                    Hubs.GameHub.ChangeControllerFrame(c, $"/{game.CloudId}/p/{c.ControllerToken}");
	                }
	                else
	                {
	                    Hubs.GameHub.ChangeControllerFrame(c, $"/{game.CloudId}/idle");
	                }
	            }
	        }
	    }

	    public void SetGameHostHeartbeat(string gameId, string connectionId)
	    {
	        if (string.IsNullOrEmpty(gameId)) { return; }
	        var g = GameByID(gameId);
            g.LastHostSignalRHeartbeat = DateTime.Now;
            g.HostConnectionID = connectionId;
	    }
	}
}