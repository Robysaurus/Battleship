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
    private static Texture2D boardTexture;

    private const int resWidth = 360;
    private const int resHeight = 396;
    private static int virtualWidth = 360;
    private static int virtualHeight = 396;
    private static Matrix scaleMatrix;
    private static Viewport viewport;
    public static float aspect = virtualWidth / (float)resWidth;
    public static Vector2 viewportBounds = new Vector2(viewport.X, viewport.Y);
    private static bool isResizing;
    public static readonly float[] rotations = { 0.5f, (float)(Math.PI - 0.5), (float)(Math.PI + 0.5), -0.5f };

    private static bool leftClickedBefore = false;
    private static bool rightClickedBefore = false;
    private static bool collision = false;

    private static SpriteFont pixelSCFont;

    private static Sprite boardSprite;
    private static Sprite twoWideShipSprite;
    private static Sprite strandedUnitSprite;
    public static List<Sprite> sprites;

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
        boardTexture = Content.Load<Texture2D>("Board");
        pixelSCFont = Content.Load<SpriteFont>("PixelSCFont");

        boardSprite = new Sprite(boardTexture, Vector2.Zero, Vector2.Zero, 1f, 0, false, null, null, null, false);
        twoWideShipSprite = new Sprite(twoWideShipTexture, new Vector2(graphics.PreferredBackBufferWidth / 2.12f, graphics.PreferredBackBufferHeight / 2.005f), new Vector2(twoWideShipTexture.Width / 2f, twoWideShipTexture.Height / 2f), 1f, 0.5f, true, new RotatedRect(new Rectangle((new Vector2(graphics.PreferredBackBufferWidth / 2.12f - twoWideShipTexture.Width / 2f, graphics.PreferredBackBufferHeight / 2.005f - twoWideShipTexture.Height / 2f)).ToPoint(), (new Vector2(twoWideShipTexture.Width, twoWideShipTexture.Height)).ToPoint()), 0.5f), null, null, true);
        strandedUnitSprite = new Sprite(strandedUnitTexture, new Vector2(graphics.PreferredBackBufferWidth/4f, graphics.PreferredBackBufferHeight/3f), new Vector2(strandedUnitTexture.Width/2f, strandedUnitTexture.Height/2f), 1f, 0f, false, new Circle(strandedUnitTexture.Width / 2f, new Vector2(graphics.PreferredBackBufferWidth/4f, graphics.PreferredBackBufferHeight/3f)), null, null, true);
        
        sprites = new List<Sprite>(3){
            boardSprite, twoWideShipSprite, strandedUnitSprite
        };
    }

    protected override void Update(GameTime gameTime) {
        MouseState mouseState = Mouse.GetState();
        Vector2 mousePos = mouseState.Position.ToVector2();
        if (IsActive) {
            collision = false;
            foreach (Sprite s in sprites) {
                if (!s.GetTexture().Equals(boardTexture) && s.Contains(mousePos)) {
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
            spriteBatch.Draw(s.GetTexture(), s.GetPosition(), null, s.GetColor(), (float)s.GetRotation(), s.GetOrigin(), s.GetScale(), SpriteEffects.None, 0);
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