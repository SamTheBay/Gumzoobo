using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BubbleGame
{
    public class LevelDetails
    {
        public int enemyStuckTime;
        public float bubbleRiseSpeed;
        public float gravity;
        // random generation speed
        // enemy movement speed factor
        

        public LevelDetails()
        {
            // set the defaults that levels can override if they want
            enemyStuckTime = 15000;
            bubbleRiseSpeed = 2f;
            gravity = 5f;
        }

    }




    public class LevelManager
    {
        const int levelNum = 100;
        string[][] levelLayouts = new string[levelNum][];
        LevelDetails[] levelDetails = new LevelDetails[levelNum];

        public LevelManager()
        {
            // setup default level details
            for (int i = 0; i < levelDetails.Length; i++)
            {
                levelDetails[i] = new LevelDetails();
            }

            // layout for all of the levels is here

            //-------------------------------------------------------------------
            // Start Section 1
            // Notes: Has help windows through out
            // Introduces: drone, bouncer, lazerbot, mint gum, super gum, shoes
            //-------------------------------------------------------------------


            int levelIndex = 0;
            string[] levellayout = levelLayouts[levelIndex] = new string[17];
            levellayout[0] =  "X--------------m--------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X------XXXXXXXXXXXXXXXXX------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X--d-----------------------D--X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X------X---------------X------X";
            levellayout[7] =  "XXXXXXXX---------------XXXXXXXX";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X----------d-------D----------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X------X---------------X------X";
            levellayout[12] = "X------XXXXXXXXXXXXXXXXX------X";
            levellayout[13] = "X--1--------------------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXX-------------XXXXXXXXX";







            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X--D---D---D---D---D----------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXX--------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X--------XXXXXXXXXXXXXXXXXXXXXX";
            levellayout[9]  = "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "XXXXXXXXXXXXXXXXXXXXXX--------X";
            levellayout[13] = "X------------1----------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "X--------XXXXXXXXXXXXXXXXXXXXXX";





            
            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X--------------c--------------X";
            levellayout[2] =  "X--d--d------------------D--D-X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X--------X-----------X--------X";
            levellayout[5] =  "XXXXXXXXXX-----------XXXXXXXXXX";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X------------D--D--D----------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X--------X-----------X--------X";
            levellayout[10] = "X--------XXXXXXXXXXXXX--------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X------------1----------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-d------------m------------D-X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X----X-------XXXXX-------X----X";
            levellayout[4] =  "XXXXXX-------------------XXXXXX";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X------B--B--B--B--B----------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X------------------------1----X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXX-----XXXXXXXXXXXXX";

            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--------------c--------------X";
            levellayout[1] =  "X--L----L-----------l------l--X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------X----X------------X";
            levellayout[4] =  "XXXXXXXXXXXXX----XXXXXXXXXXXXXX";
            levellayout[5] =  "X-----d--XXXX----XXXXX--D-----X";
            levellayout[6] =  "X--------XXXX----XXXXX--------X";
            levellayout[7] =  "X--------XXXX----XXXXX--------X";
            levellayout[8] =  "XXXXXXXXXXXXX----XXXXXXXXXXXXXX";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X----XXXXXXXXXXXXXXXXXXXXX----X";
            levellayout[13] = "X---D---XXXXX--1--XXXXX---d---X";
            levellayout[14] = "X-------XXXXX-----XXXXX-------X";
            levellayout[15] = "X-------XXXXX-----XXXXX-------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            //-------------------------------------------------------------------
            // End Section 1
            // Start Section 2
            // Notes: 
            // Introduces: Rocket Bots, Cinnemon Gum
            //-------------------------------------------------------------------


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X---L---L---L---L---L---------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X---1---------------------u---X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--------------1--------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X------------XXXXXX-----------X";
            levellayout[4] =  "X---------fD-X----X-d---------X";
            levellayout[5] =  "X------------X----X-----------X";
            levellayout[6] =  "X------------X----X-----------X";
            levellayout[7] =  "X--------XXXXX----XXXXX-------X";
            levellayout[8] =  "X------D--------------d-------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X----XXXXXX---------XXXXXX----X";
            levellayout[12] = "X---D------------------d------X";
            levellayout[13] = "X-----------------------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "XXXXXXXXXXXXXX----XXXXXXXXXXXXX";
            levellayout[16] = "X-----------------------------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "XXXXXXX-----------------XXXXXXX";
            levellayout[2] =  "XXXXXXXXXX-----------XXXXXXXXXX";
            levellayout[3] =  "X--------D--d---D---d---------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----XXXXXXXXXXXXXXXXXXX-----X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X-----d-----------------D-----X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "XXXX-----------------------XXXX";
            levellayout[11] = "XXXXXXX-----------------XXXXXXX";
            levellayout[12] = "XXXXXXXXXX-----------XXXXXXXXXX";
            levellayout[13] = "X-------------1---------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "X-----XXXXXXXXXXXXXXXXXXXX----X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X----------l-------L----------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X------X---------------X------X";
            levellayout[3] =  "X------XXXXXXXXXXXXXXXXX------X";
            levellayout[4] =  "X-----------XXXXXXX-----------X";
            levellayout[5] =  "X-----------X--d--X-----------X";
            levellayout[6] =  "X-----------X-----X-----------X";
            levellayout[7] =  "X-----------X-----X-----------X";
            levellayout[8] =  "X------XXXXXXXXXXXXXXXXX------X";
            levellayout[9] =  "X------X-D--X--D--X--d-X------X";
            levellayout[10] = "X------X----X-----X----X------X";
            levellayout[11] = "X------X----X-----X----X------X";
            levellayout[12] = "X------XXXXXXXXXXXXXXXXX------X";
            levellayout[13] = "X--1--------------------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";




            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------c-----X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X----XXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X--------------R----------D---X";
            levellayout[6] =  "X----X------------------------X";
            levellayout[7] =  "X----X-----------------X------X";
            levellayout[8] =  "X----X----R------------XXXXXXXX";
            levellayout[9] =  "X----X-----------------D------X";
            levellayout[10] = "X----X------------------------X";
            levellayout[11] = "X----X------------X-----------X";
            levellayout[12] = "X----X------------XXXXXXXXXXXXX";
            levellayout[13] = "X----X-----------D------------X";
            levellayout[14] = "X----X------------------------X";
            levellayout[15] = "X----X-----X------------------X";
            levellayout[16] = "X----X-----XXXXXXXXXXXXXXXXXXXX";



            //-------------------------------------------------------------------
            // End Section 2
            // Start Section 3
            // Notes: 
            // Introduces: Invisibots, Abc gum
            //-------------------------------------------------------------------

            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X--u--------------------------X";
            levellayout[2] =  "X--------R--------------------X";
            levellayout[3] =  "X------------------r----------X";
            levellayout[4] =  "XXXXX---------------------XXXXX";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------R-----------X";
            levellayout[8] =  "XXXXX---------------------XXXXX";
            levellayout[9] =  "X-----------r-----------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "XXXXX---------------------XXXXX";
            levellayout[13] = "X--------------------------1--X";
            levellayout[14] = "X----------R------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXX---------------------XXXXX";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X--------------1--------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X--I----D-----m----d------i---X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X--------D----------d---------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X----------------B--BI-B--BI--X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X---------------XXXXXXXXXXXXXXX";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X-------------1---------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXX-------------X";




            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X---XX------------------XX----X";
            levellayout[1] =  "X----XX--I----------i--XX-----X";
            levellayout[2] =  "X-----XX--------------XX------X";
            levellayout[3] =  "X------XX--I------i--XX-------X";
            levellayout[4] =  "X-------XX----------XX--------X";
            levellayout[5] =  "X--------XX--------XX---------X";
            levellayout[6] =  "X---------XX------XX----------X";
            levellayout[7] =  "X----------XX----XX-----------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X----R-------------------r----X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X----------i--XXX--I----------X";
            levellayout[13] = "X------------X1--X------------X";
            levellayout[14] = "X-----------XX---XX-----------X";
            levellayout[15] = "X----------XXX---XXX----------X";
            levellayout[16] = "X---------XXXXXXXXXXX---------X";






            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X--u-----------------------u--X";
            levellayout[3] =  "X------------------r----------X";
            levellayout[4] =  "X-------R---------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X----------r-----------R------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----------XXXXXXX-----------X";
            levellayout[13] = "X-----------X-1---X-----------X";
            levellayout[14] = "X---R-------X-----X-----------X";
            levellayout[15] = "X-----------X-----X-----------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            //-------------------------------------------------------------------
            // End Section 3
            // Start Section 4
            // Notes:
            // Introduces: Spikey Bots, Crystal Ball, grape gum 
            //-------------------------------------------------------------------



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X---------S----S-----S-----m--X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----XXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[4] =  "X-c-------s----s-----s--------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "XXXXXXXXXXXXXXXXXXXXXXXXXX----X";
            levellayout[8] =  "X--------------------------c--X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X----XXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[12] = "X--m-----------------1--------X";
            levellayout[13] = "X-----------------------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "XXXXXXXXXXXXXXXXXXXXXXXXXX----X";
            levellayout[16] = "X-----------------------------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[1] =  "X-----X--------S--------X-----X";
            levellayout[2] =  "X-----X-----------------X-----X";
            levellayout[3] =  "X-----X-----------------X-----X";
            levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[5] =  "X-----S-----X-----X-----S-----X";
            levellayout[6] =  "X-----------X-----X-----------X";
            levellayout[7] =  "X-----------X-----X-----------X";
            levellayout[8] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[9] =  "X--I--X-----X--I--X-----X--I--X";
            levellayout[10] = "X-----X-----X-----X-----X-----X";
            levellayout[11] = "X-----X-----X-----X-----X-----X";
            levellayout[12] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[13] = "X--1--------------------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";




            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--g--------------------------X";
            levellayout[1] =  "X--------------I--------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X----------X-------X----------X";
            levellayout[4] =  "XXXXXXX----XXXXXXXXX----XXXXXXX";
            levellayout[5] =  "X--i--X-----------------X-I---X";
            levellayout[6] =  "X-----X-----------------X-----X";
            levellayout[7] =  "X-----X-----------------X-----X";
            levellayout[8] =  "XXXXXXX-----R---r-------XXXXXXX";
            levellayout[9] = "X--i--X-----------------X-I---X";
            levellayout[10] = "X-----X-----------------X-----X";
            levellayout[11] = "X-----X-----------------X-----X";
            levellayout[12] = "XXXXXXX-----------------XXXXXXX";
            levellayout[13] = "X------------------------1----X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXX-----------------XXXXXXX";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X---------------XX------------X";
            levellayout[1] =  "X----------------XX---1-------X";
            levellayout[2] =  "X-----r-----------XX----------X";
            levellayout[3] =  "X------------------XX---------X";
            levellayout[4] =  "X--------XX---------XX--------X";
            levellayout[5] =  "X---------XX---R-----XX-------X";
            levellayout[6] =  "X----------XX---------XX------X";
            levellayout[7] =  "X-----------XX----------------X";
            levellayout[8] =  "X-----R------XX---------------X";
            levellayout[9] =  "X-------------XX--------------X";
            levellayout[10] = "XXX------------XX-----r-------X";
            levellayout[11] = "X-XX------------XX------------X";
            levellayout[12] = "X--XX------------XX-----------X";
            levellayout[13] = "X---XX------------XX----------X";
            levellayout[14] = "X----XX------------XX---------X";
            levellayout[15] = "X--g--XX------------XX--------X";
            levellayout[16] = "X------XX------------XX-------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "XXX-------------------------XXX";
            levellayout[1] =  "X-XX------R------r---------XX-X";
            levellayout[2] =  "X--XX---------------------XX--X";
            levellayout[3] =  "X---XX-------------------XX---X";
            levellayout[4] =  "X----XX-----------------XX----X";
            levellayout[5] =  "X-----XX---------------XX-----X";
            levellayout[6] =  "X------XX------R------XX------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X---R----------------------r--X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X------XXXXXX-----XXXXXX------X";
            levellayout[11] = "X-----XX---------------XX-----X";
            levellayout[12] = "X----XX-----------------XX----X";
            levellayout[13] = "X---XX---------1---------XX---X";
            levellayout[14] = "X--XX---------------------XX--X";
            levellayout[15] = "X-XX-----------------------XX-X";
            levellayout[16] = "XXX--------XXXXXXXXX--------XXX";


            //-------------------------------------------------------------------
            // End Section 4
            // Start Section 5
            // Notes:
            // Introduces: Hunter Bot, Necklace
            //-------------------------------------------------------------------

            //levellayout = levelLayouts[++levelIndex] = new string[18];
            //levellayout[0] =  "X-----------------------------X";
            //levellayout[1] =  "X-----------------------------X";
            //levellayout[2] =  "X-----------------------------X";
            //levellayout[3] =  "X-----------------------------X";
            //levellayout[4] =  "X-----------------------------X";
            //levellayout[5] =  "X-----------------------------X";
            //levellayout[6] =  "X-----------------------------X";
            //levellayout[7] =  "X-----------------------------X";
            //levellayout[8] =  "X-----------------------------X";
            //levellayout[9] =  "X-----------------------------X";
            //levellayout[10] = "X-----------------------------X";
            //levellayout[11] = "X-----------------------------X";
            //levellayout[12] = "X-----------------------------X";
            //levellayout[13] = "X-----------------------------X";
            //levellayout[14] = "X-----------------------------X";
            //levellayout[15] = "X-----------------------------X";
            //levellayout[16] = "X-----------------------------X";
            //levellayout[17] = "X-----------------------------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--------------j--------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X--------XXXXXXXXXXXXX--------X";
            levellayout[3] =  "X--------X-----------X--------X";
            levellayout[4] =  "X--------X--H-----H--X--------X";
            levellayout[5] =  "X--------X-----------X--------X";
            levellayout[6] =  "X--------X-----X-----X--------X";
            levellayout[7] =  "X--------X---XXXXX---X--------X";
            levellayout[8] =  "X--------X-----X-----X--------X";
            levellayout[9] =  "X--------X--H-----H--X--------X";
            levellayout[10] = "X--------X-----------X--------X";
            levellayout[11] = "X--------X-----------X--------X";
            levellayout[12] = "X--------XXXXXXXXXXXXX--------X";
            levellayout[13] = "X--------------1--------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "X--------XXXXXXXXXXXXX--------X";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----X--H--X-----X--H--X-----X";
            levellayout[4] =  "X-----X-----X-----X-----X-----X";
            levellayout[5] =  "X-----X-----X-----X-----X-----X";
            levellayout[6] =  "X-----XXXXXXX-----XXXXXXX-----X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------X--H--X-----------X";
            levellayout[11] = "X-----------X-----X-----------X";
            levellayout[12] = "X-----------X-----X-----------X";
            levellayout[13] = "X---1-------XXXXXXX-----------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXX-----------------XXXXXXX";




            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[1] =  "X-------------X---------------X";
            levellayout[2] =  "X-------------X---------------X";
            levellayout[3] =  "X-------------X---------------X";
            levellayout[4] =  "X------X------X------X--------X";
            levellayout[5] =  "X------X------X------X---r----X";
            levellayout[6] =  "X------X--R---X------X--------X";
            levellayout[7] =  "X------X------X------X--------X";
            levellayout[8] =  "X------X------X--r---X--------X";
            levellayout[9] =  "X------X------X------X--------X";
            levellayout[10] = "X------X------X------X--------X";
            levellayout[11] = "X------X------X------X--------X";
            levellayout[12] = "X------X------X------X--------X";
            levellayout[13] = "X--1---X---r---------X---H----X";
            levellayout[14] = "X------X-------------X--------X";
            levellayout[15] = "X------X-------------X--------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";




            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X--------------1--------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X------------r----------------X";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X-----------------------r-----X";
            levellayout[14] = "X----R-----R------------------X";
            levellayout[15] = "X-------------------R---------X";
            levellayout[16] = "X-----------------------------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X--------------h--------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X----r--------------------R---X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X---------XXXXXXXXXXX---------X";
            levellayout[8] =  "X---------X----1----X---------X";
            levellayout[9] =  "X---------X---------X---------X";
            levellayout[10] = "X---------X---------X---------X";
            levellayout[11] = "X---------XXXXXXXXXXX---------X";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X-----------------------------X";
            levellayout[14] = "X----R---------------------r--X";
            levellayout[15] = "X--------------H--------------X";
            levellayout[16] = "X-----------------------------X";


            //-------------------------------------------------------------------
            // End Section 5
            // Start Section 6
            // Notes:
            // Introduces: Fire Drones, Super Bouncers
            //-------------------------------------------------------------------



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-------------R---------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X--O--------XXXXXXX--------o--X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----X-----------------X-----X";
            levellayout[6] =  "XXXXXXX-----------------XXXXXXX";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X--O--------XXXXXXX--------o--X";
            levellayout[9] = "X-----------------------------X";
            levellayout[10] = "X-----X-----------------X-----X";
            levellayout[11] = "XXXXXXX-----------------XXXXXXX";
            levellayout[12] = "X-----------XXXXXXX-----------X";
            levellayout[13] = "X-------------------------1---X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X--------------R--------------X";
            levellayout[2] =  "X----XXXXXX---------XXXXXX----X";
            levellayout[3] =  "X----X--O-X---------X--O-X----X";
            levellayout[4] =  "X----X----X---------X----X----X";
            levellayout[5] =  "X----X----X---------X----X----X";
            levellayout[6] =  "X----XXXXXX---------XXXXXX----X";
            levellayout[7] =  "X-------1---------------------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X----XXXXXX---------XXXXXX----X";
            levellayout[11] = "X----X-o--X---------X--o-X----X";
            levellayout[12] = "X----X----X---------X----X----X";
            levellayout[13] = "X----X----X---------X----X----X";
            levellayout[14] = "X----XXXXXX---------XXXXXX----X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "X-----------------------------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-1---------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------XXXXX-------------X";
            levellayout[3] =  "XXXXX---------------------p---X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------XXXXX-X";
            levellayout[7] =  "X------XXXXX------------------X";
            levellayout[8] =  "X-------P----------P----------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X----------------XXXXX--------X";
            levellayout[11] = "X-----XXXXX-------------------X";
            levellayout[12] = "X---p-------------------------X";
            levellayout[13] = "X------------------------P----X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X--XXXXX----------------------X";
            levellayout[16] = "X----------------------XXXXX--X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X---------O---X------o--------X";
            levellayout[1] =  "X-------------X---------------X";
            levellayout[2] =  "X-------------X---------------X";
            levellayout[3] =  "X-----XXXXXXXXXXXXXXXXXXX-----X";
            levellayout[4] =  "X--O-----------------------o--X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "XXXXXX--O-------------o--XXXXXX";
            levellayout[8] =  "X----X-------------------X----X";
            levellayout[9] =  "X----X-------------------X----X";
            levellayout[10] = "X----XXXXX-----------XXXXX----X";
            levellayout[11] = "X--------X-----------X--------X";
            levellayout[12] = "X--------X-----------X--------X";
            levellayout[13] = "X---1-------------------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXX-------------------------X";






            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X----------L---L--------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[5] =  "X---o--o--o----X----O--O--O---X";
            levellayout[6] =  "X--------------X--------------X";
            levellayout[7] =  "X--------------X--------------X";
            levellayout[8] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[9] = "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----------XXXXXXXX----------X";
            levellayout[13] = "X----P------X--1---X------P---X";
            levellayout[14] = "X-----------X------X----------X";
            levellayout[15] = "X-----------X------X----------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            //-------------------------------------------------------------------
            // End Section 6
            // Start Section 7
            // Notes:
            // Introduces: Rocket Blasters
            //-------------------------------------------------------------------


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------XXXXXXX-----------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X----T------XXXXXXX--------t--X";
            levellayout[5] =  "X-----------X-----X-----------X";
            levellayout[6] =  "X-----------X-----X-----------X";
            levellayout[7] =  "X-----------X-----X-----------X";
            levellayout[8] =  "X-----------XXXXXXX-----------X";
            levellayout[9] =  "X------t----X-----X-----------X";
            levellayout[10] = "X-----------X-----X----T------X";
            levellayout[11] = "X-----------X-----X-----------X";
            levellayout[12] = "X-----------XXXXXXX-----------X";
            levellayout[13] = "X-----T-----X--1--X-----------X";
            levellayout[14] = "X-----------X-----X--------t--X";
            levellayout[15] = "X-----------X-----X-----------X";
            levellayout[16] = "X-----------XXXXXXX-----------X";




            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "XXXXX---t-----------------XXXXX";
            levellayout[1] =  "X-----------------------t-----X";
            levellayout[2] =  "X----------XXXXXXXXX----------X";
            levellayout[3] =  "X--------------T--------------X";
            levellayout[4] =  "XXXXX---------------------XXXXX";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X----------XXXXXXXXX----------X";
            levellayout[7] =  "X-------t---------------t-----X";
            levellayout[8] =  "XXXXX---------------------XXXXX";
            levellayout[9] = "X-----------------------------X";
            levellayout[10] = "X----------XXXXXXXXX----------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "XXXXX-----T---------------XXXXX";
            levellayout[13] = "X--------------------------1--X";
            levellayout[14] = "X----------XXXXXXXXX----------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXX---------------------XXXXX";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X----T------------------T-----X";
            levellayout[3] =  "X--------------t--------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----t-----------------t-----X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X-----------------------------X";
            levellayout[14] = "X-------------1---------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "X-----------------------------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------g--R--g-----------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-----X-----X-----X-----X-----X";
            levellayout[5] =  "X-----X-----X-----X-----X-----X";
            levellayout[6] =  "X-----X-----X-----X-----X-----X";
            levellayout[7] =  "X-----X-----X-----X-----X-----X";
            levellayout[8] =  "X--T--X--t--X--T--X--t--X--T--X";
            levellayout[9] =  "X-----X-----X-----X-----X-----X";
            levellayout[10] = "X-----X-----X-----X-----X-----X";
            levellayout[11] = "X-----X-----X-----X-----X-----X";
            levellayout[12] = "X-----X-----X-----X-----X-----X";
            levellayout[13] = "X--------------2--------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----X-----------------X-----X";
            levellayout[2] =  "X----X-------------------X----X";
            levellayout[3] =  "X---X---------------------X---X";
            levellayout[4] =  "X--X----------t----T-------X--X";
            levellayout[5] =  "X-X-------------------------X-X";
            levellayout[6] =  "XX----X----t------------X----XX";
            levellayout[7] =  "X----X-------------------X----X";
            levellayout[8] =  "X---X---------------------X---X";
            levellayout[9] =  "X--X-----------------------X--X";
            levellayout[10] = "X-X-------------------------X-X";
            levellayout[11] = "XX-----X---------------X-----XX";
            levellayout[12] = "X-1---X-----------t-----X-----X";
            levellayout[13] = "X----X-------------------X----X";
            levellayout[14] = "X---X-------T-------------X---X";
            levellayout[15] = "X--X-----------------------X--X";
            levellayout[16] = "XXX-------------------------XXX";


            //-------------------------------------------------------------------
            // End Section 7
            // Start Section 8
            // Notes:
            // Introduces: Warp Bots
            //-------------------------------------------------------------------



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--k-----------W------I--I----X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[5] =  "X-------I--I----------W-------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[9] = "X----W---------I---I----------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[13] = "X-------------1---------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-------1-------------W-------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-----X----X-------X----X-----X";
            levellayout[5] =  "X-----XXXXXX-------XXXXXX-----X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "X--------------W--------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X----X------X-----X------X----X";
            levellayout[11] = "XXXXXX------XXXXXXX------XXXXXX";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X-------W------------W--------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----X----X-------X----X-----X";
            levellayout[16] = "X-----XXXXXX-------XXXXXX-----X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X--------I-----------W--------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----X-----X-----X-----X-----X";
            levellayout[4] =  "X-----XXXXXXX-----XXXXXXX-----X";
            levellayout[5] =  "X-----X--W--X-----X---i-X-----X";
            levellayout[6] =  "X-----X-----X-----X-----X-----X";
            levellayout[7] =  "X-----X-----X-----X-----X-----X";
            levellayout[8] =  "X-----XXXXXXX-----XXXXXXX-----X";
            levellayout[9] =  "X-----X-I---X-----X--w--X-----X";
            levellayout[10] = "X-----X-----X-----X-----X-----X";
            levellayout[11] = "X-----X-----X-----X-----X-----X";
            levellayout[12] = "X-----XXXXXXX-----XXXXXXX-----X";
            levellayout[13] = "X--------------1--------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";



            //levellayout = levelLayouts[++levelIndex] = new string[18];
            //levellayout[0] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[1] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[2] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[3] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[5] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[6] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[7] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[8] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[9] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[10] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[11] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[12] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[13] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[14] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[15] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            //levellayout[17] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "XXXXXXXXXXXX---W--XXXXXXXXXXXXX";
            levellayout[1] =  "XXXXXXXXXXXX------XXXXXXXXXXXXX";
            levellayout[2] =  "XXXXXXXXXXXX------XXXXXXXXXXXXX";
            levellayout[3] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXX--W---XXX";
            levellayout[5] =  "XXXXXXX--W----XXXXXXXX------XXX";
            levellayout[6] =  "XXXXXXX-------XXXXXXXX------XXX";
            levellayout[7] =  "XXXXXXX-------XXXXXXXXXXXXXXXXX";
            levellayout[8] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[9] =  "XXXXXXXXXXXXXXXXXX--W---XXXXXXX";
            levellayout[10] = "XX--W---XXXXXXXXXX------XXXXXXX";
            levellayout[11] = "XX------XXXXXXXXXX------XXXXXXX";
            levellayout[12] = "XX------XXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[13] = "XXXXXXXXXXXX---1----XXXXXXXXXXX";
            levellayout[14] = "XXXXXXXXXXXX--------XXXXXXXXXXX";
            levellayout[15] = "XXXXXXXXXXXX--------XXXXXXXXXXX";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X------X----X-----X-----------X";
            levellayout[1] =  "X------X----X--W--X--------W--X";
            levellayout[2] =  "X------X----X-----X-----------X";
            levellayout[3] =  "X------X----X-----X-----------X";
            levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[5] =  "X------X----W-----X----X------X";
            levellayout[6] =  "X------X----------X----X------X";
            levellayout[7] =  "X------X----------X----X------X";
            levellayout[8] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[9] =  "X------X----X----------X---W--X";
            levellayout[10] = "X------X----X----------X------X";
            levellayout[11] = "X------X----X----------X------X";
            levellayout[12] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[13] = "X---W-------X--1--X-----------X";
            levellayout[14] = "X-----------X-----X-----------X";
            levellayout[15] = "X-----------X-----X-----------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            //-------------------------------------------------------------------
            // End Section 8
            // Start Section 9
            // Notes:
            // Introduces:
            //-------------------------------------------------------------------

            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----X------XXXXXXXXXXXX-----X";
            levellayout[4] =  "X-----X-----------------------X";
            levellayout[5] =  "X-----X------------R----------X";
            levellayout[6] =  "X-----X-----------------------X";
            levellayout[7] =  "X-----XXXXXXXXXX-------XXXXXXXX";
            levellayout[8] =  "X-----X-----------------------X";
            levellayout[9] =  "X-----X------r-----H------R---X";
            levellayout[10] = "X-----X-----------------------X";
            levellayout[11] = "X-----X------XXXXXXXXXXXXX----X";
            levellayout[12] = "X-----X-----------------------X";
            levellayout[13] = "X--1--X-------O---O-----------X";
            levellayout[14] = "X-----X-----------------------X";
            levellayout[15] = "X-----X-----------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--------------1--------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X------XXXXXXXXXXXXXXXXX------X";
            levellayout[4] =  "X------X---------------X------X";
            levellayout[5] =  "X------X---------------X------X";
            levellayout[6] =  "X------X----h------h---X------X";
            levellayout[7] =  "X------X---------------X------X";
            levellayout[8] =  "X------X---------------X------X";
            levellayout[9] =  "X------X-------m-------X------X";
            levellayout[10] = "X------X---------------X------X";
            levellayout[11] = "X------X----T------t---X------X";
            levellayout[12] = "X------X---------------X------X";
            levellayout[13] = "X---------------S-s-----------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";





            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--------------1--------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[4] =  "X-----------------l--l--l--l--X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[8] =  "X-------L---L---L---L---------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[12] = "X-----------------------------X";
            levellayout[13] = "X-----O---O---O---------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X--------------H--------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X----T-------XXXXXX------t----X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[7] =  "X-------------1---------------X";
            levellayout[8] =  "X-----------------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X-----R------------------r----X";
            levellayout[13] = "X-----------------------------X";
            levellayout[14] = "X------------XXXXXX-----------X";
            levellayout[15] = "X---------------H-------------X";
            levellayout[16] = "X-----------------------------X";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X---S---S---------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X----------------------c------X";
            levellayout[3] =  "XXXXXXXXXXXX------------------X";
            levellayout[4] =  "X-------------S---S-----------X";
            levellayout[5] =  "X-----------------------------X";
            levellayout[6] =  "X----------------------X------X";
            levellayout[7] =  "X--m---XXXXXXXXXXXXXXXXX------X";
            levellayout[8] =  "X----------S----S-------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "XXXXXXXXXXXXXXXXXXXXXXXX------X";
            levellayout[12] = "X---------1-------------------X";
            levellayout[13] = "X-----------------------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-------XXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[16] = "X-----------------------------X";

            //-------------------------------------------------------------------
            // End Section 9
            // Start Section 10
            // Notes: Possibly will have lights going out occasionally
            // Introduces: 
            //-------------------------------------------------------------------


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X----O---O----------H---------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "XXXXXXXXXXXXXXXXXXXXXXXX------X";
            levellayout[5] =  "X----W-----------------I---I--X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "XXXXXXXXXX------XXXXXXXXXXXXXXX";
            levellayout[9] =  "X-----------------S---S-------X";
            levellayout[10] = "X-----------------------------X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "XXXXX-------XXXXXXXXXXXXXXXXXXX";
            levellayout[13] = "X----------------1------------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X-----------------------------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXX------XXX";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----p-------XXXXX-----T-----X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X--XXXXXX---------------------X";
            levellayout[6] =  "X-----------------XXXXX-------X";
            levellayout[7] =  "X-------------------------P---X";
            levellayout[8] =  "X----------t------------------X";
            levellayout[9] =  "X-----------------------------X";
            levellayout[10] = "X------------------------XXXXXX";
            levellayout[11] = "X------XXXXXX-----------------X";
            levellayout[12] = "X--1--------------------------X";
            levellayout[13] = "X-------------------T---------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "XXXXX--------------XXXXXX-----X";
            levellayout[16] = "X-----------------------------X";

            
            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X-----------------------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X------------1----------------X";
            levellayout[6] =  "X-----------------------------X";
            levellayout[7] =  "X-----------------------------X";
            levellayout[8] =  "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[9] =  "X--O--O--X----O---O--X-O---O--X";
            levellayout[10] = "X--------X-----------X--------X";
            levellayout[11] = "X--------X-----------X--------X";
            levellayout[12] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[13] = "X-----O--X-----O-----X----O---X";
            levellayout[14] = "X--------X-----------X--------X";
            levellayout[15] = "X--------X-----------X--------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X--------T-----g---X-----H----X";
            levellayout[2] =  "X-----X-----------------W-----X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-------------X---------------X";
            levellayout[5] =  "X---------------I-------X-----X";
            levellayout[6] =  "X------X----------------------X";
            levellayout[7] =  "X-----------------------B-----X";
            levellayout[8] =  "X---------------X-------------X";
            levellayout[9] =  "X----------r------------------X";
            levellayout[10] = "X---X--------------t-----X----X";
            levellayout[11] = "X-----------------------------X";
            levellayout[12] = "X--1--------X----------I------X";
            levellayout[13] = "X-----------------X-----------X";
            levellayout[14] = "X-----------------------------X";
            levellayout[15] = "X--X----X--------------X------X";
            levellayout[16] = "X-----------------------------X";



            levellayout = levelLayouts[++levelIndex] = new string[17];
            levellayout[0] =  "X-----------------------------X";
            levellayout[1] =  "X-----------------------------X";
            levellayout[2] =  "X----------------c------------X";
            levellayout[3] =  "X-----------------------------X";
            levellayout[4] =  "X-----------------------------X";
            levellayout[5] =  "X-----XXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[6] =  "X-----X-----------------------X";
            levellayout[7] =  "X-----X-----------------------X";
            levellayout[8] =  "X-----X--t-------H--------T---X";
            levellayout[9] =  "X-----X-----------------------X";
            levellayout[10] = "X-----X-----------------------X";
            levellayout[11] = "X-----X-----------------------X";
            levellayout[12] = "X-----XXXXXXXXXXXXXXXXXXXXXXXXX";
            levellayout[13] = "X--1--X---O---O---X---O---O---X";
            levellayout[14] = "X-----X-----------X-----------X";
            levellayout[15] = "X-----X-----------X-----------X";
            levellayout[16] = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";


        }

        public Level GetLevel(int levelnum)
        {
            return new Level(levelLayouts[levelnum], levelnum, levelDetails[levelnum]);
        }

        public Level GetNextLevel()
        {
            int currentLevelNum = Level.singletonLevel.LevelNum;
            return new Level(levelLayouts[currentLevelNum+1], currentLevelNum+1, levelDetails[currentLevelNum+1]);
        }
    }
}