using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleship;

public class BattleshipGame : Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D twoWideShipTexture;
    private Texture2D background;

    private readonly int resWidth = 918;
    private readonly int resHeight = 515;
    private int virtualWidth = 918;
    private int virtualHeight = 515;
    private Matrix scaleMatrix;
    private Viewport viewport;
    private bool isResizing;

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

    public void OnResize(object sender, EventArgs e) {
        if (!isResizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0) {
            isResizing = true;
            UpdateScaleMatrix();
            isResizing = false;
        }
    }

    protected override void Initialize() {
        // TODO: Add your initialization logic here
        UpdateScaleMatrix();
        base.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        twoWideShipTexture = Content.Load<Texture2D>("TwoWideBoat");
        background = Content.Load<Texture2D>("background");
    }

    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            Exit();
        }

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.Black);
        GraphicsDevice.Viewport = viewport;
        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: scaleMatrix);
        spriteBatch.Draw(background, Vector2.Zero, Color.White);
        spriteBatch.Draw(twoWideShipTexture, new Vector2(graphics.PreferredBackBufferWidth / 2f, graphics.PreferredBackBufferHeight / 2f), null, Color.White, 0f, new Vector2(twoWideShipTexture.Width / 2f, twoWideShipTexture.Height / 2f), new Vector2(10f, 10f), SpriteEffects.None, 0f);
        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void UpdateScaleMatrix() {
        float sWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        float sHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        if (sWidth / resWidth > sHeight / resHeight) {
            float aspect = sHeight / resHeight;
            virtualWidth = (int)(aspect * resWidth);
            virtualHeight = (int)sHeight;
        } else {
            float aspect = sWidth / resWidth;
            virtualWidth = (int)sWidth;
            virtualHeight = (int)(aspect * resHeight);
        }
        scaleMatrix = Matrix.CreateScale(virtualWidth / (float)resWidth);
        viewport = new Viewport{
            X = (int)(sWidth / 2 - virtualWidth / 2f),
            Y = (int)(sHeight / 2 - virtualHeight / 2f),
            Width = virtualWidth,
            Height = virtualHeight,
            MinDepth = 0,
            MaxDepth = 1
        };
    }
}