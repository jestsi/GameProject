namespace GameProjectULearn;

public class MainField : Form
{
    private readonly BuildingStructs.Platform _platform;
    private readonly BuildingStructs.Platform _platform_p2;
    private readonly BuildingStructs.Wall leftWall;
    private readonly BuildingStructs.Wall rightWall;

    public MainField()
    {
        ClientSize = new Size(300, 300);
        DoubleBuffered = true; // двойная буфферизация для того что бы исчесзло мерцание 
        
        const int wallsWidth = 5; 

        _platform = new ((ClientSize.Width / 4) + 10, ClientSize.Height - 20, ClientSize.Width/2, ClientSize.Height, Color.Brown);
        _platform_p2 = new(ClientSize.Width / 4 + 10, 0, ClientSize.Width / 2, 20, Color.Blue);
        
        leftWall = new(0, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        rightWall = new(ClientSize.Width, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        
        // нижние четыре строки нужны что бы при изменении позции объекта обновлялся холст(поле)
        rightWall.PositionChange += (x, y, newX, newY) => Invalidate();
        leftWall.PositionChange += (x, y, newX, newY) => Invalidate();
        _platform.PositionChange += (x, y, newX, newY) => Invalidate();
        _platform_p2.PositionChange += (x, y, newX, newY) => Invalidate();

        ClientSizeChanged += (obj, args) =>
        {
            _platform.MoveAsync((ClientSize.Width / 4) + 10, 
                ClientSize.Height - 20, true, ClientSize);
            _platform.ResizeAsync(ClientSize.Width/2, ClientSize.Height);
        };

        ClientSizeChanged += (obj, args) =>
        {
            _platform_p2.MoveAsync((ClientSize.Width / 4) + 10, 
                0, true, ClientSize);
            _platform.ResizeAsync(ClientSize.Width/2, ClientSize.Height);
        };

        ClientSizeChanged += (obj, args) =>
        {
            rightWall.MoveAsync(ClientSize.Width, 0);
            rightWall.ResizeAsync(wallsWidth, ClientSize.Height);
        };
        
        ClientSizeChanged += (obj, args) =>
        {
            leftWall.MoveAsync(0, 0);
            leftWall.ResizeAsync(wallsWidth, ClientSize.Height);
        };

        Paint += (sender, args) =>
        {   
            args.Graphics.ResetTransform();

            Draw(args, leftWall);
            Draw(args, rightWall);

            Draw(args, _platform);
            Draw(args, _platform_p2);

        };
    }
    private static void Draw(PaintEventArgs p, IGameObjects obj)
    {
        switch (obj.GetType().Name)
        {
            case "Platform" when obj is BuildingStructs.Platform pl:
                p.Graphics.FillRectangle(new SolidBrush(pl._Color),pl.X, pl.Y, pl.Width, pl.Height);
                break;
            case "Wall" when obj is BuildingStructs.Wall wl:
                p.Graphics.DrawLine(new Pen(wl._Color, wl.Width), 
                    wl.X, wl.Y, 
                    wl.X, wl.Height);
                break;
            default:
                throw new NotImplementedException();

        }
    }
    
    private void MovePlatform(KeyEventArgs e, BuildingStructs.Platform pl, int stepSize = 10)
    {
        if (e == null) throw new ArgumentNullException(nameof(e));
        
        var newFloorX = 0;

        if (e.KeyCode is Keys.A or Keys.Left)
            newFloorX = -stepSize + pl.X;
        else
            newFloorX = stepSize + pl.X;
        
        if (newFloorX < 0 ) return;
        
        pl.MoveAsync(newFloorX, pl.Y, true, ClientSize);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Left or Keys.Right)
            MovePlatform(e, _platform);
        else if (e.KeyCode is Keys.A or Keys.D)
            MovePlatform(e, _platform_p2);
        
    }
}