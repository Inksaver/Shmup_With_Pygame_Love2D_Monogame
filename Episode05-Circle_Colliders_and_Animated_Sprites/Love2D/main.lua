 --[[ 
Lua / Love2D equivalent of Python / Pygame Shmup Episode 5 & 6:
https://www.youtube.com/watch?v=_y5U8tB36Vk
https://www.youtube.com/watch?v=3Bk-Ny7WLzE
Add remaining images into img folder
Meteors will be rotated
Multiple meteor images are randomly chosen
Introduction of circle collider
Draw circles around images
]]

local Shared 				 =  require "shared"	-- global variables for entire project
local Player 						-- declare Player local to this file
local Mob							-- declare Mob local to this file
local Bullet						-- declare Bullet local to this file
local newBulletTimerInterval = 0.2 	-- allow a new bullet every 0.2 seconds
local newBulletTimer 		 = 0	-- start timer at 0, update(dt) increases its value
local allowNewBullet 		 = true	-- change to false as soon as a new bullet is added to the bulletList
local mobList				 = {}	-- store all meteors (Mobs) in this table
local bulletList			 = {}	-- store all bullets created in this table
local sprites 				 = {}	-- table to hold all sprites

local function processEvents()
	--[[get keyboard input and check if user closed window]]
	local quit = false
	local keyboardState = love.keyboard
	if keyboardState.isDown('escape') then
		quit = true
	end
	
	return keyboardState, quit
end

local function checkMobBulletCollisions(dt)
	-- check if any bullets are colliding with any Mobs
	for i = #mobList, 1, -1 do								-- outer loop checks mobs
		local destroy = false								-- local boolean set to false
		for j = #bulletList, 1, -1 do						-- inner loop checks bullets
			--destroy = bulletList[j].rect:intersects(mobList[i]:getRect())
			destroy = bulletList[j].circle:intersects(mobList[i]:getCircle())
			if destroy then									
				mobList[i]:setProperties()					-- re-deploy mob first
				table.remove(bulletList, j)					-- delete bullet
				if #bulletList <= 0 then					-- if no more bullets break loop
					break
				end
			end
		end
	end
	-- update remaining bullets.
	for i = #bulletList, 1, -1 do						-- Go through the bulletList in reverse order. bullet:update(dt) returns true/false
		if not bulletList[i]:update(dt) then			-- update the bullet. If it is too far up the screen: (false)
			table.remove(bulletList, i)					-- remove it from the table
		end
	end
end

local function checkMobPlayerCollisions(keyboardState, dt)
	--[[ update Player and all mobs]]
	Player.update(keyboardState, dt)
	for i = #mobList, 1, -1 do							-- Go through the mobList
		mobList[i]:update(dt)
		--if Player.rect:intersects(mobList[i]:getRect()) then
		if Player.circle:intersects(mobList[i]:getCircle()) then
			if not Shared.debug then
				Shared.gamestate = Shared.gamestates['quit']
			end
			mobList[i]:setProperties()
		end
	end
end

local function shoot()
	--[[ fire bullet if enough time has passed  ]]
	if allowNewBullet then								 -- has enough time passed to fire a new bullet?
		table.insert(bulletList, Bullet(Player, sprites.bullet)) -- add a new bullet passing Player, width, height
		allowNewBullet = false							 -- prevent new bullets being made
		newBulletTimer = 0								 -- reset newBulletTimer to 0
	end
end

local function loadImages()
	sprites.background = love.graphics.newImage("img/starfield.png")
	sprites.player = love.graphics.newImage("img/PlayerShip1_orange.png")
	sprites.bullet = love.graphics.newImage("img/laserRed16.png")
	sprites.meteors = {}
	
	local meteorList = {'meteorBrown_big1.png', 'meteorBrown_big2.png',
						 'meteorBrown_med1.png', 'meteorBrown_med3.png',
						 'meteorBrown_small1.png', 'meteorBrown_small2.png',
						 'meteorBrown_tiny1.png'}
			   
	for i = 1, #meteorList do
		table.insert(sprites.meteors, love.graphics.newImage("img/"..meteorList[i]))
	end
end

function love.load()
	if arg[#arg] == "-debug" then
		print("running in debug mode") 
		require("mobdebug").start()
	end 
	print("To prevent this console window opening, delete the line: 't.console = true' in the conf.lua file")
	--[[ load all game assets ]]
	loadImages()
	Player 	= require "player"				-- import the Player static class
	Player.setProperties(sprites.player, 0.5, 500) 	-- setup player image, scale, speed
	Mob 	= require "mob"					-- import the Mob class
	-- make 8 Mobs (Dangerous rectangles) and store them in a list
	for i = 1, 8 do
		table.insert(mobList, Mob(sprites.meteors[math.random(1, #sprites.meteors)])) 
	end
	Bullet 	= require "bullet"				-- import the Bullet class
	Shared.gamestate = Shared.gamestates['play']
	Shared.debug = true
end

function love.update(dt)
	newBulletTimer = newBulletTimer + dt 				-- increase newBulletTimer by dt
	if newBulletTimer >= newBulletTimerInterval then	-- check if a new bullet can be created
		allowNewBullet = true							-- YAY! new bullet can be created
		newBulletTimer = 0 								-- reset newBulletTimer to 0
	end
	local keyboardState, quit = processEvents()
	
	if quit or Shared.gamestate == Shared.gamestates["quit"] then
		love.event.quit()
	else
		if Shared.gamestate == Shared.gamestates["play"] then
			if keyboardState.isDown("space") then		-- has the Player hit the space key to fire a bullet?
				shoot()
			end
			checkMobPlayerCollisions(keyboardState, dt)
			checkMobBulletCollisions(dt)
		end
	end
end

function love.draw()
	love.graphics.setColor(Shared.WHITE)
	love.graphics.draw(sprites.background,0,0)
	Player.draw()			-- draw Player. Note single dot .draw
	for i = 1, #bulletList do
		bulletList[i]:draw()
	end
	for i = 1, #mobList do
		mobList[i]:draw()
	end
end