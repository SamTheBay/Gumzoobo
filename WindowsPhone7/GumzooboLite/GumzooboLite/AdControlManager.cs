using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#if WINDOWS_PHONE
    using Microsoft.Advertising;
    using Microsoft.Advertising.Mobile.Xna;
    using WP7XNASDK;
    using AdDuplex.Xna;
#endif

namespace BubbleGame
{
    public class AdControlManager
    {
        enum AdControlType
        {
            MSFT,
            Millenial,
            AdDuplex
        }

#if WINDOWS_PHONE 
        // MSFT Ad Control
        private DrawableAd bannerAd;
        
        // Millenial Ad Control
        MMAdView adView;

        // ad duplex
        AdManager adDuplex;

        int adCycleDuration = 30000; // 30 seconds in the minimum based on the rules
        int adCycleElapsed = 0;
#endif


        AdControlType adType = AdControlType.MSFT;
        bool testMode = false;
        bool showAds = true;
        Texture2D defaultAd;
        bool anyAdServed = false;
        int adServeFailoverTime = 30000;

        const string mainAdUnit = "27436";
        const string testAdUnit = "TextAd";
        static string[] trialAdUnits = { "77796", "77797", "77798", "77799", "77800", "77801", "77802", "77803", "77804",
                                         "77805", "77806", "77807", "77808", "77809", "77810", "77811", "77812", "77813",
                                         "77814", "77815", "77816", "77817", "77818"};


        public AdControlManager(GameComponentCollection Components, bool testMode = true)
        {
            this.testMode = testMode;

            // roll for an ad engine
            int roll = Util.Random.Next(0, 100);
            if (roll < 10)
            {
                adType = AdControlType.AdDuplex;
            }
            else
            {
                adType = AdControlType.MSFT;
            }

            // Create an ad manager for this game
#if WINDOWS_PHONE
            if (adType == AdControlType.MSFT)
            {
                if (testMode)
                {
                    AdGameComponent.Initialize(BubbleGame.sigletonGame, "test_client");
                }
                else
                {
                    AdGameComponent.Initialize(BubbleGame.sigletonGame, "f10769b5-c50b-4701-b552-20984ec4dbfa");
                }
                Components.Add(AdGameComponent.Current);
            }    
#endif
            
        }


        public void Load()
        {
#if WINDOWS_PHONE
            if (adType == AdControlType.MSFT)
            {
                if (testMode)
                {
                    bannerAd = AdGameComponent.Current.CreateAd(testAdUnit, new Rectangle(160, 400, 480, 80), false);
                }
                else
                {
                    // roll for trial ad unit
                    int roll = Util.Random.Next(0, 100);
                    if (roll < 5)
                    {
                        // we have one adunit for every category to keep tabs on which ones are giving the best cpms
                        bannerAd = AdGameComponent.Current.CreateAd(trialAdUnits[Util.Random.Next(0, trialAdUnits.Length)], new Rectangle(0, 720, 480, 80), false);
                    }
                    else
                    {
                        // this is our main ad unit which we will adjust for the best cpms
                        bannerAd = AdGameComponent.Current.CreateAd(mainAdUnit, new Rectangle(160, 400, 480, 80), false);
                    }
                }

                bannerAd.BorderEnabled = true;
                bannerAd.DropShadowEnabled = false;
                bannerAd.BorderColor = Color.White;
                bannerAd.Visible = true;
                bannerAd.ErrorOccurred += new EventHandler<AdErrorEventArgs>(bannerAd_ErrorOccurred);
                bannerAd.AdRefreshed += new EventHandler(bannerAd_AdRefreshed);
            }    
            else if (adType == AdControlType.Millenial)
            {
                adView = new MMAdView(BubbleGame.graphics.GraphicsDevice, new Vector2(160, 400), AdPlacement.BannerAdTop);
                adView.Apid = "60242";
                adView.RefreshTimer = 30;
                adView.DesiredAdHeight = 80;
                adView.DesiredAdWidth = 480;
            }
            else if (adType == AdControlType.AdDuplex)
            {
                adDuplex = new AdManager(BubbleGame.sigletonGame, "9907");
                adDuplex.LoadContent();
                adDuplex.RefreshInterval = 30;
            }
#endif

            defaultAd = BubbleGame.sigletonGame.Content.Load<Texture2D>("Textures/DefaultAd");
        }


        public void UnLoad()
        {
#if WINDOWS_PHONE

            // protect against ad control crash during resume
            if (adType == AdControlType.MSFT)
            {
                AdGameComponent.Current.RemoveAll();
            }

#endif
        }


#if WINDOWS_PHONE
        void bannerAd_AdRefreshed(object sender, EventArgs e)
        {
            anyAdServed = true;
        }

        void bannerAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            bannerAd.Refresh();
        }
#endif

        public void Update(GameTime gameTime)
        {
#if WINDOWS_PHONE
            if (adType == AdControlType.MSFT)
            {
                if (showAds)
                {
                    bannerAd.Visible = true;
                }
                else
                {
                    bannerAd.Visible = false;
                }


                bannerAd.DisplayRectangle = new Rectangle(160, 400, 480, 80);

                adCycleElapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (adCycleElapsed > adCycleDuration)
                {
                    adCycleElapsed = 0;
                    bannerAd.Refresh();
                }

                // handle failover
                if (!anyAdServed && gameTime.TotalGameTime.TotalMilliseconds > adServeFailoverTime)
                {
                    adType = AdControlType.AdDuplex;
                    adDuplex = new AdManager(BubbleGame.sigletonGame, "9907");
                    adDuplex.LoadContent();
                    adDuplex.RefreshInterval = 30;
                    bannerAd.Visible = false;
                }
            }
            else if (adType == AdControlType.Millenial)
            {
                if (showAds)
                {
                    // Update the adView with the current game time
                    adView.Update(gameTime);
                }
            }
            else if (adType == AdControlType.AdDuplex)
            {
                if (showAds)
                {
                    adDuplex.Visible = true;
                }
                else
                {
                    adDuplex.Visible = false;
                }

                adDuplex.Update(gameTime);
            }
#endif
        }

        public bool ShowAds
        {
            set { showAds = value; }
            get { return showAds; }
        }


        public void Draw(GameTime gameTime)
        {
#if WINDOWS_PHONE
            if (showAds)
            {
                // draw in a default add for when ads are not getting displayed by any network
                SpriteBatch spriteBatch = BubbleGame.screenManager.SpriteBatch;

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                spriteBatch.Draw(defaultAd, new Vector2(160, 400), Color.White);

                spriteBatch.End();

                if (adType == AdControlType.Millenial)
                {
                    spriteBatch.Begin();
                    adView.Draw(gameTime);
                    spriteBatch.End();
                }
                else if (adType == AdControlType.AdDuplex)
                {
                    adDuplex.Draw(spriteBatch, new Vector2(160, 400));
                }
            }
#endif
        }
    }
}
