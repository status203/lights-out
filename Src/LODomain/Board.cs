using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace LODomain
{
    public class Board
    {
        public const int BoardSize = 5;
        
        /// <summary>
        /// Whether NewGame has been called.
        /// </summary>
        /// <value></value>
        private bool GameCreated { get; set; }

        // Go with the dense 'jagged' array representation for ease of returning
        // enumerables externally.
        /// <summary>
        /// (Top left, rows first) 0-based jagged array representing the board.
        /// </summary>
        private List<List<LightState>> _lights;
        public IEnumerable<IEnumerable<LightState>> Lights
        {
            get
            {
                return _lights;
            }
        }

        /// <summary>
        /// Construct the board with all ligts off.
        /// </summary>
        public Board()
        {
            _lights = Enumerable.Range(1, 5).Select<int, List<LightState>>(
                i => Enumerable.Repeat(LightState.Off, 5).ToList()
            ).ToList();            
        }

        /// <summary>
        /// Randomises the lights of the board. Resulting board must have at
        /// least one light set to On.
        /// </summary>
        public void NewGame() {
            var rnd = new Random();
            for (var row = 0; row < BoardSize; ++row) {
                for (var col = 0; col < BoardSize; ++col) {
                    _lights[row][col] = (LightState)rnd.Next(2);
                }
            }

            // Check that at least one light is on, and if not set one.
            if (!Lights.Flatten().Contains(LightState.On)) {
                var row = rnd.Next(1, BoardSize) - 1;
                var col = rnd.Next(1, BoardSize) - 1;
                _lights[row][col] = LightState.On;
            }
        }

        /// <summary>
        /// Returns whether a board is complete, i.e. whether all
        /// lights are turned off.
        /// </summary>
        /// <returns></returns>
        public bool IsComplete() {
            // TODO: If performance became an issue, could track # of lit lights in MakeMove instead. (Could then also disallow further moves once completed)
            return GameCreated 
                && Lights.Flatten().Count(light => light == LightState.On) == 0;
        }

        /// <summary>
        /// Updates the board to reflect selecting the given light.
        /// </summary>
        /// <param name="light">The (Top Left) 0-based co-ord of the selected light.</param>
        public void MakeMove(LightLocation light) {
            var lightsToToggle = ValidLightsToToggle(light);

            foreach (var togglee in lightsToToggle) {
                var currentState = _lights[togglee.Row][togglee.Column];
                var newState = ToggledState(currentState);
                _lights[togglee.Row][togglee.Column] = newState;
            }

            GameCreated = true;
        }

        /// <summary>
        /// Helper method that, given a light state, returns the toggled
        /// version of that state.
        /// </summary>
        /// <param name="originalState"></param>
        /// <returns></returns>
        private LightState ToggledState(LightState originalState) =>
            originalState == LightState.On ? LightState.Off : LightState.On;

        /// <summary>
        /// Returns the locations of all the ligts that should be toggled
        /// if the specified location is used for a move.
        /// </summary>
        /// <param name="ligt">The (Top Left) 0-based co-ord of the selected light.</param>
        /// <returns></returns>
        private IEnumerable<LightLocation> ValidLightsToToggle(LightLocation ligt)
        {
            // Get all relevant light locations and then filter out the invalid ones
            return AllLightsToToggle(ligt)
                .Where(c => c.Row >= 0
                            && c.Row < BoardSize
                            && c.Column >= 0
                            && c.Column < BoardSize);
        }

        private IEnumerable<LightLocation> AllLightsToToggle(LightLocation light)
        {
            var deltas = new (int Row, int Column)[] {
                 (0,0), (0,1), (1, 0), (-1, 0), (0,-1)
            };
            return deltas.Select(
                delta => new LightLocation(
                     row: light.Row + delta.Row,
                     column: light.Column + delta.Column
            ));
        }

        public void ToggleLight(LightLocation light) {
            var currentState = _lights[light.Row][light.Column];
            var newState = ToggledState(currentState);
            _lights[light.Row][light.Column] = newState;
        }
    }
}