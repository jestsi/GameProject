using System.ComponentModel;

namespace GameProjectULearn;

public enum TypeObject
{
    Player, 
    Building,
    Danger
}

public abstract class GameObjects
{
    public virtual int X { get; protected set; }
    public virtual int Y { get; protected set; }

    public virtual int Width { get; protected set; }

    public virtual int Height { get; protected set; }
    public virtual Color Color { get; set; }

    public virtual TypeObject Type { get; }

    public GameObjects()
    {
        X = 0;
        Y = 0;
        Color = Color.Black;
        Width = 1;
        Height = 1;
    }
    
    public GameObjects(int x, int y, int width, int height, Color color)
    {
        if (width < 0 || height < 0 || x < 0 || y < 0) 
            throw new ArgumentException($"the argument under the name " +
                                        $"{nameof(width)}," +
                                        $"{nameof(height)}," +
                                        $"{nameof(x)} or " +
                                        $"{nameof(y)} is invalid");

        X = x;
        Y = y;
        Color = color;
        Width = width;
        Height = height;

        PositionChange = delegate(int x, int y, int newX, int newY) {};
        ColorChanged = delegate(Color last, Color newColor) {}; 
        SizeChanged = delegate(int width, int height) { };
    }
    
    public virtual void Move(int x, int y)
    {
        if (x < 0 || y < 0 ) 
            throw new ArgumentException($"the argument under the name {nameof(x)} or {nameof(y)} is invalid");
        
        PositionChange?.Invoke(X, Y, x, y);
        X = x;
        Y = y;
    }

    public virtual async void MoveAsync(int x, int y) => await Task.Run(() => Move(x, y));
    public virtual async void ResizeAsync(int x, int y) => await Task.Run(() => Resize(x, y));
    
    public virtual void Resize(int width, int height)
    {
        if (width <= 0 || height <= 0) 
            throw new ArgumentException($"the argument under the name {nameof(width)} or {nameof(height)} is invalid");

        SizeChanged?.Invoke(width, height);
        Width = width;
        Height = height;
    }

    public virtual event Action<int, int, int, int> PositionChange;
    public virtual event Action<Color, Color> ColorChanged;
    public virtual event Action<int, int> SizeChanged;
}

public class BuildingStructs
{
    public sealed class Wall : GameObjects
    {
        private Color _color;
        private const TypeObject _Type = TypeObject.Building;
        private static readonly int defaultWidth = 5;


        public static int DefaultWidth => defaultWidth;
        public override event Action<int, int, int, int> PositionChange;
        public override event Action<Color, Color> ColorChanged;
        public override event Action<int, int> SizeChanged;


        public Wall(int x, int y, int width, int height, Color color) 
            : base(x, y, width, height, color)
        {
            
        }
        
        public override Color Color
        {
            get => _color;
            set
            {   
                ColorChanged?.Invoke(Color, value);
                _color = value;
            }
        }
        public override TypeObject Type => _Type;
    }

    public sealed class Platform : GameObjects
    {
        private Color _color;
        private const TypeObject _Type = TypeObject.Building;

        public Platform(int x, int y, int width, int height, Color color) : base(x, y, width, height, color)
        {
            
        }
        
        public void Move(int x, int y, bool limiter = true, Size? clientSize = null)
        {
            if (CheckOnNullArgumentsInMove(x, y, limiter, clientSize)) 
                throw new ArgumentException();

            if (limiter && 
                (x+Width) <= clientSize.Value.Width && 
                x >= 0 && y >= 0)
            {
                PositionChange?.Invoke(X, Y, x, y);
                X = x;
                Y = y;
                return;
            }

            if (limiter) return;
            
            PositionChange?.Invoke(X, Y, x, y);
            X = x;
            Y = y;
        }
        
        

        private static bool CheckOnNullArgumentsInMove(int x, int y, bool limiter, Size? clientSize)
        {
            return x < 0 || y < 0 ||
                ((limiter && clientSize == null) ||
                (!limiter && clientSize != null));
        }

        public async void MoveAsync(int x, int y, bool limiter = true, Size? clientSize = null)
        {
            await Task.Run(() => Move(x, y, limiter, clientSize));
        }

        public static void MovePlatform(in KeyEventArgs args, in Platform platform, Size clientSize, int stepSize = 10)
        {
            if (args == null) 
                throw new ArgumentNullException(nameof(args));
        
            int newFloorX;

            if (args.KeyCode is Keys.A or Keys.Left)
                newFloorX = -stepSize + platform.X;
            else
                newFloorX = stepSize + platform.X;
        
            if (newFloorX < 0 ) return;
        
            platform.Move(newFloorX, platform.Y, true, clientSize);
        }
        
        public async void MovePlatformAsync(object? sender, KeyEventArgs args, Size clientSize)
        {
            if (args.KeyCode is not (Keys.A or Keys.Left or Keys.D or Keys.Right))
                return;
            await Task.Run(() => MovePlatform(args, this, clientSize));
        }
        
        public override event Action<int, int, int, int>? PositionChange;
        public override event Action<Color, Color>? ColorChanged;
        public override event Action<int, int>? SizeChanged;

        public override Color Color
        {
            get => _color;
            set
            {
                ColorChanged?.Invoke(_color, value); 
                _color = value; 
            }
        }

        public override TypeObject Type => _Type;
    }
    
}