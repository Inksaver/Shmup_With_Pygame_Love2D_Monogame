local Class = require("lib.Class")
local Mob = Class:derive("Mob")
local Rectangle = require "lib.Rectangle"
local Shared = require "shared"

function Mob:new(sprite)
	--[[ class constructor now takes a sprite parameter]]
	self.sprite = sprite
	self.rect = Rectangle(0, 0, sprite:getWidth(), sprite:getHeight())
	self:setProperties()
end

function Mob:setProperties()
	-- set speed to a random amount, so some will move faster than others
	self.rect.x = math.random(0, Shared.WIDTH - self.sprite:getWidth())	-- make the mob appear on screen randomly across it's width
	self.rect.y = math.random(-150, -100)			-- start off the top of the screen by random amount
	self.speedY = math.random(50, 600)
	self.speedX = math.random(-100, 100)
	if Shared.debug then
		self.speedY = math.random(10, 100)
		self.speedX = math.random(-20, 20)
	end
end

function Mob:getRect()
	return self.rect
end

function Mob:update(dt)
	self.rect:updatePosition(self.speedX * dt, self.speedY * dt)
	-- check if Mob has gone off bottom or sides of screen
	if self.rect.top > Shared.HEIGHT or self.rect.right < 0 or self.rect.left > Shared.WIDTH then
		self:setProperties() -- reset to position above top of the screen
	end
end

function Mob:draw()
	love.graphics.draw(self.sprite, self.rect.x, self.rect.y)
	if Shared.debug then
		love.graphics.setColor(Shared.RED)
		love.graphics.rectangle("line", self.rect.x, self.rect.y, self.rect.w, self.rect.h)
		love.graphics.setColor(Shared.WHITE)
	end
end

return Mob