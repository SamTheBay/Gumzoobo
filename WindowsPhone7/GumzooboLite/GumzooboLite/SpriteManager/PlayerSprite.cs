using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace BubbleGame
{
    public class PlayerSprite : AnimatedSprite
    {
        public int lastHighScoreIndex = 11;
        private const float AccelerometerScale = 1.5f;
        protected int controllerIndex;
        protected Direction lastDirection = Direction.Right;
        protected Vector2 startPosition;
        protected bool isSelectingCharacer = false;
        protected bool hasSelectedCharacter = false;
        protected int currentCharacterSelection = 0;
        protected string name;
        protected Texture2D weaponBubble;
        protected Color lightColor;
        protected Color darkColor;


        // input
        protected bool previousMoveRightPressed = false;
        protected bool previousMoveLeftPressed = false;
        protected bool previousJumpPressed = false;
        protected bool previousSpecialPressed = false;
        protected bool previousShootPressed = false;
        protected bool jumpTriggered = false;
        protected bool jumpPressed = false;
        protected bool moveRightPressed = false;
        protected bool moveLeftPressed = false;
        protected bool shootTriggered = false;
        protected bool shootPressed = false;
        protected bool specialTriggered = false;
        protected bool specialPressed = false;
        protected bool moveRightTriggered = false;
        protected bool moveLeftTriggered = false;
        protected bool[] weaponTriggered = new bool[(int)Weapon.Num];
        protected bool[] weaponPressed = new bool[(int)Weapon.Num];
        protected bool[] previousWeaponPressed = new bool[(int)Weapon.Num];
        protected Rectangle WeaponRect = new Rectangle(0, 0, 100, 80);
        protected Rectangle jumpButton = new Rectangle(800 - 160, 480 - 120, 160, 120);
        protected Rectangle shootButton = new Rectangle(0, 480 - 120, 160, 120);
        protected Rectangle specialButton = new Rectangle(620 - 10, 480 - 80, 48 + 20, 80);


        // weapons
        protected Weapon currentWeapon = Weapon.Bubble;
        protected WeaponSprite[][] weapons = new WeaponSprite[(int)Weapon.Num][];
        protected int[] ammo = new int[(int)Weapon.Num];

        // characteristics
        protected float movementSpeed = 4f;
        protected float jumpSpeed = 10f;
        protected int jumpDuration = 25;
        protected int deadDuration = 1000;

        // state
        protected int jumpElapsed = 25;
        protected bool isDead = false;
        protected int deadElapsed = 0;
        protected bool canMove = true;
        protected bool forcedMove = false;
        protected Direction forcedDirection = Direction.Right;
        protected bool inSpecial = false;
        protected bool canChew = true;
        protected bool canShoot = true;
        protected bool wasInAir = false;
        protected bool damageInvulnerable = false;
        protected int damageInvulDuration = 1000;
        protected int damageInvulElapsed = 1000;
        protected int weaponBubbleElapsed = 500;
        protected int weaponBubbleDuration = 500;

        // stats
        protected int points = 0;
        protected int lastLifeAwardedPoints = 0;
        protected int lives = 3;
        protected int health = 100;
        protected float chew = 100f;
        protected int hasContinue = 1;

        // special item counters
        protected float speedBonusAmount = 4f;
        protected int speedBonusElapsed = 30000;
        protected int speedBonusDuration = 30000;
        protected bool isSpeedBonus = false;
        protected int ammoBonusElapsed = 15000;
        protected int ammoBonusDuration = 15000;
        protected bool isAmmoBonus = false;
        protected int invincibleBonusElapsed = 15000;
        protected int invincibleBonusDuration = 15000;
        protected bool isInvincibleBonus = false;
        protected int drawHealthElapsed = 1500;
        protected int drawHealthDuration = 1500;

        protected Rectangle characterFrame = new Rectangle(0, 0, 60, 60);
        protected String pointsString = ((int)0).ToString();
        protected Vector2 ThoughtOffset = new Vector2(0, 0);
        protected Vector2 ThoughtPosition = new Vector2(0, 0);
        protected Vector2 ThoughtWepPostition = new Vector2(0, 0);

        Vector2 lifeBarCorner;
        Rectangle lifeBarFill;
        Texture2D BlankBarHorz;
        Texture2D FullBarHorz;

        public PlayerSprite(int nControllerIndex, string nTextureName, Point nFrameDimensions, Point nFrameOrigin, int nFramesPerRow, Vector2 nSourceOffset, Vector2 nPosition)
            : base(nTextureName, nFrameDimensions, nFrameOrigin, nFramesPerRow, nSourceOffset, nPosition)
        {
            controllerIndex = nControllerIndex;

            Load();
        }

        public PlayerSprite(int nControllerIndex)
            : base("Seal", new Point(80, 80), new Point(40, 40), 14, new Vector2(40f, 40f), new Vector2(250f, 250f))
        {
            controllerIndex = nControllerIndex;
            lightColor = Color.White;
            darkColor = Color.Black;
        }


        public void Load()
        {
            BlankBarHorz = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "emptybarhoriz"));
            FullBarHorz = GameSprite.game.Content.Load<Texture2D>(System.IO.Path.Combine(@"Textures\\PlayerDisplay", "blankfullbarhoriz"));
            lifeBarCorner = new Vector2(400, 480 - 24 - FullBarHorz.Height / 2);
            lifeBarFill = new Rectangle(0, 0, 0, FullBarHorz.Height);

            for (int i = 0; i < (int)Weapon.Num; i++)
            {
                weaponPressed[i] = false;
                weaponTriggered[i] = false;
                previousWeaponPressed[i] = false;
            }

            weaponBubble = InternalContentManager.GetTexture("ThoughtBubble");

            // load up weapons for this player
            weapons[(int)Weapon.Bubble] = new BubbleWeapon[20];
            for (int i = 0; i < Bubbles.Length; i++)
            {
                Bubbles[i] = new BubbleWeapon(this);
            }
            ammo[(int)Weapon.Bubble] = WeaponSprite.MaxAmmo(Weapon.Bubble);
            
            //all ammo hack TODO: turn off
            //for (int i = 0; i < (int)Weapon.Num; i++)
            //{
            //    ammo[i] = WeaponSprite.MaxAmmo((Weapon)i);
            //}


            weapons[(int)Weapon.Cinnemon] = new CinnemonWeapon[5];
            for (int i = 0; i < Cinnemon.Length; i++)
            {
                Cinnemon[i] = new CinnemonWeapon(this);
            }

            weapons[(int)Weapon.Mint] = new MintWeapon[10];
            for (int i = 0; i < Mint.Length; i++)
            {
                Mint[i] = new MintWeapon(this);
            }

            weapons[(int)Weapon.Grape] = new GrapeWeapon[39];
            for (int i = 0; i < Grape.Length; i++)
            {
                Grape[i] = new GrapeWeapon(this);
            }

            weapons[(int)Weapon.ABC] = new ABCWeapon[30];
            for (int i = 0; i < ABC.Length; i++)
            {
                ABC[i] = new ABCWeapon(this);
            }

            weapons[(int)Weapon.Super] = new SuperBubbleWeapon[10];
            for (int i = 0; i < SuperBubble.Length; i++)
            {
                SuperBubble[i] = new SuperBubbleWeapon(this);
            }


            PlayAnimation("Idle", Direction.Right);
        }




        public Direction LastDirection
        {
            get { return lastDirection; }
        }

        public int PlayerIndex
        {
            get { return controllerIndex; }
        }

        public override void CollisionAction(GameSprite otherSprite)
        {
            if (otherSprite is Enemy)
            {
                // touched an enemy, should take damage
                Enemy enemy = (Enemy)otherSprite;
                if (enemy.IsDead == false && enemy.Stuck == false)
                {
                    Damage(40);
                }

            }
        }



        public void CaptureInput()
        {
            
            AccelerometerState accelState = Accelerometer.GetState();

            if (accelState.IsActive)
            {
                // set our movement speed
                double movement;
                movement = MathHelper.Clamp(-accelState.Acceleration.Y * AccelerometerScale, -1f, 1f);

                if (BubbleGame.sigletonGame.Window.CurrentOrientation == DisplayOrientation.LandscapeRight)
                {
                    movement *= -1;
                }

                // set values of move left or move right
                if (movement > 0.15f)
                    moveRightPressed = true;
                else
                    moveRightPressed = false;
                if (movement < -0.15f)
                    moveLeftPressed = true;
                else
                    moveLeftPressed = false;
            }
            else
            {
                if (InputManager.IsKeyPressed(Keys.Left))
                    moveLeftPressed = true;
                else
                    moveLeftPressed = false;
                if (InputManager.IsKeyPressed(Keys.Right))
                    moveRightPressed = true;
                else
                    moveRightPressed = false;

            }

            jumpPressed = InputManager.IsLocationPressed(jumpButton);
            shootPressed = InputManager.IsLocationPressed(shootButton);
            if (!previousMoveLeftPressed && moveLeftPressed)
            {
                moveLeftTriggered = true;
            }
            else
            {
                moveLeftTriggered = false;
            }

            if (!previousMoveRightPressed && moveRightPressed)
            {
                moveRightTriggered = true;
            }
            else
            {
                moveRightTriggered = false;
            }

            if (!previousJumpPressed && jumpPressed)
            {
                jumpTriggered = true;
            }
            else
            {
                jumpTriggered = false;
            }

            if (!previousShootPressed && shootPressed)
            {
                shootTriggered = true;
            }
            else
            {
                shootTriggered = false;
            }

            previousMoveRightPressed = moveRightPressed;
            previousMoveLeftPressed = moveLeftPressed;
            previousJumpPressed = jumpPressed;
            previousSpecialPressed = specialPressed;
            previousShootPressed = shootPressed;
            for (int i = 0; i < (int)Weapon.Num; i++)
            {
                WeaponRect.Y = 10 + i * (26 + 26);
                weaponPressed[i] = InputManager.IsLocationPressed(WeaponRect);
                if (weaponPressed[i] == true && previousWeaponPressed[i] == false)
                {
                    weaponTriggered[i] = true;
                }
                else
                {
                    weaponTriggered[i] = false;
                }
                previousWeaponPressed[i] = weaponPressed[i];
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CaptureInput();

            if (isDead)
            {
                deadElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (deadElapsed >= deadDuration)
                {
                    Resurrect();
                }
                else
                {
                    return;
                }
            }

            // special timers
            if (isInvincibleBonus)
            {
                invincibleBonusElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (invincibleBonusElapsed >= invincibleBonusDuration)
                {
                    EndInvinciblityBonus();
                }
            }
            if (isAmmoBonus)
            {
                ammoBonusElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (ammoBonusElapsed >= ammoBonusDuration)
                {
                    EndAmmoBonus();
                }
            }
            if (isSpeedBonus)
            {
                speedBonusElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (speedBonusElapsed >= speedBonusDuration)
                {
                    EndSpeedBonus();
                }
            }
            if (damageInvulnerable)
            {
                damageInvulElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (damageInvulElapsed >= damageInvulDuration)
                {
                    damageInvulnerable = false;
                }
            }

            if (drawHealthDuration > drawHealthElapsed)
            {
                drawHealthElapsed += gameTime.ElapsedGameTime.Milliseconds;
            }

            if (weaponBubbleElapsed < weaponBubbleDuration)
            {
                weaponBubbleElapsed += gameTime.ElapsedGameTime.Milliseconds;
            }

            // chew
            chew += gameTime.ElapsedGameTime.Milliseconds * 100 * (1f / WeaponSprite.ChewSpeed(currentWeapon));
            if (chew >= 100f)
            {
                chew = 100f;
            }

            // accumulate the desired direction from user input
            Vector2 desiredMovement = Vector2.Zero;

            // count jump time
            if (jumpElapsed < jumpDuration)
            {
                jumpElapsed++;
                if (!IsJumping)
                {
                    // jump has just ended
                    PlayAnimation("PlayerEndJump", lastDirection);
                }
            }

            // check if we have just landed
            if (wasInAir == true && !InAir())
            {
                // we have landed, switch to running (will be overriden by idle if we aren't moving)
                PlayAnimation("Player", lastDirection);
            }
            wasInAir = InAir();

            // put in gravity
            if (!IsJumping && InAir())
            {
                desiredMovement.Y += CanMove(Direction.Down, (int)Level.singletonLevel.Gravity);
                if (baseAnimationString == "Player")
                {
                    PlayAnimation("Idle", lastDirection);
                }
            }
            if (!IsJumping)
            {
                // jump has finished and grounded (can jump again)
                if (jumpTriggered && canMove)
                {
                    if (!InAir())
                    {
                        jumpElapsed = 0;
                        AudioManager.audioManager.PlaySFX("Jump");
                    }
                    else if (40 > CanMove(Direction.Down, 40, Level.singletonLevel.BubbleSprites)) // check if there is a bubble below us to jump with
                    {
                        jumpElapsed = 0;
                        desiredMovement.Y = 0;
                        AudioManager.audioManager.PlaySFX("BubbleJump");
                    }
                    if (jumpElapsed == 0)
                    {
                        PlayAnimation("PlayerStartJump", lastDirection);
                    }
                }

                // check if we are grounded on a super bubble, move with it if so
                Rectangle bottomRectangle = GetRectangle(Direction.Down, (int)Level.singletonLevel.Gravity);

                // check if there is an super bubble sprite below us
                List<GameSprite> superbubbles = Level.singletonLevel.SuperBubbleSprites;
                for (int i = 0; i < superbubbles.Count; i++)
                {
                    if (superbubbles[i].IsActive && bottomRectangle.Intersects(superbubbles[i].TopBox))
                    {
                        // move up with the super bubble
                        SuperBubbleWeapon superbubble = (SuperBubbleWeapon)superbubbles[i];
                        desiredMovement.Y = superbubble.Velocity.Y;
                        break;
                    }
                }
            }

            

            // player is no longer jumping
            if (jumpElapsed < jumpDuration && !jumpPressed && !(forcedMove && forcedDirection == Direction.Up))
            {
                // end jump
                jumpElapsed = jumpDuration;
                //int startFrame = 
                PlayAnimation("PlayerEndJump", lastDirection);

            }
            else if (jumpElapsed < jumpDuration)
            {
                // round out the jump to make a little more realistic looking
                desiredMovement.Y -= jumpSpeed * (((float)jumpDuration - (float)jumpElapsed) / (float)jumpDuration);
            }

            else if (moveRightTriggered)
            {
                // character moving right animation
                lastDirection = Direction.Right;
                if (!inSpecial && !InAir())
                {
                    PlayAnimation("Player", Direction.Right);
                }
                else if (!inSpecial)
                {
                    // change directions
                    FlipAnimationDirection(Direction.Right);
                }
            }
            else if (moveLeftTriggered)
            {
                // character moving left animation
                lastDirection = Direction.Left;
                if (!inSpecial && !InAir())
                {
                    PlayAnimation("Player", Direction.Left);
                }
                else if (!inSpecial)
                {
                    // change directions
                    FlipAnimationDirection(Direction.Left);
                }
            }


            // handle movement input
            if ((moveLeftPressed && canMove) || (forcedMove && forcedDirection == Direction.Left))
            {
                desiredMovement.X -= CanMove(Direction.Left, (int)movementSpeed);
                lastDirection = Direction.Left;
                if (this.currentAnimation != null && this.currentAnimation.Name != "PlayerLeft" && !InAir() && !inSpecial)
                {
                    PlayAnimation("Player", Direction.Left);
                }
            }
            if ((moveRightPressed && canMove) || (forcedMove && forcedDirection == Direction.Right))
            {
                desiredMovement.X += CanMove(Direction.Right, (int)movementSpeed);
                lastDirection = Direction.Right;
                if (this.currentAnimation != null && this.currentAnimation.Name != "PlayerRight" && !InAir() && !inSpecial)
                {
                    PlayAnimation("Player", Direction.Right);
                }
            }

            // Manage the animation based on movement
            if (desiredMovement == Vector2.Zero)
            {
                // character idle in this direction animation
                if (!inSpecial && !IsJumping)
                {
                    PlayAnimation("Idle", lastDirection);
                }
            }
            position += desiredMovement;
            

            // check for other actions
            if (shootTriggered && canShoot)
            {
                Shoot();
            }

            if (specialTriggered)
            {
                Special();
            }

            // handle switching weapons
            for (int i = 0; i < (int)Weapon.Num; i++)
            {
                if (weaponTriggered[i] && ammo[i] != 0)
                {
                    currentWeapon = (Weapon)i;
                    weaponBubbleElapsed = 0;
                }
            }

        }


        public WeaponSprite[] GetWeaponSet(Weapon wep)
        {
            return weapons[(int)wep];
        }


        private void Shoot()
        {
            WeaponSprite[] weaponset = GetWeaponSet(currentWeapon);

            // check to make sure we have enough chew
            if (chew < 100f || ammo[(int)currentWeapon] == 0)
            {
                return;
            }

            // look for a sprite that we can activate
            if (currentWeapon != Weapon.Grape)
            {
                for (int i = 0; i < weaponset.Length; i++)
                {
                    if (weaponset[i].IsActive == false)
                    {
                        weaponset[i].Activate();
                        if (!isAmmoBonus)
                            chew = 0f;
                        if (currentWeapon != Weapon.Bubble && !isAmmoBonus)
                            ammo[(int)currentWeapon]--;
                        AudioManager.audioManager.PlaySFX("BubbleShoot");
                        break;
                    }
                }
            }
            // if it is grapes then we launch a bunch
            else
            {
                GrapeWeapon[] grapeweps = new GrapeWeapon[3];
                int grapeindex = 0;
                for (int i = 0; i < weaponset.Length; i++)
                {
                    if (weaponset[i].IsActive == false)
                    {
                        grapeweps[grapeindex] = (GrapeWeapon)weaponset[i];
                        grapeindex++;
                        if (grapeindex >= 3)
                            break;
                    }
                }
                if (grapeindex >= 3)
                {
                    if (!isAmmoBonus)
                    {
                        chew = 0;
                        ammo[(int)currentWeapon]--;
                    }
                    for (int i = 0; i < grapeindex; i++)
                    {
                        grapeweps[i].Activate(i);
                    }
                    AudioManager.audioManager.PlaySFX("GrapeBubble");
                }
            }

            // check if we are out of this ammo now
            if (ammo[(int)currentWeapon] == 0)
            {
                currentWeapon = (Weapon)(((int)currentWeapon + 1) % (int)Weapon.Num);
                while (GetAmmo(currentWeapon) == 0)
                {
                    currentWeapon = (Weapon)(((int)currentWeapon + 1) % (int)Weapon.Num);
                }
            }
        }

        protected virtual void Special()
        {
            // implmeneted by individual characters
        }


        public override void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            bool isVisible = true;
            if (damageInvulnerable)
            {
                // make character flash after being damaged
                int flashtime = 50;
                if ((damageInvulElapsed / flashtime) % 2 == 0)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            if (isVisible)
            {
                base.Draw(spriteBatch, layerDepth);
            }

            //if (weaponBubbleElapsed < weaponBubbleDuration)
            //{
            //    ThoughtOffset.X = 0;
            //    ThoughtOffset.Y = 0;
            //    SpriteEffects effect = SpriteEffects.None;
            //    if (lastDirection == Direction.Right)
            //    {
            //        effect = SpriteEffects.FlipHorizontally;
            //        ThoughtOffset.X = -5;
            //    }
            //    ThoughtPosition.X = position.X + frameDimensions.X/2 - weaponBubble.Width/2;
            //    ThoughtPosition.Y = position.Y - weaponBubble.Height + reduceTop;
            //    spriteBatch.Draw(weaponBubble, ThoughtPosition, null, Color.White, 0f, Vector2.Zero, 1f, effect, 0f);
            //    ThoughtWepPostition.X = position.X + frameDimensions.X/2 - 27 + ThoughtOffset.X;
            //    ThoughtWepPostition.Y = position.Y - weaponBubble.Height / 2 - 24 + ThoughtOffset.Y + reduceTop;
            //    PlayerStatsDisplay.DrawWeaponIcon(ThoughtWepPostition, spriteBatch, this, 1f, currentWeapon);
            //}

            DrawLifeBar(spriteBatch);
        }


        public void RewardPoints(int points)
        {
            this.points += points;
            if (this.points > lastLifeAwardedPoints + 40000)
            {
                lives++;
                lastLifeAwardedPoints += 40000;
            }
            pointsString = this.points.ToString();
        }

        public void RewardAmmo(Weapon wep, int amount)
        {
            ammo[(int)wep] += amount;
            if (ammo[(int)wep] > WeaponSprite.MaxAmmo(wep))
            {
                ammo[(int)wep] = WeaponSprite.MaxAmmo(wep);
            }
        }


        public virtual void Damage(int damage)
        {
            if (isDead == false && isInvincibleBonus == false && damageInvulnerable == false)
            {
                // rumble controller
                BubbleGame.rumbleManager.StartRumbleBurst(controllerIndex);

                // perform damage
                health -= damage;
                if (health <= 0)
                    Die();
                else
                {
                    damageInvulnerable = true;
                    damageInvulElapsed = 0;
                    drawHealthElapsed = 0;
                }
            }
        }


        public virtual void Heal(int health)
        {
            if (isDead == false)
            {
                this.health += health;
                if (this.health > 100)
                    this.health = 100;
                if (health > 0)
                    drawHealthElapsed = 0;
            }
        }

        public virtual void Die()
        {
            lives -= 1;
            if (lives < 0)
            {
                Deactivate();
            }
            else
            {
                chew = 100f;
                jumpElapsed = jumpDuration;
                deadElapsed = 0;
                isDead = true;
                // end bonus's
                if (isSpeedBonus)
                {
                    EndSpeedBonus();
                }
                if (isInvincibleBonus)
                {
                    EndInvinciblityBonus();
                }
                if (isAmmoBonus)
                {
                    EndAmmoBonus();
                }
                PlayAnimation("PlayerDead", lastDirection);
            }
        }

        public override void Activate()
        {
            base.Activate();
            
            GamePad.SetVibration(BubbleGame.IntToPI(controllerIndex), 0f, 0f);

            lives = 3;
            health = 100;
            chew = 100f;
            isDead = false;
            deadElapsed = 0;
            points = 0;
            pointsString = points.ToString();
            lastLifeAwardedPoints = 0;
            jumpElapsed = jumpDuration;
            position.X = startPosition.X;
            position.Y = startPosition.Y;
        }

        public override void Deactivate()
        {
            HighScoresScreen.highlightIndex = BubbleGame.localHighScores.AddHighScore(SaveGameManager.CurrentOpenedGame.RecordName, (UInt32)points);
            base.Deactivate();
        }

        public void Resurrect()
        {
            position.X = startPosition.X;
            position.Y = startPosition.Y;
            isDead = false;
            deadElapsed = 0;
            health = 100;
            chew = 100;
            PlayAnimation("Idle", lastDirection);
        }

        public void StartSpeedBonus()
        {
            if (isDead == false && isActive == true)
            {
                if (!isSpeedBonus)
                {
                    movementSpeed += speedBonusAmount;
                }
                isSpeedBonus = true;
                speedBonusElapsed = 0;
            }
        }

        protected void EndSpeedBonus()
        {
            isSpeedBonus = false;
            movementSpeed -= speedBonusAmount;
        }

        public void StartAmmoBonus()
        {
            if (isDead == false && isActive == true)
            {
                isAmmoBonus = true;
                ammoBonusElapsed = 0;
                chew = 100f;
            }
        }

        protected void EndAmmoBonus()
        {
            isAmmoBonus = false;
        }

        public void StartInvincibilityBonus()
        {
            if (isDead == false && isActive == true)
            {
                isInvincibleBonus = true;
                invincibleBonusElapsed = 0;
            }
        }

        protected void EndInvinciblityBonus()
        {
            isInvincibleBonus = false;
        }


        protected void DrawLifeBar(SpriteBatch spriteBatch)
        {
            if (drawHealthDuration > drawHealthElapsed)
            {
                // Draw the life bar
                Color lifeColor = Color.LightGreen;
                if (Health < 30)
                {
                    lifeColor = Color.OrangeRed;
                }
                else if (Health < 60)
                {
                    lifeColor = Color.Yellow;
                }

                lifeBarCorner = position;
                lifeBarCorner.Y += -20 + reduceTop;
                lifeBarCorner.X += (frameDimensions.X - BlankBarHorz.Width) / 2;
                spriteBatch.Draw(BlankBarHorz, lifeBarCorner, Color.White);
                lifeBarFill.Width = (int)((float)FullBarHorz.Width * Health / 100f);
                spriteBatch.Draw(FullBarHorz,
                    lifeBarCorner,
                    lifeBarFill,
                    lifeColor);
            }
        }

        // Accessors
        public bool IsDead
        {
            get { return isDead; }
        }

        public int Health
        {
            get { return health; }
        }

        public float Chew
        {
            get { return chew; }
        }


        public int Points
        {
            get { return points; }
        }

        public String PointsString
        {
            get { return pointsString; }
        }


        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public BubbleWeapon[] Bubbles
        {
            get { return (BubbleWeapon[])weapons[(int)Weapon.Bubble]; }
        }

        public CinnemonWeapon[] Cinnemon
        {
            get { return (CinnemonWeapon[])weapons[(int)Weapon.Cinnemon]; }
        }

        public MintWeapon[] Mint
        {
            get { return (MintWeapon[])weapons[(int)Weapon.Mint]; }
        }

        public GrapeWeapon[] Grape
        {
            get { return (GrapeWeapon[])weapons[(int)Weapon.Grape]; }
        }

        public ABCWeapon[] ABC
        {
            get { return (ABCWeapon[])weapons[(int)Weapon.ABC]; }
        }

        public SuperBubbleWeapon[] SuperBubble
        {
            get { return (SuperBubbleWeapon[])weapons[(int)Weapon.Super]; }
        }

        public Weapon CurrentWeapon
        {
            get { return currentWeapon; }
        }

        public string Name
        {
            get { return name; }
        }

        public Color MyColor
        {
            get
            {
                return darkColor;
            }
        }

        public Color MyLightColor
        {
            get { return lightColor; }
        }


        public Vector2 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        public int GetAmmo(Weapon wep)
        {
            if (wep == Weapon.Num)
                return 0;
            return ammo[(int)wep];
        }

        public bool IsJumping
        {
            get { return !(jumpDuration == jumpElapsed); }
        }

        public int Continues
        {
            get { return hasContinue; }
            set { hasContinue = value; }

        }

        public override bool InAir()
        {
            bool inAir = base.InAir();
            if (inAir)
            {
                // check super bubbles as well
            }
            return inAir;
        }


        public bool IsSelecting
        {
            get { return isSelectingCharacer; }
            set { isSelectingCharacer = value; }
        }

        public bool HasSelected
        {
            get { return hasSelectedCharacter; }
        }

        public int CurrentSelection
        {
            get { return currentCharacterSelection; }
            set { currentCharacterSelection = value; }
        }

        public Texture2D CharacterTexture
        {
            get { return texture; }
        }

        public virtual Rectangle CharacterFrame
        {
            get { return characterFrame; }
        }

    }
}