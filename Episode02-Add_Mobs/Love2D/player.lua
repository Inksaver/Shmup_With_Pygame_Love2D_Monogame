local Shared = require "shared"
local Player = {} 	-- simple table for Player. As there is only one, class not required

Player.x = 0		-- Player starting position x
Player.y = 0		-- Player starting position y
Player.w = 0		-- width
Player.h = 0		-- height
Player.speed = 0	-- how fast the Player can move

Player.rect = {}	-- required for collision detection
Player.rect.x = 0
Player.rect.y = 0 
Player.rect.w = 0
Player.rect.h = 0

function Player.setProperties(width, height, speed)
	--[[ static class equivalent of a constructor ]]
	Player.w = width
	Player.h = height
	Player.speed = speed
	Player.x = Shared.WIDTH / 2 - Player.w / 2 -- Player starting position x based on window width and Player width
	Player.y = Shared.HEIGHT - Player.h - 10	-- Player starting position y based on window height and Player height
	Player.rect.x = Player.x 
	Player.rect.y = Player.y 
	Player.rect.w = Player.w
	Player.rect.h = Player.h
end

function Player.update(dt, keyboard)
	if keyboard.isDown("left") or keyboard.isDown("a") then	
		Player.x = Player.x - Player.speed * dt	-- move Player left
	end
	if keyboard.isDown("right") or keyboard.isDown("d")then	
		Player.x = Player.x + Player.speed * dt	-- move Player right
	end
	if Player.x < 0 then					-- check if Player x position is off-screen left side
		Player.x = 0						-- whoops! change it to 0: left side of screen
	end
	if Player.x > Shared.WIDTH - Player.w then		-- check if Player x position is off-screen right side (take Player width into account)
		Player.x = Shared.WIDTH - Player.w			-- whoops! change it to right side of screen, less the Player width
	end
end

function Player.draw()
	love.graphics.setColor(Shared.GREEN)			-- change colour to green
	love.graphics.rectangle("fill", Player.x, Player.y, Player.w, Player.h)
end

return Player    