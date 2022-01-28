local Player = {} 	-- simple table for Player. As there is only one, class not required
local Shared = require "shared"
local Rectangle = require "lib.Rectangle"

function Player.setProperties(width, height, speed)
	--[[ static class equivalent of a constructor ]]
	Player.speed = speed
	Player.rect = Rectangle(Shared.WIDTH / 2 - width / 2, Shared.HEIGHT - height - 10, width, height)
end

function Player.update(keyboard, dt)
	if keyboard.isDown("left") or keyboard.isDown("a") then
		Player.rect:updatePosition(Player.speed * dt * -1, 0) 	-- move Player left
	end
	if keyboard.isDown("right") or keyboard.isDown("d")then	
		Player.rect:updatePosition(Player.speed * dt, 0) 		-- move Player right
	end
	if Player.rect.x < 0 then							-- check if Player x position is off-screen left side
		Player.rect:setX(0)								-- whoops! change it to 0: left side of screen
	end
	if Player.rect.right > Shared.WIDTH then					-- check if Player x position is off-screen right side (take Player width into account)
		Player.rect:setX(Shared.WIDTH - Player.rect.width)		-- whoops! change it to right side of screen, less the Player width		
	end
end

function Player.draw()
	love.graphics.setColor(Shared.GREEN)			-- change colour to green
	love.graphics.rectangle("fill", Player.rect.x, Player.rect.y, Player.rect.w, Player.rect.h)
end

return Player    