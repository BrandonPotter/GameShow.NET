using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShow.GameModel;

namespace GameShow.CloudClient
{
    public class CloudSession
    {
        private string Endpoint(string relativePath)
        {
            if (!relativePath.StartsWith("/")) {  relativePath = $"/{relativePath}"}
            return "http://gshow.azurewebsites.net" + relativePath;
        }

        public void PushGameState(Game game)
        {
                
        }
    }
}
