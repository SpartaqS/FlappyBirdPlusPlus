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

            foreach(Pipe currentPipe in allPipes)
            {
                for(int i = 0; i < uncoveredRanges.Count; i++)
                {
                    NotCoveredRange currentRange = uncoveredRanges[i];

                    bool coveredFromBottom = currentRange.Min >= currentPipe.MinimumScore; // pipe covers fully from the bottom
                    bool coveredFromTop = currentPipe.RestrictMaximumScore ? currentRange.Max <= currentPipe.MaximumScore : true; // pipe covers fully from the top (if we do not restrict, we cover up to 'infinity')

                    if(coveredFromBottom && coveredFromTop) // this pipe covers this range fully
                    {
                        uncoveredRanges.RemoveAt(i); // delete this range (uncoveredRanges.Count should update for the next check of our for loop)
                        --i; // decrement 'i' so we do not skip over the next uncovered range
                    }
                    else if(coveredFromBottom && currentPipe.MaximumScore > currentRange.Min) // only covered from the bottom - adjust this uncovered range's Min
                    {
                        currentRange.Min = currentPipe.MaximumScore + 1; // +1 since the pipe covers up to MinimumScore (inclusive)
                    }
                    else if (coveredFromTop && currentPipe.MinimumScore < currentRange.Max) // only covered from the top - adjust this uncovered range's Max
                    {
                        currentRange.Max = currentPipe.MinimumScore - 1; // -1 since the pipe covers up to MaximumScore (inclusive)
                    }
                    else // pipe's coverage is either a 'subset' of currentRange or is totally out of the range
                    {
                        if(currentRange.Max > currentPipe.MaximumScore && currentRange.Min < currentPipe.MinimumScore) // the pipe's range is a 'subset' of currentRange
                        {
                            uncoveredRanges.Insert(0, new NotCoveredRange(currentRange.Min, currentPipe.MinimumScore - 1)); // create a range that is to the left of currentPipe's range
                            currentRange.Min = currentPipe.MaximumScore + 1; // transform the examined range into a range that is to the roght of the currentPipe's range
                            ++i; // because we have added a new range at the beginning, we need to skip the current one (since it became i+1th)
                        } //else "Do nothing since this pipe's range does not effect this range"                        
                    }
                }
            }

            //uncoveredRanges.Clear(); // TEMP

            if (uncoveredRanges.Count != 0)
            {
                Assert.Fail("At least one score range has no valid pipes for spawning. First encountered range: from " + uncoveredRanges[0].Min + " to " + uncoveredRanges[0].Max);
            }
        }

        private class NotCoveredRange
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
