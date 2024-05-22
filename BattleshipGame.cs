using System;
using System.Collections.Generic;
using Battleship.Util;
using Battleship.Util.InputHandling;
using Battleship.Util.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Battleship;

public class BattleshipGame : Game {
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private static Texture2D twoWideShipTexture;
    private static Texture2D strandedUnitTexture;
    private static Texture2D p1BoardTexture;
    private static Texture2D p2BoardTexture;

    private const int resWidth = 360;
    private const int resHeight = 396;
    private static int virtualWidth = 360;
    private static int virtualHeight = 396;
    private static Matrix scaleMatrix;
    private static Viewport viewport;
    public static float aspect = virtualWidth / (float)resWidth;
    public static Vector2 viewportBounds = new Vector2(viewport.X, viewport.Y);
    private static bool isResizing;
    public static readonly float[] rotations = { 0.5f, (float)(Math.PI/2f), (float)(Math.PI - 0.5f), (float)(Math.PI + 0.5f), (float)(Math.PI*3f/2f), -0.5f };

    private static bool leftClickedBefore = false;
    private static bool rightClickedBefore = false;
    private static bool middleClickedBefore = false;
    private static bool collision = false;

    private static SpriteFont pixelSCFont;

    private static Sprite p1BoardSprite;
    private static Sprite p2BoardSprite;
    private static Sprite twoWideShipSprite;
    private static Sprite strandedUnitSprite;
    public static List<Sprite> sprites;
    public static readonly Circle[][] tiles ={
        new Circle[]{ new Circle(12, new Vector2(26, 119)), new Circle(12, new Vector2(26, 143)), new Circle(12, new Vector2(26, 167)), new Circle(12, new Vector2(26, 191)), new Circle(12, new Vector2(26, 215)), new Circle(12, new Vector2(26, 239)), new Circle(12, new Vector2(26, 263)), new Circle(12, new Vector2(26, 287)) },
        new Circle[]{ new Circle(12, new Vector2(48, 107)), new Circle(12, new Vector2(48, 131)), new Circle(12, new Vector2(48, 155)), new Circle(12, new Vector2(48, 179)), new Circle(12, new Vector2(48, 203)), new Circle(12, new Vector2(48, 227)), new Circle(12, new Vector2(48, 251)), new Circle(12, new Vector2(48, 275)), new Circle(12, new Vector2(48, 299)) },
        new Circle[]{ new Circle(12, new Vector2(70, 95)), new Circle(12, new Vector2(70, 119)), new Circle(12, new Vector2(70, 143)), new Circle(12, new Vector2(70, 167)), new Circle(12, new Vector2(70, 191)), new Circle(12, new Vector2(70, 215)), new Circle(12, new Vector2(70, 239)), new Circle(12, new Vector2(70, 263)), new Circle(12, new Vector2(70, 287)), new Circle(12, new Vector2(70, 311)) },
        new Circle[]{ new Circle(12, new Vector2(92, 83)), new Circle(12, new Vector2(92, 107)), new Circle(12, new Vector2(92, 131)), new Circle(12, new Vector2(92, 155)), new Circle(12, new Vector2(92, 179)), new Circle(12, new Vector2(92, 203)), new Circle(12, new Vector2(92, 227)), new Circle(12, new Vector2(92, 251)), new Circle(12, new Vector2(92, 275)), new Circle(12, new Vector2(92, 299)), new Circle(12, new Vector2(92, 323)) },
        new Circle[]{ new Circle(12, new Vector2(114, 71)), new Circle(12, new Vector2(114, 95)), new Circle(12, new Vector2(114, 119)), new Circle(12, new Vector2(114, 143)), new Circle(12, new Vector2(114, 167)), new Circle(12, new Vector2(114, 191)), new Circle(12, new Vector2(114, 215)), new Circle(12, new Vector2(114, 239)), new Circle(12, new Vector2(114, 263)), new Circle(12, new Vector2(114, 287)), new Circle(12, new Vector2(114, 311)), new Circle(12, new Vector2(114, 335)) },
        new Circle[]{ new Circle(12, new Vector2(136, 59)), new Circle(12, new Vector2(136, 83)), new Circle(12, new Vector2(136, 107)), new Circle(12, new Vector2(136, 131)), new Circle(12, new Vector2(136, 155)), new Circle(12, new Vector2(136, 179)), new Circle(12, new Vector2(136, 203)), new Circle(12, new Vector2(136, 227)), new Circle(12, new Vector2(136, 251)), new Circle(12, new Vector2(136, 275)), new Circle(12, new Vector2(136, 299)), new Circle(12, new Vector2(136, 323)), new Circle(12, new Vector2(136, 347)) },
        new Circle[]{ new Circle(12, new Vector2(158, 47)), new Circle(12, new Vector2(158, 71)), new Circle(12, new Vector2(158, 95)), new Circle(12, new Vector2(158, 119)), new Circle(12, new Vector2(158, 143)), new Circle(12, new Vector2(158, 167)), new Circle(12, new Vector2(158, 191)), new Circle(12, new Vector2(158, 215)), new Circle(12, new Vector2(158, 239)), new Circle(12, new Vector2(158, 263)), new Circle(12, new Vector2(158, 287)), new Circle(12, new Vector2(158, 311)), new Circle(12, new Vector2(158, 335)), new Circle(12, new Vector2(158, 359)) },
        new Circle[]{ new Circle(12, new Vector2(180, 35)), new Circle(12, new Vector2(180, 59)), new Circle(12, new Vector2(180, 83)), new Circle(12, new Vector2(180, 107)), new Circle(12, new Vector2(180, 131)), new Circle(12, new Vector2(180, 155)), new Circle(12, new Vector2(180, 179)), new Circle(12, new Vector2(180, 203)), new Circle(12, new Vector2(180, 227)), new Circle(12, new Vector2(180, 251)), new Circle(12, new Vector2(180, 275)), new Circle(12, new Vector2(180, 299)), new Circle(12, new Vector2(180, 323)), new Circle(12, new Vector2(180, 347)) },
        new Circle[]{ new Circle(12, new Vector2(202, 47)), new Circle(12, new Vector2(202, 71)), new Circle(12, new Vector2(202, 95)), new Circle(12, new Vector2(202, 119)), new Circle(12, new Vector2(202, 143)), new Circle(12, new Vector2(202, 167)), new Circle(12, new Vector2(202, 191)), new Circle(12, new Vector2(202, 215)), new Circle(12, new Vector2(202, 239)), new Circle(12, new Vector2(202, 263)), new Circle(12, new Vector2(202, 287)), new Circle(12, new Vector2(202, 311)), new Circle(12, new Vector2(202, 335)), new Circle(12, new Vector2(202, 359)) },
        new Circle[]{ new Circle(12, new Vector2(224, 59)), new Circle(12, new Vector2(224, 83)), new Circle(12, new Vector2(224, 107)), new Circle(12, new Vector2(224, 131)), new Circle(12, new Vector2(224, 155)), new Circle(12, new Vector2(224, 179)), new Circle(12, new Vector2(224, 203)), new Circle(12, new Vector2(224, 227)), new Circle(12, new Vector2(224, 251)), new Circle(12, new Vector2(224, 275)), new Circle(12, new Vector2(224, 299)), new Circle(12, new Vector2(224, 323)), new Circle(12, new Vector2(224, 347)) },
        new Circle[]{ new Circle(12, new Vector2(246, 71)), new Circle(12, new Vector2(246, 95)), new Circle(12, new Vector2(246, 119)), new Circle(12, new Vector2(246, 143)), new Circle(12, new Vector2(246, 167)), new Circle(12, new Vector2(246, 191)), new Circle(12, new Vector2(246, 215)), new Circle(12, new Vector2(246, 239)), new Circle(12, new Vector2(246, 263)), new Circle(12, new Vector2(246, 287)), new Circle(12, new Vector2(246, 311)), new Circle(12, new Vector2(246, 335)) },
        new Circle[]{ new Circle(12, new Vector2(268, 83)), new Circle(12, new Vector2(268, 107)), new Circle(12, new Vector2(268, 131)), new Circle(12, new Vector2(268, 155)), new Circle(12, new Vector2(268, 179)), new Circle(12, new Vector2(268, 203)), new Circle(12, new Vector2(268, 227)), new Circle(12, new Vector2(268, 251)), new Circle(12, new Vector2(268, 275)), new Circle(12, new Vector2(268, 299)), new Circle(12, new Vector2(268, 323)) },
        new Circle[]{ new Circle(12, new Vector2(290, 95)), new Circle(12, new Vector2(290, 119)), new Circle(12, new Vector2(290, 143)), new Circle(12, new Vector2(290, 167)), new Circle(12, new Vector2(290, 191)), new Circle(12, new Vector2(290, 215)), new Circle(12, new Vector2(290, 239)), new Circle(12, new Vector2(290, 263)), new Circle(12, new Vector2(290, 287)), new Circle(12, new Vector2(290, 311)) },
        new Circle[]{ new Circle(12, new Vector2(312, 107)), new Circle(12, new Vector2(312, 131)), new Circle(12, new Vector2(312, 155)), new Circle(12, new Vector2(312, 179)), new Circle(12, new Vector2(312, 203)), new Circle(12, new Vector2(312, 227)), new Circle(12, new Vector2(312, 251)), new Circle(12, new Vector2(312, 275)), new Circle(12, new Vector2(312, 299)) },
        new Circle[]{ new Circle(12, new Vector2(334, 119)), new Circle(12, new Vector2(334, 143)), new Circle(12, new Vector2(334, 167)), new Circle(12, new Vector2(334, 191)), new Circle(12, new Vector2(334, 215)), new Circle(12, new Vector2(334, 239)), new Circle(12, new Vector2(334, 263)), new Circle(12, new Vector2(334, 287)) }
    };

