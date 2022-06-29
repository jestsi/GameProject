namespace GameProjectULearn;

public sealed class MainField : Form
{
    private BuildingStructs.Platform platform;
    private BuildingStructs.Platform platformP2;

    private BuildingStructs.Wall leftWall;
    private BuildingStructs.Wall rightWall;

    public MainField()
    {
        ClientSize = new Size(300, 300);
        DoubleBuffered = true; // двойная буфферизация для того что бы исчесзло мерцание 
    }

    protected override void OnLoad(EventArgs e)
    { 

        platform = new BuildingStructs.Platform((ClientSize.Width / 4) + 10, ClientSize.Height - 20, ClientSize.Width/2, 20, Color.Brown);
        platformP2 = new BuildingStructs.Platform(ClientSize.Width / 4 + 10, 0, ClientSize.Width / 2, 20, Color.Blue);
        
        leftWall = new(0, 0, BuildingStructs.Wall.DefaultWidth, ClientSize.Height, Color.Crimson);
        rightWall = new(ClientSize.Width, 0, BuildingStructs.Wall.DefaultWidth, ClientSize.Height, Color.Crimson);

        DefaultEventsInitialization(platform, platformP2, leftWall, rightWall);

        KeyDown += (_, args) => Task.Run(() =>
        {
            if (args.KeyCode is Keys.A or Keys.D)
                platformP2.MovePlatformAsync(this, args, ClientSize);
            else if (args.KeyCode is Keys.Left or Keys.Right)
                platform.MovePlatformAsync(this, args, ClientSize);
        });
        
        
        Paint += (_, args) =>
        {   
            args.Graphics.ResetTransform();
            args.Graphics.Flush();

            Draw(args, leftWall);
            Draw(args, rightWall);

            Draw(args, platform);
            Draw(args, platformP2);

        };
        base.OnLoad(e);
    }

    private void DefaultEventsInitialization(params GameObjects[] objects)
    {
        foreach (var obj in objects)
        {
            obj.PositionChange += (_,_,_,_) => Invalidate();
        }

        ClientSizeChanged += (_, _) =>
        {
            platform.MoveAsync((ClientSize.Width / 4) + 10,
                ClientSize.Height - 20, true, ClientSize);
            platform.ResizeAsync(ClientSize.Width / 2, platform.Height);

            platformP2.MoveAsync((ClientSize.Width / 4) + 10,
                0, true, ClientSize);
            platformP2.ResizeAsync(ClientSize.Width / 2, platformP2.Height);
        };

        ClientSizeChanged += (_, _) =>
        {
            rightWall.MoveAsync(ClientSize.Width, 0);
            rightWall.ResizeAsync(BuildingStructs.Wall.DefaultWidth, ClientSize.Height);

            leftWall.MoveAsync(0, 0);
            leftWall.ResizeAsync(BuildingStructs.Wall.DefaultWidth, ClientSize.Height);
        };
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