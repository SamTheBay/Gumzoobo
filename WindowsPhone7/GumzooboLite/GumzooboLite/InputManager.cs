#region File Description
//-----------------------------------------------------------------------------
// InputManager.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
#endregion

namespace BubbleGame
{
    /// <summary>
    /// This class handles all keyboard and gamepad actions in the game.
    /// </summary>
    public static class InputManager
    {
        #region Action Enumeration


        /// <summary>
        /// The actions that are possible within the game.
        /// </summary>
        public enum Action
        {
            MainMenu,
            Ok,
            Back,
            ExitGame,
            EnterGame,
            CursorUp,
            CursorDown,
            MoveCharacterUp,
            MoveCharacterDown,
            MoveCharacterLeft,
            MoveCharacterRight,
            Chew,
            Shoot,
            Jump,
            Special,
            WeaponSwitchLeft,
            WeaponSwitchRight,
            Delete,
            Pause,
            TotalActionCount,
        }


        /// <summary>
        /// Readable names of each action.
        /// </summary>
        private static readonly string[] actionNames = 
            {
                "Main Menu",
                "Ok",
                "Back",
                "Cursor Up",
                "Cursor Down",
                "Exit Game",
                "Enter game",
                "Move Character - Up",
                "Move Character - Down",
                "Move Character - Left",
                "Move Character - Right",
                "Chew",
                "Shoot",
                "Jump",
                "Special",
                "Weapon Switch Left",
                "Weapon Switch Right",
                "Delete",
                "Pause",
            };

        /// <summary>
        /// Returns the readable name of the given action.
        /// </summary>
        public static string GetActionName(Action action)
        {
            int index = (int)action;

            if ((index < 0) || (index > actionNames.Length))
            {
                throw new ArgumentException("action");
            }

            return actionNames[index];
        }


        #endregion


        #region Support Types


        /// <summary>
        /// GamePad controls expressed as one type, unified with button semantics.
        /// </summary>
        public enum GamePadButtons
        {
            Start,
            Back,
            A,
            B,
            X,
            Y,
            Up,
            Down,
            Left,
            Right,
            LeftShoulder,
            RightShoulder,
            LeftTrigger,
            RightTrigger,
        }


        /// <summary>
        /// A combination of gamepad and keyboard keys mapped to a particular action.
        /// </summary>
        public class ActionMap
        {
            /// <summary>
            /// List of GamePad controls to be mapped to a given action.
            /// </summary>
            public List<GamePadButtons> gamePadButtons = new List<GamePadButtons>();


            /// <summary>
            /// List of Keyboard controls to be mapped to a given action.
            /// </summary>
            public List<Keys> keyboardKeys = new List<Keys>();
        }


        #endregion


        #region Constants


        /// <summary>
        /// The value of an analog control that reads as a "pressed button".
        /// </summary>
        const float analogLimit = 0.5f;


        #endregion


        #region Keyboard Data


        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        private static KeyboardState currentKeyboardState;

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
        }


