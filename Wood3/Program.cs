using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
namespace Wood3
{

    public class GameObject
    {
        public int entityId;
        public string entityType;
        public int x;
        public int y;
    }

    public class Ship : GameObject
    {
        public int rotation;//arg1: the ship's rotation orientation (between 0 and 5)
        public int speed;//arg2: the ship's speed (between 0 and 2)
        public int fuel;         //arg3: the ship's stock of rum units
        public int isControlled;              //arg4: 1 if the ship is controlled by you, 0 otherwise
    }

    public class BARREL : GameObject
    {
        public int amountOfRum; //arg1: the amount of rum in this barrel
    }

    public class CANNONBALL : GameObject
    {
        public int amountOfRum; //arg1: the amount of rum in this barrel
    }

    public interface IStrategy
    {
        public Direction NextMove(GameObject myShip, Dictionary<int, GameObject> gameObjects);
    }

    public class Direction
    {
        public int index;
        public int x;
        public int y;
        public double score;
    }

    public class Wood3Strategy : IStrategy
    {

        public Direction NextMove(GameObject myShip, Dictionary<int, GameObject> gameObjects)
        {
            int isEvenLine = myShip.y % 2 == 0 ? -1 : 0;
            List<Direction> directions = new List<Direction>() {
             new Direction(){ index= 0, x=myShip.x+1, y=myShip.y, score=0.0 },
             new Direction(){ index= 1, x=myShip.x+1+isEvenLine, y=myShip.y-1, score=0.0 },
             new Direction(){ index= 2, x=myShip.x+isEvenLine, y=myShip.y-1, score=0.0 },
             new Direction(){ index= 3, x=myShip.x-1, y=myShip.y, score=0.0 },
             new Direction(){ index= 4, x=myShip.x+isEvenLine, y=myShip.y+1, score=0.0 },
             new Direction(){ index= 5, x=myShip.x+1+isEvenLine, y=myShip.y+1, score=0.0 },
            };

            double norm = Math.Sqrt(21.0 * 21.0 + 23.0 * 23.0);
            foreach (var dir in directions)
            {
                double s = 0.0;
                foreach (GameObject g in gameObjects.Values)
                {
                    double distance = Math.Sqrt((g.x - dir.x) * (g.x - dir.x) + (g.y - dir.y) * (g.y - dir.y));
                    if (g is BARREL)
                    {
                        BARREL barrel = g as BARREL;
                        s += barrel.amountOfRum * (norm/distance);
                    }
                }
                dir.score = s;
            }
            var bestDir = directions.OrderByDescending(x => x.score).First();
            return bestDir;
        }
    }

    public class GameManager
    {
        public int myShipCount;
        public Dictionary<int, GameObject> gameObjects;
        public IStrategy gameStrategy;

        public string NextMove(GameObject myShip, Dictionary<int, GameObject> gameObjects)
        {
            Direction bestDir = gameStrategy.NextMove(myShip, gameObjects);
            return $"MOVE {bestDir.x} {bestDir.y}";
        }
    }


    public class Player
    {
        public static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.gameStrategy = new Wood3Strategy();
            // game loop
            while (true)
            {
                int myShipCount = int.Parse(Console.ReadLine()); // the number of remaining ships
                Console.Error.WriteLine(myShipCount);
                int entityCount = int.Parse(Console.ReadLine()); // the number of entities (e.g. ships, mines or cannonballs)
                Console.Error.WriteLine(entityCount);
                gameManager.myShipCount = myShipCount;
                gameManager.gameObjects = new Dictionary<int, GameObject>();
                for (int i = 0; i < entityCount; i++)
                {
                    string turnData = Console.ReadLine();
                    Console.Error.WriteLine(turnData);
                    string[] inputs = turnData.Split(' ');

                    int entityId = int.Parse(inputs[0]);
                    string entityType = inputs[1];
                    int x = int.Parse(inputs[2]);
                    int y = int.Parse(inputs[3]);
                    int arg1 = int.Parse(inputs[4]);
                    int arg2 = int.Parse(inputs[5]);
                    int arg3 = int.Parse(inputs[6]);
                    int arg4 = int.Parse(inputs[7]);


                    switch (entityType)
                    {
                        case "SHIP":
                            gameManager.gameObjects.Add(entityId, new Ship() { entityId = entityId, entityType = entityType, x = x, y = y, rotation = arg1, speed = arg2, fuel = arg3, isControlled = arg4 });
                            break;
                        case "BARREL":
                            gameManager.gameObjects.Add(entityId, new BARREL() { entityId = entityId, entityType = entityType, x = x, y = y, amountOfRum = arg1 });
                            break;
                    }

                }

                foreach (var gameObject in gameManager.gameObjects.Values)
                {
                    if (gameObject is Ship)
                    {
                        var thisShip = gameObject as Ship;
                        if (1 == thisShip.isControlled)
                        {
                            Console.WriteLine(gameManager.NextMove(thisShip, gameManager.gameObjects));
                        }
                    }
                }
            }
        }
    }

}
