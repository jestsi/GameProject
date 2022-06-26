namespace GameProjectULearn;

public sealed class MainField : Form
{
    public MainField()
    {
        ClientSize = new Size(300, 300);
        DoubleBuffered = true; // двойная буфферизация для того что бы исчесзло мерцание 
    }

    protected override void OnLoad(EventArgs e)
    {
        const int wallsWidth = 5; 

        var platform = new BuildingStructs.Platform((ClientSize.Width / 4) + 10, ClientSize.Height - 20, ClientSize.Width/2, 20, Color.Brown);
        var platformP2 = new BuildingStructs.Platform(ClientSize.Width / 4 + 10, 0, ClientSize.Width / 2, 20, Color.Blue);
        
        BuildingStructs.Wall leftWall = new(0, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        BuildingStructs.Wall rightWall = new(ClientSize.Width, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        
        // нижние четыре строки нужны что бы при изменении позции объекта обновлялся холст(поле)
        rightWall.PositionChange += async (_, _, _, _) => Invalidate();
        leftWall.PositionChange += async (_, _, _, _) => Invalidate();
        platform.PositionChange += async (_, _, _, _) => Invalidate();
        platformP2.PositionChange += async (_, _, _, _) => Invalidate();

        ClientSizeChanged += async (_, _) =>
        {
            platform.MoveAsync((ClientSize.Width / 4) + 10, 
                ClientSize.Height - 20, true, ClientSize);
            platform.ResizeAsync(ClientSize.Width/2, platform.Height);
        };

        ClientSizeChanged += async (_, _) =>
        {
            platformP2.MoveAsync((ClientSize.Width / 4) + 10, 
                0, true, ClientSize);
            platformP2.ResizeAsync(ClientSize.Width/2, platformP2.Height);
        };

        ClientSizeChanged += async (_, _) =>
        {
            rightWall.MoveAsync(ClientSize.Width, 0);
            rightWall.ResizeAsync(wallsWidth, ClientSize.Height);
        };
        
        ClientSizeChanged += async (_, _) =>
        {
            leftWall.MoveAsync(0, 0);
            leftWall.ResizeAsync(wallsWidth, ClientSize.Height);
        };

        KeyDown += async (_, args) =>
        {
            if (args.KeyCode is Keys.A or Keys.D)
                platformP2.MovePlatformAsync(this, args, ClientSize);
            else if (args.KeyCode is Keys.Left or Keys.Right)
                platform.MovePlatformAsync(this, args, ClientSize);
        };
        
        Paint += async (_, args) =>
        {   
            args.Graphics.ResetTransform();

            Draw(args, leftWall);
            Draw(args, rightWall);

            Draw(args, platform);
            Draw(args, platformP2);

        };
        base.OnLoad(e);
    }

    private static void Draw(PaintEventArgs p, GameObjects obj)
    {
        switch (obj.GetType().Name)
        {
            case "Platform" when obj is BuildingStructs.Platform pl:
                p.Graphics.FillRectangle(new SolidBrush(pl.Color),pl.X, pl.Y, pl.Width, pl.Height);
                break;
            case "Wall" when obj is BuildingStructs.Wall wl:
                p.Graphics.DrawLine(new Pen(wl.Color, wl.Width), 
                    wl.X, wl.Y, 
                    wl.X, wl.Height);
                break;
            default:
                throw new NotImplementedException();

        }
    }

    
}