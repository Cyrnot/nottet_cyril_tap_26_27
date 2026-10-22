using System.Collections.Generic;
using Spectre.Console;

namespace GameSpectre.Render
{
    public class SpectreRenderManager
    {
        private Dictionary<(int X, int Y), char> _current_buffer = new Dictionary<(int X, int Y), char>();
        private Dictionary<(int X, int Y), char> _previous_buffer = new Dictionary<(int X, int Y), char>();

        public void Draw(int x, int y, string text)
        {
            if (x < 1 || y < 1)
            {
                return;
            }

            for (int index = 0; index < text.Length; index++)
            {
                _current_buffer[(x + index, y)] = text[index];
            }
        }

        public void Render()
        {
            foreach (var old_cell in _previous_buffer)
            {
                if (!_current_buffer.ContainsKey(old_cell.Key))
                {
                    DrawCharacter(old_cell.Key, ' ');
                }
            }

            foreach (var current_cell in _current_buffer)
            {
                if (!_previous_buffer.TryGetValue(
                        current_cell.Key, out char old_character)
                    || old_character != current_cell.Value)
                {
                    DrawCharacter(current_cell.Key, current_cell.Value);
                }
            }

            Dictionary<(int X, int Y), char> old_buffer = _previous_buffer;
            _previous_buffer = _current_buffer;
            _current_buffer = old_buffer;
            _current_buffer.Clear();
        }

        private void DrawCharacter((int X, int Y) position, char character)
        {
            AnsiConsole.Cursor.SetPosition(position.X, position.Y);
            AnsiConsole.Write(character.ToString());
        }
    }
}