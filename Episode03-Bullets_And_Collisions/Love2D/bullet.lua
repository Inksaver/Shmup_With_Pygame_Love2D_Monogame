local Class = require("lib.Class")
local Bullet = Class:derive("Bullet")
local Rectangle = require "lib.Rectangle"
local Shared = require "shared"

function Bullet:new(Player, width, height, position)
	--[[ class constructor, takes Player object, and rectangle width, height ]]
	self.speedY = 1000
	if Shared.debug then
		self.speedY = 400
	end
	local posX = Player.rect.x + Player.rect.w / 2 - width / 2
	if position == 'left' then
		posX = Player.rect.left
	elseif position == 'right' then
		posX = Player.rect.right - width
	end
	self.active = true
	-- rectangle collider
	self.rect = Rectangle(posX,
						  Player.rect.top - height + 10,
						  width,
						  height)
end

function Bullet:getRect()
	return self.rect
end

function Bullet:update(dt)
	self.rect:updatePosition(0, self.speedY * dt * -1) -- negative as bullets go up
	if self.rect.top <= 0 then
		self.active = false 
	end
	return self.active -- when false, bullet can be set to nil
end

function Bullet:draw()
	love.graphics.rectangle("fill", self.rect.x, self.rect.y, self.rect.w, self.rect.h)
end

return Bullet