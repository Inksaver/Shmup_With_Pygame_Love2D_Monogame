local Class = require("lib.Class")
local Mob = Class:derive("Mob")
local Shared = require "shared"

function Mob:new(width, height)
	--[[ class constructor ]]
	self.w = width
	self.h = height
	Mob.setProperties(self)
end

function Mob.setProperties(self)
	-- set speed to a random amount, so some will move faster than others
	self.x = math.random(0, Shared.WIDTH - self.w)	-- make the mob appear on screen randomly across it's width
	self.y = math.random(-150, -100)			-- start off the top of the screen by random amount
	self.speedY = math.random(40, 600)
	self.speedX = math.random(-200, 200)
end

function Mob:getRect()
	rect = {}
	rect.x = self.x
	rect.y = self.y
	rect.w = self.w
	rect.h = self.h
	return rect
end

function Mob:update(dt)
	self.y = self.y + self.speedY * dt
	self.x = self.x + self.speedX * dt
	-- check if Mobob has gone off bottom or sides of screen
	if self.y >Shared.HEIGHT or self.x < 0 - self.w or self.x > Shared.WIDTH then
		Mob.setProperties(self) -- reset to position above top of the screen
	end
end

function Mob:draw()
	love.graphics.rectangle("fill", self.x, self.y, self.w, self.h)
end

return Mob