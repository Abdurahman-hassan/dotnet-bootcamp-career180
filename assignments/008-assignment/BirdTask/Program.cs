namespace Bird
{


    public class Bird
    {
        public string Name { get; set; }
        public int WingSpan { get; set; }

        public virtual void DisplayInfo() {
            Console.WriteLine($"{Name} is a bird that has a wingspan of {WingSpan}");
        }
        
        public virtual void Fly()
        {
            Console.WriteLine("It does fly.");
        }

    }

    // sparrow
    public class Sparrow : Bird 
    {
        public Sparrow() { 
            Name = "Sparrow";
            WingSpan = 100;
        }
        public override void Fly() {
            Console.WriteLine("sparrow is flying");
        }
    }



    // Eagle
    public class Eagle : Bird
    {
        public Eagle()
        {
            Name = "Eagle";
            WingSpan = 10000;
        }
        public override void Fly()
        {
            Console.WriteLine("Eagle is flying");
        }
    }




    internal class Program
    {
        static void Main(string[] args)
        {
            Bird bird = new Bird();
            Sparrow sparrow = new Sparrow();
            Eagle eagle = new Eagle();

            bird.DisplayInfo();
            bird.Fly();
            // ===============================
            sparrow.DisplayInfo();
            sparrow.Fly();
            // ===============================
            eagle.DisplayInfo();
            eagle.Fly();
        }
    }
}
