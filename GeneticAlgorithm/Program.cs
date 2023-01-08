using System.Diagnostics;

using static GeneticAlgorithm.Utils;

using GeneticAlgorithm.TravelingSalesman;

var tim = new Stopwatch();
var gaTime = new Stopwatch();
var group = new double[4];
var numIterations = 80;

var numCities = 12;
var populationSize = 50;
var percentToEliminate = 0.5;
var maxGeneration = numCities * 30;

var cm = new CitiesManager(numCities);

//Extensive ---------------------------------------------------------------------------------------
tim.Restart();
cm.BestOrder();
tim.Stop(); 

var extensiveTime = tim.Elapsed;

Console.WriteLine("Time spent on extensive solution: " + tim.Elapsed);
Console.WriteLine("BestScore: " + 1 / cm.bestScore);
Console.WriteLine("Best Solution: " + cm.DetailedBestSolution() + "\n");

gaTime.Restart();
for (var i = 0; i < numIterations; i++)
{
    //var cm = new CitiesManager(numCities);
    var geDecCross = new TravelingSalesmanGA(populationSize, percentToEliminate, maxGeneration, cm, CrossoverType.Decreasing, MutationType.Static, false, SelectionType.Roulette);
    var gaIncCross = new TravelingSalesmanGA(populationSize, percentToEliminate, maxGeneration, cm, CrossoverType.Increasing, MutationType.Static, false, SelectionType.Roulette);
    var gaDecCrossTop = new TravelingSalesmanGA(populationSize, percentToEliminate, maxGeneration, cm, CrossoverType.Decreasing, MutationType.Static, false, SelectionType.Top);
    var gaIncCrossTop = new TravelingSalesmanGA(populationSize, percentToEliminate, maxGeneration, cm, CrossoverType.Increasing, MutationType.Static, false, SelectionType.Top);

    Console.WriteLine("Test " + i + "\n");
    //Console.WriteLine(cm.DetailedCities() + "\n");

    /*
    //Extensive ---------------------------------------------------------------------------------------
    tim.Restart();
    cm.BestOrder();
    tim.Stop();

    Console.WriteLine("Time spent on extensive solution: " + tim.Elapsed);
    Console.WriteLine("BestScore: " + 1 / cm.bestScore);
    Console.WriteLine("Best Solution: " + cm.DetailedBestSolution() + "\n");
    */

    //GA Decreasing Crossover -----------------------------------------------------------------------
    tim.Restart();
    var gaSolution = (Route)geDecCross.Run();
    tim.Stop();

    Console.WriteLine("Time spent on GA solution (Dec Roulette): " + tim.Elapsed);
    Console.WriteLine("BestScore: " + cm.TotalDistance(gaSolution.order));
    Console.WriteLine("Best Solution: " + gaSolution.DetailedString() + "\n");

    group[0] += (double)(cm.TotalDistance(gaSolution.order) * cm.bestScore);

    //GA Increasing Crossover -------------------------------------------------------------------------
    tim.Restart();
    gaSolution = (Route)gaIncCross.Run();
    tim.Stop();

    Console.WriteLine("Time spent on GA solution (Inc Roulette): " + tim.Elapsed);
    Console.WriteLine("BestScore: " + cm.TotalDistance(gaSolution.order));
    Console.WriteLine("Best Solution: " + gaSolution.DetailedString() + "\n");

    group[1] += (double)(cm.TotalDistance(gaSolution.order) * cm.bestScore);

    //GA Decreasing Crossover Top -----------------------------------------------------------------------
    tim.Restart();
    gaSolution = (Route)gaDecCrossTop.Run();
    tim.Stop();

    Console.WriteLine("Time spent on GA solution (Dec Top): " + tim.Elapsed);
    Console.WriteLine("BestScore: " + cm.TotalDistance(gaSolution.order));
    Console.WriteLine("Best Solution: " + gaSolution.DetailedString() + "\n");

    group[2] += (double)(cm.TotalDistance(gaSolution.order) * cm.bestScore);

    //GA Increasing Crossover Top -------------------------------------------------------------------------
    tim.Restart();
    gaSolution = (Route)gaIncCrossTop.Run();
    tim.Stop();

    Console.WriteLine("Time spent on GA solution (Inc Top): " + tim.Elapsed);
    Console.WriteLine("BestScore: " + cm.TotalDistance(gaSolution.order));
    Console.WriteLine("Best Solution: " + gaSolution.DetailedString() + "\n");

    group[3] += (double)(cm.TotalDistance(gaSolution.order) * cm.bestScore);



    Console.WriteLine("Routes " + Route.allRoutes.Count);

    /*
    Console.WriteLine("\n\"All\" Solutions (" + tally.Count() + "):");
    foreach (KeyValuePair<int[], int> kvp in tally)
    {
        Console.Write("Solution ( ");
        foreach (int j in kvp.Key)
        {
            Console.Write(j + ", ");
        }
        Console.WriteLine(" ): " + kvp.Value);
    }
    */

    Console.WriteLine("\n\n");
}
gaTime.Stop();

for (var i = 0; i < group.Length; i++)
{
    group[i] /= numIterations;
}

Console.WriteLine(" % distance\n");
Console.WriteLine(" Dec Roulette " + group[0]);
Console.WriteLine(" Inc Roulette " + group[1]);
Console.WriteLine(" Dec Top " + group[2]);
Console.WriteLine(" Inc Top " + group[3] + "\n\n");

Console.WriteLine(" Time elapsed\n");
Console.WriteLine(" Extensive Solution: " + extensiveTime);
Console.WriteLine(" GA Solutions: " + gaTime.Elapsed);