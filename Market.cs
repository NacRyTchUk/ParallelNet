using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ParallelNet.SpeedTest
{
    public  class Market
    {
        public Market()
        {
            Store store = new Store();
            new Thread(() => new Producer(store).run()).Start();
            new Thread(() => new Consumer(store).run()).Start();
        }
    }

    // Класс Магазин, хранящий произведенные товары
    public class Store
    {
        private int product = 0;
        private object locker = new object();
        public void get()
        {
            Monitor.Enter(locker);
            while (product < 1)
            {
                try
                {
                    Monitor.Wait(locker);
                }
                catch (Exception e)
                {
                }
            }
            product--;
            Console.WriteLine("Покупатель купил 1 товар");
            Console.WriteLine("Товаров на складе: " + product);
            Monitor.Pulse(locker);
            Monitor.Exit(locker);
        }
        public void put()
        {
            Monitor.Enter(locker);
            while (product >= 3)
            {
                try
                {
                    Monitor.Wait(locker);
                }
                catch (Exception e)
                {
                }
            }
            product++;
            Console.WriteLine("Производитель добавил 1 товар");
            Console.WriteLine("Товаров на складе: " + product);
            Monitor.Pulse(locker);
            Monitor.Exit(locker);
        }
    }
    // класс Производитель
    public class Producer
    {

        Store store;
        public Producer(Store store)
        {
            this.store = store;
        }
        public void run()
        {
            for (int i = 1; i < 6; i++)
                store.put();
        }
    }
    // Класс Потребитель
    public class Consumer
    {

        Store store;

        public Consumer(Store store)
        {
            this.store = store;
        }

        public void run()
        {
            for (int i = 1; i < 6; i++)
                store.get();
            Console.WriteLine("=======================");
            Console.WriteLine("=======================");
        }
    }
}
