using System;
using System.Collections.Generic;

namespace swalabfive
{
    // Component interface - Base interface for all computer parts
    public interface IComputerComponent
    {
        void Accept(IComputerVisitor visitor);
        int GetPrice();
        string GetName();
    }

    // Visitor interface - Defines operations for each component type
    public interface IComputerVisitor
    {
        void VisitMemory(Memory memory);
        void VisitRam(Ram ram);
        void VisitRom(Rom rom);
        void VisitExternalDisk(ExternalDisk externalDisk);
        void VisitCpu(Cpu cpu);
        void VisitKeyboard(Keyboard keyboard);
        void VisitMonitor(Monitor monitor);
        void VisitGraphicCard(GraphicCard graphicCard);
        void VisitGpu(Gpu gpu);
        void VisitGraphicMemory(GraphicMemory graphicMemory);
    }

    // Abstract composite component
    public abstract class CompositeComponent : IComputerComponent
    {
        protected List<IComputerComponent> _children = new List<IComputerComponent>();
        protected string _name;

        protected CompositeComponent(string name)
        {
            _name = name;
        }

        public void Add(IComputerComponent component)
        {
            _children.Add(component);
        }

        public virtual int GetPrice()
        {
            int total = 0;
            foreach (var child in _children)
            {
                total += child.GetPrice();
            }
            return total;
        }

        public string GetName() => _name;

        public abstract void Accept(IComputerVisitor visitor);
    }

    // Memory Price Visitor implementation
    public class MemoryPriceVisitor : IComputerVisitor
    {
        private int _totalMemoryPrice = 0;
        private List<string> _visitedComponents = new List<string>();

        public int GetTotalMemoryPrice() => _totalMemoryPrice;
        public List<string> GetVisitedComponents() => _visitedComponents;

        public void VisitMemory(Memory memory) { }
        public void VisitCpu(Cpu cpu) { }
        public void VisitKeyboard(Keyboard keyboard) { }
        public void VisitMonitor(Monitor monitor) { }
        public void VisitGraphicCard(GraphicCard graphicCard) { }
        public void VisitGpu(Gpu gpu) { }
        public void VisitGraphicMemory(GraphicMemory graphicMemory) { }

        public void VisitRam(Ram ram)
        {
            _totalMemoryPrice += ram.GetPrice();
            _visitedComponents.Add($"{ram.GetName()}: {ram.GetPrice()} AED");
        }

        public void VisitRom(Rom rom)
        {
            _totalMemoryPrice += rom.GetPrice();
            _visitedComponents.Add($"{rom.GetName()}: {rom.GetPrice()} AED");
        }

        public void VisitExternalDisk(ExternalDisk externalDisk)
        {
            _totalMemoryPrice += externalDisk.GetPrice();
            _visitedComponents.Add($"{externalDisk.GetName()}: {externalDisk.GetPrice()} AED");
        }
    }

    // Composite Components
    public class Computer : CompositeComponent
    {
        public Computer() : base("Computer") { }

        public override void Accept(IComputerVisitor visitor)
        {
            foreach (var component in _children)
            {
                component.Accept(visitor);
            }
        }
    }

    public class Memory : CompositeComponent
    {
        public Memory() : base("Memory") { }

        public override void Accept(IComputerVisitor visitor)
        {
            visitor.VisitMemory(this);
            foreach (var component in _children)
            {
                component.Accept(visitor);
            }
        }
    }

    public class GraphicCard : CompositeComponent
    {
        public GraphicCard() : base("Graphic Card") { }

        public override void Accept(IComputerVisitor visitor)
        {
            visitor.VisitGraphicCard(this);
            foreach (var component in _children)
            {
                component.Accept(visitor);
            }
        }
    }

    // Leaf Components
    public abstract class LeafComponent : IComputerComponent
    {
        protected int _price;
        protected string _name;

        protected LeafComponent(string name, int price)
        {
            _name = name;
            _price = price;
        }

        public abstract void Accept(IComputerVisitor visitor);
        public int GetPrice() => _price;
        public string GetName() => _name;
    }

    public class Cpu : LeafComponent
    {
        public Cpu(int price) : base("CPU", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitCpu(this);
    }

    public class Keyboard : LeafComponent
    {
        public Keyboard(int price) : base("Keyboard", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitKeyboard(this);
    }

    public class Monitor : LeafComponent
    {
        public Monitor(int price) : base("Monitor", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitMonitor(this);
    }

    public class Ram : LeafComponent
    {
        public Ram(int price) : base("RAM", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitRam(this);
    }

    public class Rom : LeafComponent
    {
        public Rom(int price) : base("ROM", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitRom(this);
    }

    public class ExternalDisk : LeafComponent
    {
        public ExternalDisk(int price) : base("External Disk", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitExternalDisk(this);
    }

    public class Gpu : LeafComponent
    {
        public Gpu(int price) : base("GPU", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitGpu(this);
    }

    public class GraphicMemory : LeafComponent
    {
        public GraphicMemory(int price) : base("Graphic Memory", price) { }
        public override void Accept(IComputerVisitor visitor) => visitor.VisitGraphicMemory(this);
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Computer Architecture Price Calculator");
            Console.WriteLine("=====================================");

            // Create the computer structure
            var computer = new Computer();
            
            // Add main components
            computer.Add(new Cpu(400));
            computer.Add(new Keyboard(60));
            computer.Add(new Monitor(120));

            // Create and add memory components
            var memory = new Memory();
            memory.Add(new Ram(140));
            memory.Add(new Rom(90));
            memory.Add(new ExternalDisk(150));
            computer.Add(memory);

            // Create and add graphic card components
            var graphicCard = new GraphicCard();
            graphicCard.Add(new Gpu(200));
            graphicCard.Add(new GraphicMemory(100));
            computer.Add(graphicCard);

            // Create and use the memory price visitor
            var memoryVisitor = new MemoryPriceVisitor();
            computer.Accept(memoryVisitor);

            // Display the results
            Console.WriteLine("\nMemory Components Prices:");
            Console.WriteLine("------------------------");
            foreach (var component in memoryVisitor.GetVisitedComponents())
            {
                Console.WriteLine(component);
            }
            
            Console.WriteLine("\nTotal price of memory components: {0} AED", memoryVisitor.GetTotalMemoryPrice());
        }
    }
}