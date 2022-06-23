using System.ComponentModel;
using System.Numerics;
using System.Security.Claims;

namespace GameProjectULearn;

public enum TypeObject
{
    Player,
    Building,
    Danger
}

public interface IGameObjects
{
    public int X { get; set; }
    public int Y { get; set; }

    public int Width { get; set; }
    public int Heigth { get; set; }
    public Color _Color { get; set; }

    public TypeObject Type { get; }
    
    public event Action<int, int, int, int> PositionChange;
    public event Action<Color, Color> ColorChanged; 
}

public class BuildingStructs
{
    public class Wall : IGameObjects
    {
        private int _x;
        private int _y;
        private int _width;
        private int _heigth;
        private Color _color;
        private bool _isFake;
        private const TypeObject _Type = TypeObject.Building;

        public Wall(int x, int y, int width, int heigth, Color color)
        {
            this._x = x;
            this._y = y;
            this._color = color;
            this._width = width;
            this._heigth = heigth;
            _isFake = false;
        }

        public void Move(int x, int y)
        {
            PositionChange?.Invoke(_x, _y, x, y);
            _x = x;
            _y = y;
        }
        
        public event Action<int, int, int, int> PositionChange;
        public event Action<Color, Color> ColorChanged; 
        
        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Heigth
        {
            get => _heigth;
            set => _heigth = value;
        }

        [DefaultValue(typeof(Color), "Color.Black")]
        public Color _Color
        {
            get => _color;
            set
            {   
                ColorChanged?.Invoke(_Color, value);
                _color = value;
            }
        }

        public TypeObject Type
        {
            get => _Type;
        }
    }

    public class Floor : IGameObjects
    {
        private int _x;
        private int _y;
        private int _width;
        private int _heigth;
        private Color _color;
        private const TypeObject _Type = TypeObject.Building;

        public Floor(int x, int y, int width, int heigth, Color color)
        {
            _x = x;
            _y = y;
            _width = width;
            _heigth = heigth;
            _color = color;

            PositionChange = delegate(int x, int y, int newX, int newY) {};
            ColorChanged = delegate(Color last, Color newColor) {}; 
        }

        public void Move(int x, int y)
        {
            PositionChange?.Invoke(_x, _y, x, y);
            _x = x;
            _y = y;
        }

        public event Action<int, int, int, int> PositionChange;
        public event Action<Color, Color> ColorChanged; 

        public int X
        {
            get => _x;
            set => _x = value;
        }
        
        public int Y
        {
            get => _y;
            set => _y = value;
        }
        
        public int Width
        {
            get => _width;
            set => _width = value;
        }
        
        public int Heigth
        {
            get => _heigth;
            set => _heigth = value;
        }
        
        public Color _Color
        {
            get => _color;
            set
            {
                ColorChanged?.Invoke(_color, value); 
                _color = value; 
            }
        }

        public TypeObject Type
            {
                get => _Type;
            }
            
    }
    
}