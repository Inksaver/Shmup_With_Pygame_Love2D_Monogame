local Class = require("lib.Class")
local Mob = Class:derive("Mob")
local Rectangle = require "lib.Rectangle"
local Circle = require "lib.Circle"
local Shared = require "shared"

local function rotatingRectangle( mode, x, y, w, h, a, ox, oy )
	-- if no ox or oy provided, rotation is around upper left corner
	-- if ox, oy = w/2, h/2 then rotation is around rectangle's center
	-- if no angle provided, no rotation
	ox = ox or 0
	oy = oy or 0
	a = a or 0
	love.graphics.push()
	love.graphics.translate( x, y )
	love.graphics.rotate( a )
	love.graphics.rectangle( mode,-ox, -oy, w, h )
	love.graphics.pop()
end

function Mob:new(sprite)
	--[[ class constructor now takes a sprite parameter]]
	self.sprite = sprite
	self.rect = Rectangle(0, 0, sprite:getWidth(), sprite:getHeight())
	self.radius = math.min( sprite:getWidth() / 2, sprite:getHeight() / 2)
	self.circle = Circle(0, 0, self.radius)
	self:reset()
end

function Mob.reset(self)
	--[[ set speed to a random amount, so some will move faster than others ]]
	self.ox = self.rect.width / 2
	self.oy = self.rect.height / 2
	local x = math.random(0, Shared.WIDTH - self.sprite:getWidth())
	local y = math.random(-150, -100)
	self.speedY = math.random(30, 300)
	self.speedX = math.random(-200, 200)
	if Shared.debug then
		self.speedY = math.random(30, 100)
		self.speedX = math.random(-20, 20)
	end
	self.rect:setX(x)	-- make the mob appear on screen randomly across it's width
	self.rect:setY(y)	-- start off the top of the screen by random amount
	self.circle:update(x + self.radius, y + self.radius)
	self.rotation = 0
	self.rotationSpeed = math.random(-math.pi, math.pi) * 0.5 -- math.pi / 0.5 -- 1 rotation per second
end

function Mob.rotate(self, dt) -- dt is seconds since last update
	self.rotation = self.rotation + self.rotationSpeed * dt
	if self.rotation > math.pi * 2 then
		self.rotation = 0
	end
end 

function Mob.GetRotatedVertices(self)
	local r = math.sqrt(self.rect.w * self.rect.w / 4 + self.rect.h * self.rect.h / 4);
	local thetas = {}
	table.insert(thetas, math.atan((self.rect.h / 2) / (self.rect.w / 2)))
	table.insert(thetas, thetas[1] * -1 + self.rotation )
	table.insert(thetas, thetas[1] - math.pi + self.rotation)
	table.insert(thetas, math.pi - thetas[1] + self.rotation)
	thetas[1] = thetas[1] + self.rotation
	
	local vertices = {}
	for _,theta in pairs(thetas) do
		table.insert(vertices, math.cos(theta) * r + self.rect.centre.x)
		table.insert(vertices, math.sin(theta) * r + self.rect.centre.y)
	end
	return vertices	
end

function Mob:update(dt)
	Mob.rotate(self, dt)	
	self.rect:updatePosition(self.speedX * dt, self.speedY * dt)
	self.circle:update(self.rect.x + self.radius, self.rect.y + self.radius)
	-- check if Mob has gone off bottom or sides of screen
	if self.rect.top > Shared.HEIGHT or self.rect.right < 0 or self.rect.left > Shared.WIDTH then
		self:reset() -- reset to position above top of the screen
	end
end

function Mob:draw()
	love.graphics.setColor(Shared.WHITE)
	--[[ draw sprite using offset values twice, so colliders are in correct position ]]
	love.graphics.draw(self.sprite, self.rect.x + self.ox, self.rect.y + self.oy, self.rotation, nil, nil, self.ox, self.oy)
	if Shared.debug then
		love.graphics.setColor(Shared.RED)
		--[[ draw circle collider ]]
		love.graphics.circle('line', self.circle.x, self.circle.y, self.circle.radius)

		love.graphics.setColor(Shared.GREEN)
		--[[ draw rotating rectangle ]]
		--rotatingRectangle('line', self.rect.centre.x, self.rect.centre.y, self.rect.w, self.rect.h, self.rotation, self.ox, self.oy )
		love.graphics.polygon('line', self:GetRotatedVertices())
		
		love.graphics.setColor(Shared.BLUE)
		--[[ draw rectangle collider ]]
		love.graphics.rectangle('line', self.rect.x, self.rect.y, self.rect.w, self.rect.h)
		love.graphics.setColor(Shared.WHITE)
	end
end

return Mob