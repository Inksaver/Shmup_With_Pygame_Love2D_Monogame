--[[ lua equivalent of C# static class and Python code module ]]
local S = {}
S.WIDTH 		  = love.graphics.getWidth()
S.HEIGHT 		  = love.graphics.getHeight()
-- colour constants: 0-1 each value
S.BLACK 		  = {0, 0, 0}
S.WHITE 		  = {1, 1, 1}
S.RED 			  = {1, 0, 0}
S.GREEN 		  = {0, 1, 0}
S.BLUE 			  = {0 ,0, 1}
S.CYAN 			  = {0, 1, 1}
S.MAGENTA 		 = {1, 0, 1}
S.YELLOW 		  = {1 ,1, 0}
S.DARKBLUE		 = {0, 0, 12/255}
-- variables
S.debug			  = false
S.gamestates	  = {menu = 1, play = 2, leaderboard = 3, quit = 4}
S.gamestate		= 1

return S
