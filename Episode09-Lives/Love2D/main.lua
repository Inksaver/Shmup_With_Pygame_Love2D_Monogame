--[[ 
Lua / Love2D equivalent of Python / Pygame Shmup Episode 11
https://www.youtube.com/watch?v=G5-4nV6LxgU
Lives

Background music: Frozen Jam by tgfcoder <https://twitter.com/tgfcoder> licensed under CC-BY-3 
]]
local Shared 			= require "shared"		-- global variables for entire project
local Player 			= require "player"		-- import the Player static class
local Mob 				= require "mob"			-- import the Mob class
local Bullet 			= require "bullet"		-- import the Bullet class
local Shield  			= require "shield"
local Explosion 		= require "explosion"
local newBulletTimerInterval = 0.2 	-- allow a new bullet every 0.2 seconds
local newBulletTimer 	= 0			-- start timer at 0, update(dt) increases its value
local allowNewBullet 	= true		-- change to false as soon as a new bullet is added to the bulletList
local mobList			= {}		-- store all meteors (Mobs) in this table
local bulletList		= {}		-- store all bullets created in this table
local sprites 			= {}		-- table to hold all sprites
local sounds			= {}		-- table to hold audio files
local explosions 		= {}
local explosionList 	= {}
local deathExplosion 	= nil

local function drawLives(x, y, lives, scale)
	--[[ draw up to 3 mini ships to represent lives left ]]
	for i = 0, lives - 1 do
		love.graphics.draw(sprites.player, x + 30 * i , y, nil, scale)
	end
end

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
			--[[ destroy set to true if rectangles are colliding (bullet + Mob) ]]
			--destroy = bulletList[j].rect:intersects(mobList[i]:getRect())
			destroy = bulletList[j].circle:intersects(mobList[i]:getCircle())
			if destroy then									
				love.audio.stop(sounds.shoot)
				mobList[i]:setProperties()					-- re-deploy mob first
				Shared.score = Shared.score + math.floor(mobList[i]:getCircle().radius)
				if mobList[i]:getCircle().radius < 15 then
					love.audio.play(sounds.expl1)
				else
					love.audio.play(sounds.expl2)
				end
				table.insert(explosionList, Explosion(explosions.mob, {bulletList[j].rect.centre.x, bulletList[j].rect.top}, 0.5))
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
	if Player.alive then -- still has 1 or more lives
		Player.update(keyboardState, dt)
	else -- 0 lives so allow for finish of explosion sound and animation
		if deathExplosion == nil then -- Player dead, explosion animation finished
			if not sounds.die:isPlaying( ) then -- is explosion audio completed?
				Shared.gamestate = Shared.gamestates['quit']
			end
		end
	end
	for i = #mobList, 1, -1 do							-- Go through the mobList
		mobList[i]:update(dt)
		if not Player.hidden then
			--if Player.rect:intersects(mobList[i]:getRect()) then
			if Player.circle:intersects(mobList[i]:getCircle()) then
				mobList[i]:setProperties()
				Player.shield = Player.shield - mobList[i]:getCircle().radius
				love.audio.stop(sounds.shoot)
				if Player.shield <= 0 then -- lose a life
					Player.lives = Player.lives - 1
					Player.shield = 100
					love.audio.stop(sounds.expl3)
					love.audio.play(sounds.die)
					deathExplosion = Explosion(explosions.player, {Player.rect.centre.x, Player.rect.top}, 0.8)
					if Player.lives <= 0 then
						Player.alive = false
					else
						Player.hide()
					end
				else -- no lives lost but play sound of player hit
					love.audio.play(sounds.expl3)
				end
			end
		end
	end
end

local function shoot()
	--[[ fire bullet if enough time has passed  ]]
	if allowNewBullet and not Player.hidden then								 -- has enough time passed to fire a new bullet?
		love.audio.stop(sounds.shoot)
		table.insert(bulletList, Bullet(Player, sprites.bullet)) -- add a new bullet passing Player, width, height
		allowNewBullet = false							 -- prevent new bullets being made
		newBulletTimer = 0								 -- reset newBulletTimer to 0
		love.audio.play(sounds.shoot)
	end
end

local function updateExplosions(dt)
	-- [[update all explosions. remove any non-active]]
	for i = #explosionList, 1, -1 do						-- Go through the explosions in reverse order. :update(dt) returns true/false
		if not explosionList[i]:update(dt) then				-- update. false if end of frames
			table.remove(explosionList, i)					-- remove it from the table
		end
	end
	if deathExplosion ~= nil then
		if not deathExplosion:update(dt) then
			deathExplosion = nil
		end
	end
