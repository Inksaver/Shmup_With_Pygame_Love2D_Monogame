local Class = require "lib.Class"
local Powerup = Class:derive("Powerup")
local Rectangle = require "lib.Rectangle"
local Circle = require "lib.Circle"
local Shared = require "shared"

function Powerup:new(images, centre)
	--[[ class constructor, takes table of images
		 centre of the mob destroyed {x,y} 
		images = {shield = <image>, gun = <image>}
	]]
	self.centre = centre
	self.images = images 	-- table of images
	-- choose random image
	local keys = {}
	for k,v in pairs(images) do
		table.insert(keys, k)
	end
	-- keys = {"shield", "gun"}
	self.key = keys[math.random(1, #keys)] -- math.random returns 1 or 2, keys[1] returns 'gun'
	self.image = images[self.key] 
	self.speedY = 500
	if Shared.debug then
		self.speedY = 250
	end
	self.active = true
	self.rect = Rectangle(centre[1] - (self.image:getWidth() / 2),
						  centre[2] - (self.image:getHeight() / 2),
						  self.image:getWidth(),
						  self.image:getHeight())
	self.circle = Circle(self.rect.centre.x, self.rect.centre.y, self.image:getWidth() / 2)
end

function Powerup:update(dt)
	self.rect:updatePosition(0, self.speedY * dt)
	self.circle:update(self.rect.centre.x, self.rect.centre.y)
	-- check if Mob has gone off bottom or sides of screen
	if self.rect.top > Shared.HEIGHT or self.rect.right < 0 or self.rect.left >Shared.WIDTH then
		self.active = false
	end
	return self.active -- when false, explosion can be set to nil
end

function Powerup:draw()
	-- love.graphics.draw( drawable, x, y, r, sx, sy, ox, oy, kx, ky )
	love.graphics.draw(self.image, self.rect.x, self.rect.y)
	if Shared.debug then
		love.graphics.setColor(Shared.RED)
		love.graphics.rectangle("line", self.rect.x, self.rect.y, self.rect.w, self.rect.h)
		love.graphics.setColor(Shared.WHITE)
	end
end

return Powerup