    private static bool p1Turn = true;
    public static char[][] p1Board = {
        new char[]{ '1', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '2', '2', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-' }
    };
    public static char[][] p2Board = {
        new char[]{ '1', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '2', '2', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '-', '-', '-', '-', '-', '-', '-' }
    };

    public BattleshipGame() {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = resWidth;
        graphics.PreferredBackBufferHeight = resHeight;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Window.AllowAltF4 = true;
        Window.ClientSizeChanged += OnResize;
    }

    private void OnResize(object sender, EventArgs e) {
        if (!isResizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0) {
            isResizing = true;
            float oldAspect = aspect;
            Vector2 oldViewportBounds = viewportBounds;
            UpdateScaleMatrix();
            
            foreach (Sprite s in sprites) {
                if (s.GetShape() != null) {
                    s.GetShape().Rescale(aspect/oldAspect, viewportBounds, oldViewportBounds);
                }
            }

            foreach (Circle[] circles in tiles) {
                foreach (Circle c in circles) {
                    c.Rescale(aspect/oldAspect, viewportBounds, oldViewportBounds);
                }
            }
            
            isResizing = false;
        }
    }

    protected override void Initialize() {
        UpdateScaleMatrix();
        base.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        twoWideShipTexture = Content.Load<Texture2D>("TwoWideBoat");
        strandedUnitTexture = Content.Load<Texture2D>("StrandedUnit");
        p1BoardTexture = Content.Load<Texture2D>("P1Board");
        p2BoardTexture = Content.Load<Texture2D>("P2Board");
        pixelSCFont = Content.Load<SpriteFont>("PixelSCFont");

        p1BoardSprite = new Sprite(p1BoardTexture, null, Vector2.Zero, Vector2.Zero, 1f, 0, false, null, null, null, false, true, null);
        p2BoardSprite = new Sprite(p2BoardTexture, null, Vector2.Zero, Vector2.Zero, 1f, 0, false, null, null, null, false, false, null);
        twoWideShipSprite = new Sprite(twoWideShipTexture, "2", MiscMethods.TranslatePosToCoords('B', 1, 1), new Vector2(twoWideShipTexture.Width/2f, twoWideShipTexture.Height/2f), 1f, rotations[0], true, new RotatedRect(new Rectangle((MiscMethods.TranslatePosToCoords('B', 1, 1) - twoWideShipTexture.Bounds.Size.ToVector2()/2f).ToPoint(), (new Vector2(twoWideShipTexture.Width, twoWideShipTexture.Height)).ToPoint()), rotations[0]), null, null, true, true, new string[][]{new string[]{"B","1"}, new string[]{"B","2"}});
        strandedUnitSprite = new Sprite(strandedUnitTexture, "1", MiscMethods.TranslatePosToCoords('A',1,1), new Vector2(strandedUnitTexture.Width/2f - 2, strandedUnitTexture.Height/2f), 1f, 0f, false, new Circle(strandedUnitTexture.Width / 2f + 1, MiscMethods.TranslatePosToCoords('A', 1, 1)), null, null, true, true, new string[][]{new string[]{"A","1"}});
        
        sprites = new List<Sprite>(4){
            p1BoardSprite, p2BoardSprite, twoWideShipSprite, strandedUnitSprite
        };
    }

    protected override void Update(GameTime gameTime) {
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePos = mouseState.Position.ToVector2();
        if (IsActive) {
            if (p1Turn) {
                p1BoardSprite.SetVisibility(true);
                p2BoardSprite.SetVisibility(false);
            } else {
                p1BoardSprite.SetVisibility(false);
                p2BoardSprite.SetVisibility(true);
            }

            if (mouseState.MiddleButton == ButtonState.Pressed && !middleClickedBefore) {
                p1Turn = !p1Turn;
                middleClickedBefore = true;
            }else if (mouseState.MiddleButton == ButtonState.Released) {
                middleClickedBefore = false;
            }
            
            collision = false;
            foreach (Sprite s in sprites) {
                if (!s.GetTexture().Equals(p1BoardTexture) && !s.GetTexture().Equals(p2BoardTexture) && s.Contains(mousePos)) {
                    collision = true;
                }
            }
            
            foreach (Sprite s in sprites) {
                if (s.IsSelected() && s.ShouldFollowMouse()) {
                    s.SnapTipTo(mousePos - new Vector2(viewport.X, viewport.Y));
                }
            }
            
            if (mouseState.LeftButton == ButtonState.Pressed && !leftClickedBefore) {
                MouseInputHandler.HandleLeftMouseClick(mousePos);
                leftClickedBefore = true;
            }else if(mouseState.LeftButton == ButtonState.Released){
                leftClickedBefore = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed && !rightClickedBefore) {
                MouseInputHandler.HandleRightMouseClick();
                rightClickedBefore = true;
            }else if (mouseState.RightButton == ButtonState.Released) {
                rightClickedBefore = false;
            }
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);
        GraphicsDevice.Viewport = viewport;
        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: scaleMatrix);
        
        foreach (Sprite s in sprites) {
            if (s.ShouldDraw()) {
                spriteBatch.Draw(s.GetTexture(), s.GetPosition(), null, s.GetColor(), (float)s.GetRotation(), s.GetOrigin(), s.GetScale(), SpriteEffects.None, 0);
            }
        }
        
        string[] point = MiscMethods.CoordsToClosestPoint(Mouse.GetState().Position.ToVector2());
        
        spriteBatch.DrawString(pixelSCFont, $"Coords: {point[0][0]},{Int32.Parse(point[1])}\nMouse: {Mouse.GetState().Position}", new Vector2(10f,10f), Color.Black);
        spriteBatch.DrawString(pixelSCFont, $"Man: {strandedUnitSprite.GetShape().GetPosition()}\nBoat: {twoWideShipSprite.GetShape().GetPosition()}", new Vector2(10f, graphics.PreferredBackBufferHeight - 50f), Color.Black);
        
        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateScaleMatrix() {
        float sWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        float sHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        if (sWidth / resWidth > sHeight / resHeight) {
            float tempAspect = sHeight / resHeight;
            virtualWidth = (int)(tempAspect * resWidth);
            virtualHeight = (int)sHeight;
        } else {
            float tempAspect = sWidth / resWidth;
            virtualWidth = (int)sWidth;
            virtualHeight = (int)(tempAspect * resHeight);
        }
        aspect = virtualWidth / (float)resWidth;
        scaleMatrix = Matrix.CreateScale(aspect);
        viewport = new Viewport{
            X = (int)(sWidth / 2 - virtualWidth / 2f),
            Y = (int)(sHeight / 2 - virtualHeight / 2f),
            Width = virtualWidth,
            Height = virtualHeight,
            MinDepth = 0,
            MaxDepth = 1
        };
        viewportBounds = new Vector2(viewport.X, viewport.Y);
    }
}