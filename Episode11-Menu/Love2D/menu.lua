--[[ Static class to display menu at start of game, or after lives lost ]]
local Shared = require "shared"
local Menu = {}

function Menu.update(keyboard)
	--[[
	love.keypressed(key) in main.lua takes priority,
	so not required in Menu.lua
	]]
end

function Menu.draw(gameTitle)
	--[[ show start menu screen ]]
	-- drawText(text, size, x, y, align, colour) -- drawText("Hello", "size24", 10, 10, "centre", WHITE)
	Shared.drawText(gameTitle, "size64", 0, Shared.HEIGHT * 0.25, 'centre', Shared.YELLOW)
	Shared.drawText("Arrow keys move, space to fire", "size22", 0, Shared.HEIGHT * 0.5, 'centre', Shared.WHITE)
	Shared.drawText("Press Enter to begin", "size30", 0, Shared.HEIGHT * 0.65, 'centre', Shared.WHITE)
	Shared.drawText("Escape to quit at any time", "size18", 0, Shared.HEIGHT * 0.85, 'centre', Shared.WHITE)
	Shared.drawText("Press B to toggle background music", "size16", 0, Shared.HEIGHT * 0.9, 'centre', Shared.GREEN)
end

return Menu