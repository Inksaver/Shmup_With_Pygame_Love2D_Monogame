local Class = require "lib.Class"
local Powerup = Class:derive("Powerup")
local Rectangle = require "lib.Rectangle"
local Circle = require "lib.Circle"
local Shared = require "shared"

function Powerup:new(spriteList, centre)
	--[[ class constructor, takes table of images
		 centre of the mob destroyed {x,y} 
		spriteList = {shield = <image>, gun = <image>}
	]]
	self.centre = centre
	self.spriteList = spriteList 	-- table of images
	-- choose random image
	local keys = {}
	for k,v in pairs(spriteList) do
		table.insert(keys, k)
	end
	-- keys = {"shield", "gun"}
	self.type = keys[math.random(1, #keys)] -- math.random returns 1 or 2, keys[1] returns 'gun'
	self.image = spriteList[self.type] 
	self.speedY = 400
	if Shared.debug then
		self.speedY = 200
	end
	self.active = true
	self.rect = Rectangle(centre[1] - (self.image:getWidth() / 2),
						  centre[2] - (self.image:getHeight() / 2),
						  self.image:getWidth(),
						  self.image:getHeight())
	self.circle = Circle(self.rect.centre.x, self.rect.centre.y, self.image:getWidth() / 2)
end

function Powerup:getCircle()
	return self.circle
end

function Powerup:getRect()
	return self.rect
end

function Powerup:update(dt)
	self.rect:updatePosition(0, self.speedY * dt)
	self.circle:update(self.rect.centre.x, self.rect.centre.y)
	-- check if Powerup has gone off bottom of screen
	if self.rect.top >Shared.HEIGHT then
		self.active = false
	end
	return self.active -- when false, powerup can be set to nil
end

function Powerup:draw()
	-- love.graphics.draw( drawable, x, y, r, sx, sy, ox, oy, kx, ky )
	if self.active then
		love.graphics.draw(self.image, self.rect.x, self.rect.y)
		if Shared.debug then
			love.graphics.setColor(Shared.RED)
			love.graphics.rectangle("line", self.rect.x, self.rect.y, self.rect.w, self.rect.h)
			love.graphics.setColor(Shared.WHITE)
		end
	end
end

return Powerup