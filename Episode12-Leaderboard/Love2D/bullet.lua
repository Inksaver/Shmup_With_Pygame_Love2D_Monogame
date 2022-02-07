local Class = require("lib.Class")
local Bullet = Class:derive("Bullet")
local Rectangle = require "lib.Rectangle"
local Circle = require "lib.Circle"
local Shared = require "shared"

function Bullet:new(Player, sprite, align)
	--[[ class constructor, takes player and a sprite]]
	self.sprite = sprite
	local posX = Player.rect.x + Player.rect.w / 2 - sprite:getWidth() / 2
	if align == 'left' then
		posX = Player.rect.left
	elseif align == 'right' then
		posX = Player.rect.right - sprite:getWidth()
	end
	self.speedY = 1000
	if Shared.debug then
		self.speedY = self.speedY / 2
	end
	self.radius = math.min( sprite:getWidth() / 2, sprite:getHeight() / 2)
	-- rectangle collider
	self.rect = Rectangle(posX,
						  Player.rect.y - sprite:getHeight() + 10,
						  sprite:getWidth(),
						  sprite:getHeight())
	-- circle collider
	self.circle = Circle(self.rect.x + self.radius,
						 self.rect.y + self.radius,
						 self.radius)
	self.active = true
end

function Bullet:update(dt)
	self.rect:updatePosition(0, self.speedY * dt * -1) -- negative as bullets go up
	self.circle:updatePosition(0, self.speedY * dt * -1)
	if self.rect.top <= 0 then
		self.active = false 
	end
	return self.active -- when false, bullet can be set to nil
end

function Bullet:draw()
	love.graphics.setColor(Shared.WHITE)
	love.graphics.draw(self.sprite, self.rect.x, self.rect.y)
	if Shared.debug then
		love.graphics.setColor(Shared.RED)
		love.graphics.circle('line', self.circle.x, self.circle.y, self.circle.radius)
		love.graphics.setColor(Shared.YELLOW)
		love.graphics.rectangle("line", self.rect.x, self.rect.y, self.rect.w, self.rect.h)
	end
end

return Bullet