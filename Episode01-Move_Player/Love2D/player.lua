local Player = {} 	-- simple table for Player. As there is only one, class not required
local Shared = require "shared"

function Player.setProperties(width, height, speed)
	--[[ static class equivalent of a constructor ]]
	Player.speed = speed
	Player.rect = {}	-- required for collision detection
	Player.rect.x = Shared.WIDTH / 2 - width / 2 	-- Player starting position x based on window width and Player width
	Player.rect.y = Shared.HEIGHT - height - 10	-- Player starting position y based on window height and Player height 
	Player.rect.w = width
	Player.rect.h = height
end

function Player.update(keyboard, dt)
	if keyboard.isDown("left") or keyboard.isDown("a") then	
		Player.rect.x = Player.rect.x - Player.speed * dt	-- move Player left
	end
	if keyboard.isDown("right") or keyboard.isDown("d")then	
		Player.rect.x = Player.rect.x + Player.speed * dt	-- move Player right
	end
	if Player.rect.x < 0 then					-- check if Player x position is off-screen left side
		Player.rect.x = 0						-- whoops! change it to 0: left side of screen
	end
	if Player.rect.x > Shared.WIDTH - Player.rect.w then		-- check if Player x position is off-screen right side (take Player width into account)
		Player.rect.x = Shared.WIDTH - Player.rect.w			-- whoops! change it to right side of screen, less the Player width
	end
end

function Player.draw()
	love.graphics.setColor(Shared.GREEN)			-- change colour to green
	love.graphics.rectangle("fill", Player.rect.x, Player.rect.y, Player.rect.w, Player.rect.h)
end

return Player
