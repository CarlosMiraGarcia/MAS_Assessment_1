using System;
using System.Collections.Generic;

namespace MAS_Assessment_1
{
    //https://gamedev.stackexchange.com/questions/162976/how-do-i-create-a-weighted-collection-and-then-pick-a-random-element-from-it
    //base code extracted from the link above
    public class WeightedRandomizer<T>
    {
        public struct Item
        {
            public double Weigth;
            public T ItemName;
        }

        private List<Item> items = new List<Item>(); //list of items, used to create to enable the random weigth function
        private double totalWeight;
        private static Random rand = new Random(); //creates a new instace of the class Random

        public void NewItem(T item, double weight)
        {
            totalWeight += weight; //adds the item's weight to the total weight
            items.Add(new Item //adds new item to the list
            {
                ItemName = item,
                Weigth = totalWeight
            });
        }

        public Item GetRandomItem()
        {
            double random = rand.NextDouble() * totalWeight; // Next double returns a floating-point between 0.0 and 1.0, which can be used to select a range from the total weight

            foreach (Item item in items) //checks the items in the item list and if the weight is higuer or equal to the random value, return that item
            {
                if (item.Weigth >= random)
                {
                    return item;
                }
            }

            return default;
        }
    }
}