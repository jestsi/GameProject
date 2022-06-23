using System.ComponentModel;

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
    public int Height { get; set; }
    public Color _Color { get; set; }

    public TypeObject Type { get; }

    public void Move(int x, int y, bool limiter = true, Size? clientSize = null);
    public void Resize(int width, int height);
    
    
    public event Action<int, int, int, int> PositionChange;
    public event Action<Color, Color> ColorChanged;
    public event Action<int, int> SizeChanged;
}

public class BuildingStructs
{
    public class Wall : IGameObjects
    {
        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private Color _color;
        private bool _isFake;
        private const TypeObject _Type = TypeObject.Building;

        public Wall(int x, int y, int width, int height, Color color)
        {
            if (width <= 0 || height <= 0 || x < 0 || y < 0) 
                throw new ArgumentException($"the argument under the name " +
                                            $"{nameof(width)}," +
                                            $"{nameof(height)}," +
                                            $"{nameof(x)} or " +
                                            $"{nameof(y)} is invalid");

            this._x = x;
            this._y = y;
            this._color = color;
            this._width = width;
            this._height = height;
            _isFake = false;
            
            PositionChange = delegate(int x, int y, int newX, int newY) {};
            ColorChanged = delegate(Color last, Color newColor) {}; 
            SizeChanged = delegate(int width, int height) { };
        }

        public void Move(int x, int y, bool limiter = true, Size? clientSize = null)
        {
            if (x < 0 || y < 0 || (limiter && clientSize == null)) 
                throw new ArgumentException($"the argument under the name {nameof(x)} or {nameof(y)} is invalid");

            if (limiter &&
                (x+_width) <= clientSize.Value.Width && 
                x >= 0 && 
                y >= 0)
            {
                PositionChange?.Invoke(_x, _y, x, y);
                _x = x;
                _y = y;
                return;
            }
            if (limiter) return;
            
            PositionChange?.Invoke(_x, _y, x, y);
            _x = x;
            _y = y;
            
        }

        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0) 
                throw new ArgumentException($"the argument under the name {nameof(width)} or {nameof(height)} is invalid");
            SizeChanged?.Invoke(width, height);
            _width = width;
            _height = height;
        }
        
        public event Action<int, int, int, int> PositionChange;
        public event Action<Color, Color> ColorChanged;
        public event Action<int, int> SizeChanged; 

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

        public int Height
        {
            get => _height;
            set => _height = value;
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
        private int _height;
        private Color _color;
        private const TypeObject _Type = TypeObject.Building;

        public Floor(int x, int y, int width, int height, Color color)
        {
            if (width <= 0 || height <= 0 || x < 0 || y < 0) 
                throw new ArgumentException($"the argument under the name " +
                                            $"{nameof(width)}," +
                                            $"{nameof(height)}," +
                                            $"{nameof(x)} or " +
                                            $"{nameof(y)} is invalid");
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _color = color;

            PositionChange = delegate(int x, int y, int newX, int newY) {};
            ColorChanged = delegate(Color last, Color newColor) {}; 
            SizeChanged = delegate(int width, int height) { };
        }

        public void Move(int x, int y, bool limiter = true, Size? clientSize = null)
        {
            if (x < 0 || y < 0 || (limiter && clientSize == null)) 
                throw new ArgumentException($"the argument under the name {nameof(x)} or {nameof(y)} is invalid");

            if (limiter &&
                (x+_width) <= clientSize.Value.Width && 
                x >= 0 && 
                y >= 0)
            {
                PositionChange?.Invoke(_x, _y, x, y);
                _x = x;
                _y = y;
                return;
            }
            if (limiter) return;
            
            PositionChange?.Invoke(_x, _y, x, y);
            _x = x;
            _y = y;
        }

        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0) 
                throw new ArgumentException($"the argument under the name {nameof(width)} or {nameof(height)} is invalid");
            SizeChanged?.Invoke(width, height);
            _width = width;
            _height = height;
        }
        
        public event Action<int, int, int, int> PositionChange;
        public event Action<Color, Color> ColorChanged;
        public event Action<int, int> SizeChanged; 

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
        
        public int Height
        {
            get => _height;
            set => _height = value;
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