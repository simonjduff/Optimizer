﻿using System;
using Problem;
using Problem.ThermalDesign.App.Models.FirstModel;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new FirstModel();
            Console.WriteLine(model.Fitness(Idea.NewIdea()));
        }
    }
}