using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    // https://gamedev.stackexchange.com/questions/162976/how-do-i-create-a-weighted-collection-and-then-pick-a-random-element-from-it
    public class WeightedRandomizer<T>
    {
        private struct Item
        {
            public double weigth;
            public T itemName;
        }

        private List<Item> items = new List<Item>();
        private double totalWeight;
        private Random rand = new Random();

        public void NewItem(T item, double weight)
        {
            totalWeight += weight;
            items.Add(new Item
            {
                itemName = item,
                weigth = totalWeight
            });
        }

        public T GetRandomItem()
        {
            double random = rand.NextDouble() * totalWeight; // Next double returns a floating-point between 0.0 and 1.0, which can be used to select a range from the total weight

            foreach (Item item in items)
            {
                if (item.weigth >= random)
                {
                    return item.itemName;
                }
            }

            return default;
        }
    }
}
