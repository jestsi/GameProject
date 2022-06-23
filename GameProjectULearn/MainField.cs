namespace GameProjectULearn;

public class MainField : Form
{
    private readonly BuildingStructs.Floor floor;
    private readonly BuildingStructs.Wall leftWall;
    private readonly BuildingStructs.Wall rightWall;
    
    public MainField()
    {
        ClientSize = new Size(300, 300);
        floor = new (30, 250, 200, 10, Color.Brown);

        const int wallsWidth = 5;
        leftWall = new BuildingStructs.Wall(0, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        rightWall = new BuildingStructs.Wall(ClientSize.Width-wallsWidth, 0, wallsWidth, ClientSize.Height, Color.Crimson);
        
        rightWall.PositionChange += (x, y, newX, newY) => Invalidate();
        leftWall.PositionChange += (x, y, newX, newY) => Invalidate();
        floor.PositionChange += (x, y, newX, newY) => Invalidate();

        Paint += (sender, args) =>
        {   
            args.Graphics.ResetTransform();
            args.Graphics.FillRectangle(new SolidBrush(floor._Color),floor.X, floor.Y, floor.Width, floor.Heigth);
            args.Graphics.DrawLine(new Pen(rightWall._Color, rightWall.Width), 
                rightWall.X, rightWall.Y, 
                rightWall.X, rightWall.Heigth);
            args.Graphics.DrawLine(new Pen(rightWall._Color, rightWall.Width), 
                rightWall.X, rightWall.Y, 
                rightWall.X, rightWall.Heigth);
        };
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
        {
            int stepSize = 5;
            if (e.KeyCode != Keys.Left)
                floor.Move(floor.X + stepSize, floor.Y);
            else 
                floor.Move(floor.X - stepSize, floor.Y);

        }
        
    }
}