        /// <summary>
        /// The state of the keyboard as of the previous update.
        /// </summary>
        private static KeyboardState previousKeyboardState;


        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }


        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsKeyTriggered(Keys key)
        {
            return (currentKeyboardState.IsKeyDown(key)) &&
                (!previousKeyboardState.IsKeyDown(key));
        }


        #endregion


        #region GamePad Data


        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        private static GamePadState[] currentGamePadState = new GamePadState[4];

        /// <summary>
        /// The state of the gamepad as of the last update.
        /// </summary>
        public static GamePadState CurrentGamePadState(int controllerIndex)
        {
            return currentGamePadState[controllerIndex];
        }


        /// <summary>
        /// The state of the gamepad as of the previous update.
        /// </summary>
        private static GamePadState[] previousGamePadState = new GamePadState[4];


        #region GamePadButton Pressed Queries


        /// <summary>
        /// Check if the gamepad's Start button is pressed.
        /// </summary>
        public static bool IsGamePadStartPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.Start == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Back button is pressed.
        /// </summary>
        public static bool IsGamePadBackPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.Back == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's A button is pressed.
        /// </summary>
        public static bool IsGamePadAPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.A == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's B button is pressed.
        /// </summary>
        public static bool IsGamePadBPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.B == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's X button is pressed.
        /// </summary>
        public static bool IsGamePadXPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.X == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's Y button is pressed.
        /// </summary>
        public static bool IsGamePadYPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.Y == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.LeftShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// <summary>
        /// Check if the gamepad's RightShoulder button is pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Buttons.RightShoulder == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadUpPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].DPad.Up == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadDownPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].DPad.Down == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].DPad.Left == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad is pressed.
        /// </summary>
        public static bool IsGamePadDPadRightPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].DPad.Right == ButtonState.Pressed);
        }


        /// <summary>
        /// Check if the gamepad's left trigger is pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Triggers.Left > analogLimit);
        }


        /// <summary>
        /// Check if the gamepad's right trigger is pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].Triggers.Right > analogLimit);
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownPressed(int controllerIndex)
        {
            return (-1f * currentGamePadState[controllerIndex].ThumbSticks.Left.Y > analogLimit);
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftPressed(int controllerIndex)
        {
            return (-1f * currentGamePadState[controllerIndex].ThumbSticks.Left.X > analogLimit);
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick is pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightPressed(int controllerIndex)
        {
            return (currentGamePadState[controllerIndex].ThumbSticks.Left.X > analogLimit);
        }


        /// <summary>
        /// Check if the GamePadKey value specified is pressed.
        /// </summary>
        private static bool IsGamePadButtonPressed(GamePadButtons gamePadKey, int controllerIndex)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartPressed(controllerIndex);

                case GamePadButtons.Back:
                    return IsGamePadBackPressed(controllerIndex);

                case GamePadButtons.A:
                    return IsGamePadAPressed(controllerIndex);

                case GamePadButtons.B:
                    return IsGamePadBPressed(controllerIndex);

                case GamePadButtons.X:
                    return IsGamePadXPressed(controllerIndex);

                case GamePadButtons.Y:
                    return IsGamePadYPressed(controllerIndex);

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderPressed(controllerIndex);

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderPressed(controllerIndex);

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerPressed(controllerIndex);

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerPressed(controllerIndex);

                case GamePadButtons.Up:
                    return IsGamePadDPadUpPressed(controllerIndex) ||
                        IsGamePadLeftStickUpPressed(controllerIndex);

                case GamePadButtons.Down:
                    return IsGamePadDPadDownPressed(controllerIndex) ||
                        IsGamePadLeftStickDownPressed(controllerIndex);

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftPressed(controllerIndex) ||
                        IsGamePadLeftStickLeftPressed(controllerIndex);

                case GamePadButtons.Right:
                    return IsGamePadDPadRightPressed(controllerIndex) ||
                        IsGamePadLeftStickRightPressed(controllerIndex);
            }

            return false;
        }


        #endregion


        #region GamePadButton Triggered Queries


        /// <summary>
        /// Check if the gamepad's Start button was just pressed.
        /// </summary>
        public static bool IsGamePadStartTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Buttons.Start == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].Buttons.Start == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Back button was just pressed.
        /// </summary>
        public static bool IsGamePadBackTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Buttons.Back == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].Buttons.Back == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's A button was just pressed.
        /// </summary>
        public static bool IsGamePadATriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Buttons.A == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].Buttons.A == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's B button was just pressed.
        /// </summary>
        public static bool IsGamePadBTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Buttons.B == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].Buttons.B == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's X button was just pressed.
        /// </summary>
        public static bool IsGamePadXTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Buttons.X == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].Buttons.X == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's Y button was just pressed.
        /// </summary>
        public static bool IsGamePadYTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Buttons.Y == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].Buttons.Y == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's LeftShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadLeftShoulderTriggered(int controllerIndex)
        {
            return (
                (currentGamePadState[controllerIndex].Buttons.LeftShoulder == ButtonState.Pressed) &&
                (previousGamePadState[controllerIndex].Buttons.LeftShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's RightShoulder button was just pressed.
        /// </summary>
        public static bool IsGamePadRightShoulderTriggered(int controllerIndex)
        {
            return (
                (currentGamePadState[controllerIndex].Buttons.RightShoulder == ButtonState.Pressed) &&
                (previousGamePadState[controllerIndex].Buttons.RightShoulder == ButtonState.Released));
        }


        /// <summary>
        /// Check if Up on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadUpTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].DPad.Up == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].DPad.Up == ButtonState.Released));
        }


        /// <summary>
        /// Check if Down on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadDownTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].DPad.Down == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].DPad.Down == ButtonState.Released));
        }


        /// <summary>
        /// Check if Left on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadLeftTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].DPad.Left == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].DPad.Left == ButtonState.Released));
        }


        /// <summary>
        /// Check if Right on the gamepad's directional pad was just pressed.
        /// </summary>
        public static bool IsGamePadDPadRightTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].DPad.Right == ButtonState.Pressed) &&
              (previousGamePadState[controllerIndex].DPad.Right == ButtonState.Released));
        }


        /// <summary>
        /// Check if the gamepad's left trigger was just pressed.
        /// </summary>
        public static bool IsGamePadLeftTriggerTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Triggers.Left > analogLimit) &&
                (previousGamePadState[controllerIndex].Triggers.Left < analogLimit));
        }


        /// <summary>
        /// Check if the gamepad's right trigger was just pressed.
        /// </summary>
        public static bool IsGamePadRightTriggerTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].Triggers.Right > analogLimit) &&
                (previousGamePadState[controllerIndex].Triggers.Right < analogLimit));
        }


        /// <summary>
        /// Check if Up on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickUpTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].ThumbSticks.Left.Y > analogLimit) &&
                (previousGamePadState[controllerIndex].ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Down on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickDownTriggered(int controllerIndex)
        {
            return ((-1f * currentGamePadState[controllerIndex].ThumbSticks.Left.Y > analogLimit) &&
                (-1f * previousGamePadState[controllerIndex].ThumbSticks.Left.Y < analogLimit));
        }


        /// <summary>
        /// Check if Left on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickLeftTriggered(int controllerIndex)
        {
            return ((-1f * currentGamePadState[controllerIndex].ThumbSticks.Left.X > analogLimit) &&
                (-1f * previousGamePadState[controllerIndex].ThumbSticks.Left.X < analogLimit));
        }


        /// <summary>
        /// Check if Right on the gamepad's left analog stick was just pressed.
        /// </summary>
        public static bool IsGamePadLeftStickRightTriggered(int controllerIndex)
        {
            return ((currentGamePadState[controllerIndex].ThumbSticks.Left.X > analogLimit) &&
                (previousGamePadState[controllerIndex].ThumbSticks.Left.X < analogLimit));
        }


        /// <summary>
        /// Check if the GamePadKey value specified was pressed this frame.
        /// </summary>
        private static bool IsGamePadButtonTriggered(GamePadButtons gamePadKey, int controllerIndex)
        {
            switch (gamePadKey)
            {
                case GamePadButtons.Start:
                    return IsGamePadStartTriggered(controllerIndex);

                case GamePadButtons.Back:
                    return IsGamePadBackTriggered(controllerIndex);

                case GamePadButtons.A:
                    return IsGamePadATriggered(controllerIndex);

                case GamePadButtons.B:
                    return IsGamePadBTriggered(controllerIndex);

                case GamePadButtons.X:
                    return IsGamePadXTriggered(controllerIndex);

                case GamePadButtons.Y:
                    return IsGamePadYTriggered(controllerIndex);

                case GamePadButtons.LeftShoulder:
                    return IsGamePadLeftShoulderTriggered(controllerIndex);

                case GamePadButtons.RightShoulder:
                    return IsGamePadRightShoulderTriggered(controllerIndex);

                case GamePadButtons.LeftTrigger:
                    return IsGamePadLeftTriggerTriggered(controllerIndex);

                case GamePadButtons.RightTrigger:
                    return IsGamePadRightTriggerTriggered(controllerIndex);

                case GamePadButtons.Up:
                    return IsGamePadDPadUpTriggered(controllerIndex) ||
                        IsGamePadLeftStickUpTriggered(controllerIndex);

                case GamePadButtons.Down:
                    return IsGamePadDPadDownTriggered(controllerIndex) ||
                        IsGamePadLeftStickDownTriggered(controllerIndex);

                case GamePadButtons.Left:
                    return IsGamePadDPadLeftTriggered(controllerIndex) ||
                        IsGamePadLeftStickLeftTriggered(controllerIndex);

                case GamePadButtons.Right:
                    return IsGamePadDPadRightTriggered(controllerIndex) ||
                        IsGamePadLeftStickRightTriggered(controllerIndex);
            }

            return false;
        }


        #endregion


        #endregion

        public static TouchCollection TouchState;

        public static readonly List<GestureSample> Gestures = new List<GestureSample>();

        private static ButtonState PreviousBackState = ButtonState.Released;
        private static bool BackTriggered = false;

        #region Action Mapping


        /// <summary>
        /// The action mappings for the game.
        /// </summary>
        private static ActionMap[] actionMaps;


        public static ActionMap[] ActionMaps
        {
            get { return actionMaps; }
        }


        /// <summary>
        /// Reset the action maps to their default values.
        /// </summary>
        private static void ResetActionMaps()
        {
            actionMaps = new ActionMap[(int)Action.TotalActionCount];

            actionMaps[(int)Action.MainMenu] = new ActionMap();
            actionMaps[(int)Action.MainMenu].keyboardKeys.Add(
                Keys.Tab);
            actionMaps[(int)Action.MainMenu].gamePadButtons.Add(
                GamePadButtons.Start);

            actionMaps[(int)Action.Ok] = new ActionMap();
            actionMaps[(int)Action.Ok].keyboardKeys.Add(
                Keys.Enter);
            actionMaps[(int)Action.Ok].gamePadButtons.Add(
                GamePadButtons.A);

            actionMaps[(int)Action.Back] = new ActionMap();
            actionMaps[(int)Action.Back].keyboardKeys.Add(
                Keys.Escape);
            //actionMaps[(int)Action.Back].gamePadButtons.Add(
            //    GamePadButtons.B);
            actionMaps[(int)Action.Back].gamePadButtons.Add(
                GamePadButtons.Back);

            actionMaps[(int)Action.CursorUp] = new ActionMap();
            actionMaps[(int)Action.CursorUp].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.CursorUp].gamePadButtons.Add(
                GamePadButtons.Up);
            actionMaps[(int)Action.CursorUp].gamePadButtons.Add(
                GamePadButtons.Left);

            actionMaps[(int)Action.CursorDown] = new ActionMap();
            actionMaps[(int)Action.CursorDown].keyboardKeys.Add(
                Keys.Down);
            actionMaps[(int)Action.CursorDown].gamePadButtons.Add(
                GamePadButtons.Down);
            actionMaps[(int)Action.CursorDown].gamePadButtons.Add(
                GamePadButtons.Right);

            actionMaps[(int)Action.ExitGame] = new ActionMap();
            actionMaps[(int)Action.ExitGame].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.ExitGame].gamePadButtons.Add(
                GamePadButtons.Back);

            actionMaps[(int)Action.EnterGame] = new ActionMap();
            actionMaps[(int)Action.EnterGame].keyboardKeys.Add(
                Keys.Enter);
            actionMaps[(int)Action.EnterGame].gamePadButtons.Add(
                GamePadButtons.Start);

            actionMaps[(int)Action.MoveCharacterUp] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterUp].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.MoveCharacterUp].gamePadButtons.Add(
                GamePadButtons.Up);

            actionMaps[(int)Action.MoveCharacterDown] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterDown].keyboardKeys.Add(
                Keys.Down);
            actionMaps[(int)Action.MoveCharacterDown].gamePadButtons.Add(
                GamePadButtons.Down);

            actionMaps[(int)Action.MoveCharacterLeft] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterLeft].keyboardKeys.Add(
                Keys.Left);
            actionMaps[(int)Action.MoveCharacterLeft].gamePadButtons.Add(
                GamePadButtons.Left);

            actionMaps[(int)Action.MoveCharacterRight] = new ActionMap();
            actionMaps[(int)Action.MoveCharacterRight].keyboardKeys.Add(
                Keys.Right);
            actionMaps[(int)Action.MoveCharacterRight].gamePadButtons.Add(
                GamePadButtons.Right);


            actionMaps[(int)Action.Shoot] = new ActionMap();
            actionMaps[(int)Action.Shoot].keyboardKeys.Add(
                Keys.Space);
            actionMaps[(int)Action.Shoot].gamePadButtons.Add(
                GamePadButtons.RightTrigger);

            actionMaps[(int)Action.Jump] = new ActionMap();
            actionMaps[(int)Action.Jump].keyboardKeys.Add(
                Keys.Up);
            actionMaps[(int)Action.Jump].gamePadButtons.Add(
                GamePadButtons.A);

            actionMaps[(int)Action.Special] = new ActionMap();
            actionMaps[(int)Action.Special].keyboardKeys.Add(
                Keys.C);
            actionMaps[(int)Action.Special].gamePadButtons.Add(
                GamePadButtons.X);

            actionMaps[(int)Action.WeaponSwitchLeft] = new ActionMap();
            actionMaps[(int)Action.WeaponSwitchLeft].keyboardKeys.Add(
                Keys.Z);
            actionMaps[(int)Action.WeaponSwitchLeft].gamePadButtons.Add(
                GamePadButtons.LeftShoulder);

            actionMaps[(int)Action.WeaponSwitchRight] = new ActionMap();
            actionMaps[(int)Action.WeaponSwitchRight].keyboardKeys.Add(
                Keys.X);
            actionMaps[(int)Action.WeaponSwitchRight].gamePadButtons.Add(
                GamePadButtons.RightShoulder);

            actionMaps[(int)Action.Delete] = new ActionMap();
            actionMaps[(int)Action.Delete].keyboardKeys.Add(
                Keys.D);
            actionMaps[(int)Action.Delete].gamePadButtons.Add(
                GamePadButtons.Y);

            actionMaps[(int)Action.Pause] = new ActionMap();
            actionMaps[(int)Action.Pause].keyboardKeys.Add(
                Keys.Escape);
            actionMaps[(int)Action.Pause].gamePadButtons.Add(
                GamePadButtons.Start);

        }


        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        public static bool IsActionPressed(Action action, int controllerIndex)
        {
            return IsActionMapPressed(actionMaps[(int)action], controllerIndex);
        }


        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        public static bool IsActionTriggered(Action action, int controllerIndex)
        {
            return IsActionMapTriggered(actionMaps[(int)action], controllerIndex);
        }


        /// <summary>
        /// Check if an action map has been pressed.
        /// </summary>
        private static bool IsActionMapPressed(ActionMap actionMap, int controllerIndex)
        {
            if (controllerIndex == 0)
            {
                for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
                {
                    if (IsKeyPressed(actionMap.keyboardKeys[i]))
                    {
                        return true;
                    }
                }
            }
            if (currentGamePadState[controllerIndex].IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonPressed(actionMap.gamePadButtons[i], controllerIndex))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Check if an action map has been triggered this frame.
        /// </summary>
        private static bool IsActionMapTriggered(ActionMap actionMap, int controllerIndex)
        {
            if (controllerIndex == 0)
            {
                for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
                {
                    if (IsKeyTriggered(actionMap.keyboardKeys[i]))
                    {
                        return true;
                    }
                }
            }
            if (currentGamePadState[controllerIndex].IsConnected)
            {
                for (int i = 0; i < actionMap.gamePadButtons.Count; i++)
                {
                    if (IsGamePadButtonTriggered(actionMap.gamePadButtons[i], controllerIndex))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        #endregion


        #region Initialization


        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        public static void Initialize()
        {
            ResetActionMaps();
        }


        #endregion


        #region Updating


        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        public static void Update()
        {
            // update the keyboard state
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // update the gamepad state
            previousGamePadState[0] = currentGamePadState[0];
            currentGamePadState[0] = GamePad.GetState(PlayerIndex.One);
            previousGamePadState[1] = currentGamePadState[1];
            currentGamePadState[1] = GamePad.GetState(PlayerIndex.Two);
            previousGamePadState[2] = currentGamePadState[2];
            currentGamePadState[2] = GamePad.GetState(PlayerIndex.Three);
            previousGamePadState[3] = currentGamePadState[3];
            currentGamePadState[3] = GamePad.GetState(PlayerIndex.Four);


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed && PreviousBackState == ButtonState.Released)
            {
                BackTriggered = true;
            }
            else
            {
                BackTriggered = false;
            }
            PreviousBackState = GamePad.GetState(PlayerIndex.One).Buttons.Back;

            TouchState = TouchPanel.GetState();

            Gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                Gestures.Add(TouchPanel.ReadGesture());
            }

        }


        #endregion


        public static bool IsLocationPressed(Rectangle loc)
        {
            for (int i = 0; i < TouchState.Count; i++)
            {
                if ((TouchState[i].State == TouchLocationState.Pressed || TouchState[i].State == TouchLocationState.Moved) && loc.Contains((int)TouchState[i].Position.X,(int)TouchState[i].Position.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsLocationTapped(Rectangle loc)
        {
            for (int i = 0; i < Gestures.Count; i++)
            {
                // Note: we translate the coordinate plane for the rotation in line here...
                if (Gestures[i].GestureType == GestureType.Tap && loc.Contains((int)Gestures[i].Position.X, (int)Gestures[i].Position.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsLocationHeld(Rectangle loc)
        {
            for (int i = 0; i < Gestures.Count; i++)
            {
                // Note: we translate the coordinate plane for the rotation in line here...
                if (Gestures[i].GestureType == GestureType.Hold && loc.Contains((int)Gestures[i].Position.X, (int)Gestures[i].Position.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public static Direction IsSwipe()
        {
            for (int i = 0; i < Gestures.Count; i++)
            {
                // Note: we translate the coordinate plane for the rotation in line here...
                if (Gestures[i].GestureType == GestureType.Flick)
                {
                    if (Gestures[i].Delta.X > Gestures[i].Delta2.X)
                    {
                        return Direction.Left;
                    }
                    if (Gestures[i].Delta.X < Gestures[i].Delta2.X)
                    {
                        return Direction.Right;
                    }
                }
            }
            return Direction.Up;
        }

        public static bool IsBackTriggered()
        {
            return BackTriggered;
        }

    }
}
