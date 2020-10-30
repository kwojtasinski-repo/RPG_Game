using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_GAME
{
    public class Story
    {
        public static void Begin()
        {
            Console.WriteLine("You are the best hero who is on the way to kill the dragon");
            Console.WriteLine("Dragon wants to destroy your kingdom");
            Console.WriteLine("On your way to the lairs of the dragons, there is couple of archers.");
            Console.WriteLine("And they dont want to let you go without a fight");
            Console.ReadLine();
        }

        public static void BeforeKnights()
        {
            Console.WriteLine("Archers dont want to duel you. Well Done! You continue on to the dragons lair!");
            Console.WriteLine("However the dragons hired some knights to defense their lairs from people like you");
            Console.WriteLine("Watch out!");
            Console.WriteLine("Theres 2 of them that have found out about your quest.");
            Console.ReadLine();
        }

        public static void BeforeDragons()
        {
            Console.WriteLine("Congrats you killed them, you continue on your journey!");
            Console.WriteLine("You are close to the drgaon lairs.");
            Console.WriteLine("It's hot and dangerous in there.");
            Console.WriteLine("The time has come to end the dragons rampage!");
            Console.ReadLine();
        }

        public static void TheEnd()
        {
            Console.WriteLine("You killed the dragons and saved the kingdom!");
            Console.WriteLine("Congrats!");
            Console.ReadLine();
        }
    }
}
