using System;

namespace Problem
{
    public struct Idea
    {
        private static readonly Random Random = new Random();
        private readonly int[] _values;

        private static readonly (int min, int max)[] IdeaShape =
            {(1, 100), (1, 50), (1, 50), (3, 10), (1, 50), (3, 10)};

        public Idea(int[] values)
        {
            _values = values;
        }

        public static Idea NewIdea()
        {
            var values = new int[IdeaShape.Length];
            for (var i = 0; i < IdeaShape.Length; i++)
            {
                var shape = IdeaShape[i];
                values[i] = Random.Next(shape.min, shape.max + 1);
            }

            return new Idea(values);
        }

        public Idea Increment(int index)
        {
            var newArray = CloneValues();

            newArray[index]++;

            return new Idea(newArray);
        }

        private int[] CloneValues()
        {
            var newArray = new int[_values.Length];
            _values.CopyTo(newArray, 0);
            return newArray;
        }

        public Idea Decrement(int index)
        {
            var newArray = CloneValues();

            newArray[index]--;

            return new Idea(newArray);
        }

        public static implicit operator int[](Idea idea)
        {
            return idea.CloneValues();
        }
    }
}