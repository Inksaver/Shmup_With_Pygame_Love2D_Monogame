local Player = {} 	-- simple table for Player. As there is only one, class not required
local Rectangle = require "lib.Rectangle"
local Circle = require "lib.Circle"
local Shared = require "shared"

function Player.setProperties(sprite, scale, speed)
	Player.sprite = sprite
	Player.scale = scale
	Player.speed = speed
	Player.rect = Rectangle(0, 0, sprite:getWidth() * scale, sprite:getHeight() * scale)
	Player.rect.w = sprite:getWidth() * scale
	Player.rect.h = sprite:getHeight() * scale
	Player.rect.x = Shared.WIDTH / 2 - Player.rect.w / 2
	Player.rect.y = Shared.HEIGHT - Player.rect.h - 10
	Player.radius = math.max(Player.rect.width, Player.rect.height) * 0.8 / 2
	Player.circle = Circle(Player.rect.x + Player.radius, Player.rect.y + Player.radius, Player.radius)
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
	Player.circle:update(Player.rect.x + Player.radius, Player.rect.y + Player.radius)
end

function Player.draw()
	love.graphics.setColor(Shared.WHITE)			-- change colour to white
	love.graphics.draw(Player.sprite, Player.rect.x, Player.rect.y, nil, Player.scale, Player.scale)

	love.graphics.setColor(Shared.RED)
	love.graphics.rectangle("line", Player.rect.x, Player.rect.y, Player.rect.w, Player.rect.h)
	love.graphics.setColor(Shared.BLUE)
	love.graphics.circle('line', Player.circle.x, Player.circle.y, Player.circle.radius)
	love.graphics.setColor(Shared.WHITE)
end

return Player