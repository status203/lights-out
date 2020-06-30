using System.Linq;
using NUnit.Framework;

using Common;
using System.Collections.Generic;

namespace LODomain.Test
{
    public class BoardTests
    {
        /// <summary>
        /// Takes a list of (Row, Column) Pairs and returns a nested enumerable
        /// of light states representing a board with those and only those cells
        /// in an On state.
        /// </summary>
        /// <returns></returns>
        private List<List<LightState>> BoardLightsCreator(params (int Row, int Column)[] activated) {
            var lights = Enumerable.Range(1, 5).Select<int, List<LightState>>(
                i => Enumerable.Repeat(LightState.Off, 5).ToList()
            ).ToList(); 

            foreach(var light in activated) {
                lights[light.Row][light.Column] = LightState.On;
            }

            return lights;
        }

        [Test]
        public void Lights_ConstructedBoard_Creates5x5Board()
        {
            var board = new Board();

            var result = board.Lights;
            var expectedRowCount = 5;
            var expectedColumnCount = 5;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(expectedRowCount));
            foreach(var row in result) {
                Assert.That(row.Count(), Is.EqualTo(expectedColumnCount));
            }
        }

        [Test]
        public void Lights_ConstructedBoard_DefaultsToAllOff()
        {
            var board = new Board();

            var result = board.Lights.Flatten();

            Assert.That(result, Is.All.EqualTo(LightState.Off));
        }

        [Test]
        public void NewGame_AfterContstruction_ResultsInAtLeastOneLightOn()
        {
            var board = new Board();

            board.NewGame();
            var result = board.Lights.Flatten().Contains(LightState.On);

            Assert.That(result, Is.True);
        }

        [Test]
        public void MakeMove_OnInternalLight_TogglesExpectedCells()
        {
            var board = new Board();
            var light = new LightLocation(1, 2);

            board.MakeMove(light);
            var result = board.Lights.Flatten();
            var expected = BoardLightsCreator(new (int Row, int Column)[] {
                (1,2), (0,2), (2,2), (1,1), (1,3)
            }).Flatten();
            
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void MakeMove_OnCornerLight_TogglesExpectedCells()
        {
            var board = new Board();
            var light = new LightLocation(0, 0);

            board.MakeMove(light);
            var result = board.Lights.Flatten();
            var expected = BoardLightsCreator(new (int Row, int Column)[] {
                (0,0), (1,0), (0,1)
            }).Flatten();
            
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void MakeMove_OnEdgeLight_TogglesExpectedCells()
        {
            var board = new Board();
            var light = new LightLocation(0,2);
            
            board.MakeMove(light);
            var result = board.Lights.Flatten();
            var expected = BoardLightsCreator(new (int Row, int Column)[] {
                (0,2), (1,2), (0,1), (0,3)
            }).Flatten();
            
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void IsComplete_OnConstruction_ReturnsFalse()
        {
            var board = new Board();
            
            var result = board.IsComplete();

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsComplete_OnNewGame_ReturnsFalse()
        {
            var board = new Board();
            board.NewGame();

            var result = board.IsComplete();

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsComplete_ClearedBoard_ReturnsTrue() {
            var board = new Board();
            var location = new LightLocation(1,1);
            board.MakeMove(location);
            board.MakeMove(location);

            var result = board.IsComplete();

            Assert.That(result, Is.True);
        }
    }
}