end

local function loadImages()
	sprites.background = love.graphics.newImage("img/starfield.png")
	sprites.player = love.graphics.newImage("img/PlayerShip1_orange.png")
	sprites.bullet = love.graphics.newImage("img/laserRed16.png")
	sprites.meteors = {}
	
	local meteor_list = {'meteorBrown_big1.png', 'meteorBrown_big2.png',
						 'meteorBrown_med1.png', 'meteorBrown_med3.png',
						 'meteorBrown_small1.png', 'meteorBrown_small2.png',
						 'meteorBrown_tiny1.png'}
			   
	for i = 1, #meteor_list do
		table.insert(sprites.meteors, love.graphics.newImage("img/"..meteor_list[i]))
	end
end

local function loadAudio()
	sounds.shoot = love.audio.newSource("snd/Laser_Shoot6.wav", "static" )
	sounds.shield = love.audio.newSource("snd/pow4.wav", "static" )
	sounds.power = love.audio.newSource("snd/pow5.wav", "static" )
	sounds.die = love.audio.newSource("snd/rumble1.ogg", "static" )
	sounds.expl1 = love.audio.newSource("snd/expl3.wav", "static" )
	sounds.expl2 = love.audio.newSource("snd/expl6.wav", "static" )
	sounds.expl3 = love.audio.newSource("snd/Explosion5.wav", "static" )
	sounds.background = love.audio.newSource("snd/FrozenJam.ogg", "stream" )
	for k,v in pairs(sounds) do -- set volume for all sounds
		v:setVolume(0.2)
	end
	sounds.background:setVolume(0.1) -- reduce background further
end

local function loadAnimations()
	--[[ load explosion images and scale for second list ]]
	explosions.mob = {}
	explosions.player = {}

	for i = 0, 8 do
		local filename = 'regularExplosion0' .. i .. '.png' -- clever way of adding sequential filenames
		table.insert(explosions.mob, love.graphics.newImage("img/".. filename))
		filename = 'sonicExplosion0' .. i .. '.png' -- clever way of adding sequential filenames
		table.insert(explosions.player, love.graphics.newImage("img/".. filename))
	end
end

function love.load()
	if arg[#arg] == "-debug" then
		print("running in debug mode")
		require("mobdebug").start()
	end 
	print("To prevent this console window opening, delete the line: 't.console = true' in the conf.lua file")
	love.window.setMode(Shared.WIDTH, Shared.HEIGHT, {display = 2} ) -- display on second monitor
	--[[ load all game assets ]]
	loadImages()
	loadAudio()
	loadAnimations()
	Player 	= require "player"				-- import the Player static class
	Player.setProperties(sprites.player, 0.5, 500) -- setup player image, scale, speed
	Mob 	= require "mob"					-- import the Mob class
	-- make 8 Mobs (meteors) and store them in a list
	for i = 1, 8 do
		table.insert(mobList, Mob(sprites.meteors[math.random(1, #sprites.meteors)])) 
	end
	Bullet 	= require "bullet"				-- import the Bullet class
	Shield  = require "shield"
	Explosion = require "explosion"
	Shared.gamestate = Shared.gamestates['play']
	Shared.debug = false
	love.audio.play(sounds.background)
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
			if keyboardState.isDown("space") then			-- has the Player hit the space key to fire a bullet?
				shoot()
			end
			checkMobPlayerCollisions(keyboardState, dt)
			checkMobBulletCollisions(dt)
			updateExplosions(dt)
		end
	end
	if not sounds.background:isPlaying( ) then
		love.audio.play(sounds.background)
	end
	Shield.update(Player.shield)
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
	for i = 1, #explosionList do
		explosionList[i]:draw()
	end
	if deathExplosion ~= nil then
		deathExplosion:draw()
	end
	-- drawText(text, size, x, y, align, colour)
	Shared.drawText(Shared.score, 'size24', 0, 10, 'center')
	drawLives(Shared.WIDTH - 100, 5, Player.lives, 0.2)
	Shield.draw()
	if Shared.debug then
		Shared.drawText("Debug mode", 'size18', 10, Shared.HEIGHT - 24, "left", Shared.YELLOW)
	end
end