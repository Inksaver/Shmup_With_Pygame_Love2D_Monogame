local Class = require("lib.Class")
local Rectangle = Class:derive("Rectangle")

--[[
	This class creates a rectangle from a given x,y width and height
	The properties available are:
	intersects(rect) : returns true if intersecting with given rectangle
	getRect() : returns x, y, width, height, centre {x,y}
	getCentre() : returns x and y of centre (or center with getCenter())
	setX() and setY() changes the x or y position and updates other properties
	update() : sets x and y to values given
	updatePosition : updates x and y by supplied values eg speed
]]

function Rectangle:new(x, y, width, height)
	--[[ class constructor ]]
	self.x = x
	self.y = y
	self.w = width
	self.h = height
	self.width = width		-- allow use of rect.w or rect.width
	self.height = height	-- allow use of rect.h or rect.height
	self.centre = {x = 0, y = 0}
	self:setProperties()
end

function Rectangle:intersects(rect)
	--[[ check whether rectangles are NOT colliding ]] 
	if rect.w == nil then rect.w = rect.width end
	if rect.h == nil then rect.h = rect.height end
	-- if left side of rect1 is beyond rect2 right side
	-- OR left side of rect2 is beyond rect1 right side
	if self.x > rect.x + rect.w or rect.x > self.x + self.w then
		return false
	end
	-- if top side of rect1 is beyond bottom of rect2
	-- OR top side of rect2 is beyond bottom of rect1
	if self.y > rect.y + rect.h or rect.y > self.y + self.h then
		return false
	end
	-- code only gets this far if both if statements above fail 
	return true
end

function Rectangle:setProperties()
	self.top = self.y
	self.left = self.x
	self.bottom = self.y + self.height
	self.right = self.x + self.width
	self.centre.x = self.x + self.width / 2
	self.centre.y = self.y + self.height / 2
end

function Rectangle:getRect()
	return
	{
		x = self.x,
		y = self.y,
		w = self.width,
		h = self.height,
		width = self.width,		-- allow use of rect.w or rect.width
		height = self.height,	-- allow use of rect.h or rect.height
		centre = {self.centre.x, self.centre.y},
		center = centre			-- allow use of uk/us spelling
	}
end

function Rectangle:getCentre()
	return self.centre.x, self.centre.y
end

function Rectangle:getCenter() -- allow use of uk/us spelling
	return self.centre.x, self.centre.y
end

function Rectangle:setX(value)
	self.x = value
	self:setProperties()
end

function Rectangle:setY(value)
	self.y = value
	self:setProperties()
end

function Rectangle:update(x, y) -- update from absolute values of x,y
	--[[ usage: rect:update(nil, 50) to update y only]]
	if x ~= nil then
		self.x = x
	end
	if y ~= nil then
		self.y = y
	end
	self:setProperties()
end

function Rectangle:updatePosition(speedx, speedy) -- update from changes in speed
	--[[ usage: rect:updatePosition(10, nil) or (10, 0) to update x value only]]
	if speedx ~= nil and speedx ~= 0 then
		self.x = self.x + speedx
	end
	if speedy ~= nil and speedy ~= 0 then
		self.y = self.y + speedy
	end
	self:setProperties()
end

return Rectangle