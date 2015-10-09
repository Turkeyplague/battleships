using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

using static GameController;
using static UtilityFunctions;
using static GameResources;
using static DeploymentController;
using static EndingGameController;
using static MenuController;
using static HighScoreController;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
static class DiscoveryController
{

    /// <summary>
    /// Handles input during the discovery phase of the game.
    /// </summary>
    /// <remarks>
    /// Escape opens the game menu. Clicking the mouse will
    /// attack a location.
    /// </remarks>
    public static void HandleDiscoveryInput()
    {
        if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE)) {
            AddNewState(GameState.ViewingGameMenu);
        }

        if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
            DoAttack();
        }

		const int BX = 693;
		const int BY = 72;
		const int BWIDTH = 78;
		const int BHEIGHT = 44;
		Rectangle R = SwinGame.RectangleFrom(BX, BY, BWIDTH, BHEIGHT);

		if (SwinGame.MouseClicked(MouseButton.LeftButton))
		{
			Point2D mPoint = SwinGame.MousePosition();
			if (SwinGame.PointInRect(mPoint, R))
			{
				GameController.EndCurrentState();
			}
		}
    }

    /// <summary>
    /// Attack the location that the mouse if over.
    /// </summary>
    private static void DoAttack()
    {
        Point2D mouse = default(Point2D);

        mouse = SwinGame.MousePosition();

        //Calculate the row/col clicked
        int row = 0;
        int col = 0;
        row = Convert.ToInt32(Math.Floor((mouse.Y - FIELD_TOP) / (CELL_HEIGHT + CELL_GAP)));
        col = Convert.ToInt32(Math.Floor((mouse.X - FIELD_LEFT) / (CELL_WIDTH + CELL_GAP)));

        if (row >= 0 & row < HumanPlayer.EnemyGrid.Height) {
            if (col >= 0 & col < HumanPlayer.EnemyGrid.Width) {
                Attack(row, col);
            }
        }
    }

    /// <summary>
    /// Draws the game during the attack phase.
    /// </summary>s
    public static void DrawDiscovery()
    {
        const int SCORES_LEFT = 172;
        const int SHOTS_TOP = 157;
        const int HITS_TOP = 206;
        const int SPLASH_TOP = 256;
		const int MENU_BUTTON_TOP = 72;
		const int MENU_BUTTON_LEFT = 693;

        if ((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c)) {
            DrawField(HumanPlayer.EnemyGrid, ComputerPlayer, true);
        } else {
            DrawField(HumanPlayer.EnemyGrid, ComputerPlayer, false);
        }

		SwinGame.DrawBitmap(GameImage("MenuButton"), MENU_BUTTON_LEFT, MENU_BUTTON_TOP);


        DrawSmallField(HumanPlayer.PlayerGrid, HumanPlayer);
        DrawMessage();

        SwinGame.DrawText(HumanPlayer.Shots.ToString(), Color.White, GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
        SwinGame.DrawText(HumanPlayer.Hits.ToString(), Color.White, GameFont("Menu"), SCORES_LEFT, HITS_TOP);
        SwinGame.DrawText(HumanPlayer.Missed.ToString(), Color.White, GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
    }

}