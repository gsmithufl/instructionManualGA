/*
Written by Garrett Smith
University of Florida Tech Comm for Eng. Class
Description: This program demonstrates how genetic algorithms work by writing
             a simple program that mutates char arrays towards all 'z' values.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instructionManualGA
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            bool isPrintTime = true;
            Population population = new Population(100, 50);
            Entity topEntity;
            do
            {
                population.calculateScore(isPrintTime);
                topEntity = population.entities.ElementAt(0);
                population.breedEntities();
                counter++;
                if((counter % 1000) == 0)
                {
                    isPrintTime = true;
                }
                else
                {
                    isPrintTime = false;
                }
            } while (topEntity.score < (0x7A * topEntity.lengthChar));
        }
    }

    class Population
    {
        public List<Entity> entities = new List<Entity> { };
        int numEntities_p = 0;
        int lengthChar_p = 0;

        public Population(int numEntities, int lengthChar)
        {
            Random random = new Random();
            numEntities_p = numEntities;
            lengthChar_p = lengthChar;

            for(int i = 0; i < numEntities; i++)
            {
                char[] chars = new char[lengthChar];
                for (int j = 0; j < lengthChar; j++)
                {
                    int num = random.Next(0, 26);
                    chars[j] = (char)('a' + num);
                }
                Entity entity = new Entity(chars);
                entities.Add(entity);
            }
        }

        public void calculateScore(bool isPrintTime)
        {
            foreach (Entity element in entities)
            {
                int score = 0;
                foreach (char ch in element.genome)
                {
                    score += (int)ch;
                }
                element.score = score;
            }
            entities = entities.OrderByDescending(s => s.score).ToList();
            if (isPrintTime)
            {
                Console.WriteLine("Element Score: " + entities.ElementAt(0).score);
                string str = new string(entities.ElementAt(0).genome);
                Console.WriteLine("Genome: " + str);
            }
        }

        public void breedEntities()
        {
            List<Entity> tempEntity = new List<Entity> { };
            Random random = new Random();
            tempEntity.Add(entities.ElementAt(0));
            for (int i = 1; i < numEntities_p; i++)
            {
                int num = (int)random.Next(0, 9);
                int max;
                int min;
                int in1;
                int in2;
                Entity entity1;
                Entity entity2;
                switch (num)
                {
                    //40% chance for top 10%
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        max = (int)(numEntities_p * .1);
                        in1 = random.Next(0, max);
                        in2 = random.Next(0, max);
                        entity1 = entities.ElementAt(in1);
                        entity2 = entities.ElementAt(in2);
                        tempEntity.Add(new Entity(entity1, entity2));
                    break;
                    //60% chance for top 10% to 20%
                    default:
                        max = (int)(numEntities_p * .2);
                        min = (int)(numEntities_p * .1);
                        in1 = random.Next(0, max);
                        in2 = random.Next(0, max);
                        entity1 = entities.ElementAt(in1);
                        entity2 = entities.ElementAt(in2);
                        tempEntity.Add(new Entity(entity1, entity2));
                    break;
                }
            }
            entities = tempEntity;
        }
    }

    class Entity
    {
        public char[] genome;
        public int score;
        public int lengthChar;

        public Entity(char[] array)
        {
            genome = array;
            score = 0;
            lengthChar = array.Length;
        }

        public Entity(Entity entity1, Entity entity2)
        {
            Random random = new Random();
            char[] chars = new char[entity1.lengthChar];
            for(int i = 0; i < entity1.lengthChar; i++)
            {
                int boolNum = (int)random.Next(0, 2);
                if (boolNum == 1)
                {
                    chars[i] = entity1.genome.ElementAt(i);
                }
                else
                {
                    chars[i] = entity2.genome.ElementAt(i);
                }
                //now we mutate, less than 0.5% chance to mutate
                double doubleNum = (double)random.NextDouble();
                if (doubleNum < 0.01)
                {
                    int num = random.Next(0, 26);
                    chars[i] = (char)('a' + num);
                }
            }
            genome = chars;
            score = 0;
            lengthChar = entity1.lengthChar;
        }
    }
}
