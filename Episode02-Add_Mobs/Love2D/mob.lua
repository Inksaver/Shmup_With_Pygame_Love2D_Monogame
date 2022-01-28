local Class = require("lib.Class")
local Mob = Class:derive("Mob")
local Rectangle = require "lib.Rectangle"
local Shared = require "shared"

function Mob:new(width, height)
	--[[ class constructor ]]
	self.rect = Rectangle(0, 0, width, height)
	self:setProperties()
end

function Mob.setProperties(self)
	-- set speed to a random amount, so some will move faster than others
	self.rect.x = math.random(0, Shared.WIDTH - self.rect.width)	-- make the mob appear on screen randomly across it's width
	self.rect.y = math.random(-150, -100)					-- start off the top of the screen by random amount
	self.speedY = math.random(50, 600)
	self.speedX = math.random(-100, 100)
	if Shared.debug then 											-- reduce the speed
		self.speedY = math.random(10, 100)
		self.speedX = math.random(-10, 10)
	end
end

function Mob:getRect()
	return self.rect
end

function Mob:update(dt)
	self.rect:updatePosition(self.speedX * dt, self.speedY * dt)
	-- check if Mob has gone off bottom or sides of screen
	if self.rect.top > Shared.HEIGHT or self.rect.right < 0 or self.rect.left > Shared.WIDTH then
		Mob.setProperties(self) -- reset to position above top of the screen
	end
end

function Mob:draw()
	love.graphics.rectangle("fill", self.rect.x, self.rect.y, self.rect.w, self.rect.h)
end

return Mob
