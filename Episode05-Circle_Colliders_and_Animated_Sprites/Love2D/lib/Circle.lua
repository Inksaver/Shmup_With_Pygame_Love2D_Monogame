local Class = require("lib.Class")
local Circle = Class:derive("Circle")

--[[
	This class creates a circle from a given x, y, radius
	The properties available are:
	intersects(circle) : returns true if intersecting with given circle
	getCircle() : returns x, y, radius
	getCentre() : returns x and y of centre (or center with getCenter())
	setX() and setY() changes the x or y position
	update() : sets x and y to values given
	updatePosition : updates x and y by supplied values eg speed
]]

function Circle:new(x, y, radius)
	--[[ class constructor ]]
	self.x = x
	self.y = y
	self.radius = radius
	self.centre = {x = 0, y = 0}
	self:setProperties()
end

function Circle:intersects(circle)
	-- love.graphics.circle('line', x, y, radius)
	-- get distance between the circle's centers
	-- use the Pythagorean Theorem to compute the distance
	local distX = self.x - circle.x
	local distY = self.y - circle.y
	local distance = math.sqrt((distX * distX) + (distY * distY))	
	-- if the distance is less than the sum of the circle's
	-- radii, the circles are touching!
	if distance <= self.radius + circle.radius then
		return true, distance
	end
	return false , distance
end

function Circle:setProperties()
	self.top = self.y - self.radius
	self.left = self.x - self.radius
	self.bottom = self.y + self.radius
	self.right = self.x + self.radius
	self.centre.x = self.x
	self.centre.y = self.y
end

function Circle:getCircle()
	return
	{
		x = self.x,
		y = self.y,
		radius = self.radius,
		centre = self.centre,
		center = self.centre	-- allow use of uk/us spelling
	}
end

function Circle:getCentre()
	return self.x, self.y
end

function Circle:getCenter() -- allow use of uk/us spelling
	return self.x, self.y
end

function Circle:setX(value)
	self.x = value
	self:setProperties()
end

function Circle:setY(value)
	self.y = value
	self:setProperties()
end

function Circle:update(x, y)
	--[[ usage: circle:update(nil, 50) to update y only]]
	if x ~= nil then
		self.x = x
	end
	if y ~= nil then
		self.y = y
	end
	self:setProperties()
end

function Circle:updatePosition(speedx, speedy)
	--[[ usage: rect:updatePosition(10, nil) or (10, 0) to update x value only]]
	if speedx ~= nil and speedx ~= 0 then
		self.x = self.x + speedx
	end
	if speedy ~= nil and speedy ~= 0 then
		self.y = self.y + speedy
	end
	self:setProperties()
end

return Circle
