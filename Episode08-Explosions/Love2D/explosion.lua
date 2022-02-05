local Class = require("lib.Class")
local Explosion = Class:derive("Explosion")
local Rectangle = require "lib.Rectangle"

function Explosion:new(spriteList, centre, scale)
	--[[ class constructor, takes table of animation images
		 centre of the mob destroyed {x,y} , and scale factor]]
	self.centre = centre
	self.spriteList = spriteList 	-- table of images
	self.scale = scale
	self.image = spriteList[1] 		-- set to first image in the sequence
	self.frame = 1
	self.timePassed = 0
	self.frameRate = 0.1
	self.active = true
	self.rect = Rectangle(centre[1] - (self.image:getWidth() / 2) * self.scale,
						  centre[2] - (self.image:getHeight() / 2) * self.scale,
						  self.image:getWidth(),
						  self.image:getHeight())
end

function Explosion:update(dt)
	self.timePassed = self.timePassed + dt
	if self.timePassed >= self.frameRate then
		self.timePassed = 0
		self.frame = self.frame + 1
		if self.frame > #self.spriteList then
			self.frame = #self.spriteList
			self.active = false
		else
			-- ensure image sequence is centered per frame 
			self.image = self.spriteList[self.frame] --next frame
			self.rect:update(self.centre[1] - (self.image:getWidth() / 2) * self.scale,
							 self.centre[2] - (self.image:getHeight() / 2) * self.scale)
		end
	end
	return self.active -- when false, explosion can be set to nil
end

function Explosion:draw()
	-- love.graphics.draw( drawable, x, y, r, sx, sy, ox, oy, kx, ky )
	if self.active then
		love.graphics.draw(self.image, self.rect.x, self.rect.y, nil, self.scale, self.scale)
	end
end

return Explosion
