using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameShow.Cloud
{
    public class RandomnessSimulator
    {
        public static void Start()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(Simulation);
        }

        private static void Simulation(object obj)
        {
            while (true)
            {
                var gameControllers = GameContext.Current.ControllersByGame("12345");

                foreach (var gc in gameControllers)
                {
                    Hubs.GameHub.ChangeControllerFrame(gc, "/12345/content/rf" + DateTime.Now.Second.ToString());
                }

                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}