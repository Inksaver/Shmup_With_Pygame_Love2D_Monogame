local Class = require("lib.Class")
local Bullet = Class:derive("Bullet")
local Rectangle = require "lib.Rectangle"
local Shared = require "shared"

function Bullet:new(Player, sprite)
	--[[ class constructor, takes x and y integer values and a sprite]]
	self.sprite = sprite
	self.rotation = 0 -- radians 360 deg = math.pi * 2 (approx 6.3 rad). 
	self.speedY = 1000
	if Shared.debug then
		self.speedY = 400
	end
	local posX = Player.rect.x + Player.rect.w / 2 - sprite:getWidth() / 2
	-- rectangle collider
	self.rect = Rectangle(posX,
						  Player.rect.y - sprite:getHeight() + 10,
						  sprite:getWidth(),
						  sprite:getHeight())
	self.active = true
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
	love.graphics.draw(self.sprite, self.rect.x, self.rect.y)
	if Shared.debug then
		love.graphics.setColor(Shared.RED)
		love.graphics.rectangle("line", self.rect.x, self.rect.y, self.rect.w, self.rect.h)
		love.graphics.setColor(Shared.WHITE)
	end
end

return Bullet