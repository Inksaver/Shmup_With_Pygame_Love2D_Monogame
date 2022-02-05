--[[ lua equivalent of C# static class and Python code module ]]
local S = {}
S.WIDTH 		= love.graphics.getWidth()
S.HEIGHT 		= love.graphics.getHeight()
-- colour constants: 0-1 each value
S.BLACK 		= {0, 0, 0}
S.WHITE 		= {1, 1, 1}
S.RED 			= {1, 0, 0}
S.GREEN 		= {0, 1, 0}
S.BLUE 			= {0 ,0, 1}
S.CYAN 			= {0, 1, 1}
S.MAGENTA 		= {1, 0, 1}
S.YELLOW 		= {1 ,1, 0}
S.DARKBLUE		= {0, 0, 12/255}
-- variables
S.debug			= false
S.gamestates	= {menu = 1, play = 2, leaderboard = 3, quit = 4}
S.gamestate		= 1
S.score 		= 0
S.fontName 		= "font/ARIALN.ttf"
S.fonts = 
{
	size14 = love.graphics.newFont(S.fontName, 14),
	size16 = love.graphics.newFont(S.fontName, 16),
	size18 = love.graphics.newFont(S.fontName, 18),
	size20 = love.graphics.newFont(S.fontName, 20), 
	size22 = love.graphics.newFont(S.fontName, 22), 
	size24 = love.graphics.newFont(S.fontName, 24),
	size30 = love.graphics.newFont(S.fontName, 30),
	size50 = love.graphics.newFont(S.fontName, 50), 
	size64 = love.graphics.newFont(S.fontName, 64)
}

function S.drawText(text, size, x, y, align, colour) -- drawText("Hello", "size24", 10, 10, "centre", WHITE)
	love.graphics.setFont(S.fonts[size])
	colour = colour or S.WHITE
	love.graphics.setColor(colour)
	if align ~= nil then
		if align == 'centre' then align = 'center' end
		love.graphics.printf( text, x, y, S.WIDTH, align )
	else
		love.graphics.print(text, x, y)
	end
end

return S