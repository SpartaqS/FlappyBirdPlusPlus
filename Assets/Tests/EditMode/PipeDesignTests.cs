using NUnit.Framework;
using UnityEngine;
using FlappyBirdPlusPlus;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class PipeDesignTests
    {
        [Test]
        public void PipesAreDefined()
        {
            List<Pipe> allPipes = Resources.LoadAll<Pipe>("Pipes").ToList();
            Assert.IsTrue(allPipes.Count > 0,"No pipe definitions found in\"Resources/Pipes\"");
        }

        [Test]
        public void ThereIsAlwaysAtLeastOnePipePossibleToSpawn()
        {
            List<Pipe> allPipes = Resources.LoadAll<Pipe>("Pipes").ToList();

            List<NotCoveredRange> uncoveredRanges = new List<NotCoveredRange> { { new NotCoveredRange(0,int.MaxValue)} };

            // foreach pipe check if it covers any of the uncovered ranges:
            //   if it does fully: delete the uncovered range.
            //   if it does partially: modify the existing range OR create two that represent the left out ranges
            //   if it doesn't cover at all: ignore the range and move on

            uncoveredRanges.Clear(); // TEMP

            if (uncoveredRanges.Count != 0)
            {
                Assert.Fail("At least one score range has no valid pipes for spawning. First encountered range: from " + uncoveredRanges[0].Min + " to " + uncoveredRanges[0].Max);
            }
        }

        private struct NotCoveredRange
        {
            public int Min;
            public int Max;
            public NotCoveredRange(int min, int max)
            {
                Min = min;
                Max = max;
            }
        }
    }
}
