namespace GameProjectULearn;

public class MainField : Form
{
    private readonly BuildingStructs.Floor floor;
    private readonly BuildingStructs.Wall leftWall;
    private readonly BuildingStructs.Wall rightWall;
    
    public MainField()
    {
        ClientSize = new Size(300, 300);
        DoubleBuffered = true; // двойная буфферизация для того что бы исчесзло мерцание 
        
        var floorStartX = (ClientSize.Width / 4) + 10;
        var floorStartY = ClientSize.Height - 20;
        
        const int wallsWidth = 5; 

        floor = new (floorStartX, floorStartY, ClientSize.Width/2, ClientSize.Height, Color.Brown);
        leftWall = new BuildingStructs.Wall(0, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        rightWall = new BuildingStructs.Wall(ClientSize.Width, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        // нижние три строки нужны что бы при изменении позции объекта обновлялся холст(поле)
        rightWall.PositionChange += (x, y, newX, newY) => Invalidate();
        leftWall.PositionChange += (x, y, newX, newY) => Invalidate();
        floor.PositionChange += (x, y, newX, newY) => Invalidate();
        
        ClientSizeChanged += (sender, args) =>
        {
            floorStartX = (ClientSize.Width / 4) + 10;
            floorStartY = ClientSize.Height - 20;
            
            floor.Resize(ClientSize.Width/2, ClientSize.Height);
            floor.Move(floorStartX, floorStartY, true, ClientSize);
            
            rightWall.Move(ClientSize.Width, 0,  true, ClientSize);
            leftWall.Move(0, 0,  true, ClientSize);
            
            Invalidate();
        };
        
        Paint += (sender, args) =>
        {   
            args.Graphics.ResetTransform();
            args.Graphics.FillRectangle(new SolidBrush(floor._Color),floor.X, floor.Y, floor.Width, floor.Height);
            
            args.Graphics.DrawLine(new Pen(rightWall._Color, rightWall.Width), 
                rightWall.X, rightWall.Y, 
                rightWall.X, rightWall.Height);
            args.Graphics.DrawLine(new Pen(leftWall._Color, leftWall.Width), 
                leftWall.X, leftWall.Y, 
                leftWall.X, leftWall.Height);
        };
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Left or Keys.Right)
        {
            const int stepSize = 10;
            var newFloorX = e.KeyCode != Keys.Left ? floor.X + stepSize : floor.X - stepSize;
            if (newFloorX < 0) newFloorX = 0;
            else if (newFloorX > ClientSize.Width) newFloorX = ClientSize.Width;
            
            floor.Move(newFloorX, floor.Y, true, ClientSize);
            return;
        }
    }
}