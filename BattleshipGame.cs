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
    
    public static bool p1Turn = true;
    public static char[][] p1Board = {
        new char[]{ '1', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '2', '2', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '3', '3', '3', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ 't', 't', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', 't', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '4', '4', '4', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '5', '5', '5', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '5', '5', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
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
        new char[]{ '3', '3', '3', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ 't', 't', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', 't', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '4', '4', '4', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '5', '5', '5', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
        new char[]{ '-', '5', '5', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-', '-' },
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

        p1BoardSprite = new Sprite(p1BoardTexture, Vector2.Zero, Vector2.Zero, 1f, 0, false, null, null, null, false, true);
        p2BoardSprite = new Sprite(p2BoardTexture, Vector2.Zero, Vector2.Zero, 1f, 0, false, null, null, null, false, false);
        twoWideShipSprite = new Sprite(twoWideShipTexture, MiscMethods.TranslatePosToCoords('B', 1, 1), new Vector2(twoWideShipTexture.Width/2f, twoWideShipTexture.Height/2f), 1f, rotations[0], true, new RotatedRect(new Rectangle((MiscMethods.TranslatePosToCoords('B', 1, 1) - new Vector2(twoWideShipTexture.Width / 2f, twoWideShipTexture.Height / 2f)).ToPoint(), (new Vector2(twoWideShipTexture.Width, twoWideShipTexture.Height)).ToPoint()), rotations[0]), null, null, true, true);
        strandedUnitSprite = new Sprite(strandedUnitTexture, MiscMethods.TranslatePosToCoords('A',1,1), new Vector2(2+strandedUnitTexture.Width/2f, strandedUnitTexture.Height/2f), 1f, 0f, false, new Circle(strandedUnitTexture.Width / 2f + 1, MiscMethods.TranslatePosToCoords('A', 1, 1)), null, null, true, true);
        
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
                    s.MoveTo((mousePos - new Vector2(viewport.X, viewport.Y)) / aspect);
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
        
        spriteBatch.DrawString(pixelSCFont, $"Collision: {collision}\nMouse: {Mouse.GetState().Position}", new Vector2(10f,10f), Color.Black);
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